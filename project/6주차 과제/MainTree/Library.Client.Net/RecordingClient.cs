using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Client.Net.Constance;
using Library.Client.Net.DataStruct;
using Library.Client.Net.Network;
using Newtonsoft.Json;

namespace Library.Client.Net
{
    public class RecordingClient : CommonClient, IDisposable
    {
        protected PacketProcessor<Commands> _processor = new PacketProcessor<Commands>();
        public int vmsId, srvSerial, devSerial;
        public StreamInfo streamInfo;
        protected StreamManager _streamMenager = StreamManager.getManager();

        public RecordingClient(int vmsId, int srvSerial, int devSerial)
        {
            this.vmsId = vmsId;
            this.srvSerial = srvSerial;
            this.devSerial = devSerial;

            _processor.Regist(Commands.DrvRecCmdDeviceStream, DrvRecDeviceStream);
            _processor.Regist(Commands.DrvRecCmdStream, DrvRecStream);
        }

        public override void OnProcess(Packet packet)
        {
            var handler = _processor.Resolve((Commands)(packet.Header.Command - (ushort)CommEventEnum.SrvCommandStartUser));
            if (handler != null)
            {
                handler(packet);
            }
            else
            {
                var msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
                Console.WriteLine($"Unknown Command:{(Commands)packet.Header.Command}, Data:{msg}");
            }
        }

        private void DrvRecDeviceStream(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            Console.WriteLine($"ReqDeviceInfo: {msg}");
        }

        private void DrvRecStream(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.Value);
            streamInfo = JsonConvert.DeserializeObject<StreamInfo>(msg);
            var key = vmsId + ":" + devSerial + ":" + _stream.DchCh + ":" + _stream.DchmSerial;
            if (streamInfo.SteamType == StreamTypes.Video)
            {
                var handler = _streamMenager.Resolve(key);
                if (handler != null)
                {
                    Console.WriteLine(msg);
                    handler(streamInfo, packet.Data, PacketHeader.CommHeadSize + (int)packet.Header.Value, (int)packet.Header.AllocSize - (PacketHeader.CommHeadSize + (int)packet.Header.Value));
                }
            }
        }

        public void RequestStream()
        {
            _stream.cmd = Commands.DrvRecCmdDeviceStream;
            string json = JsonConvert.SerializeObject(_stream);

            // 패킷 할당 및 데이터 복사 (32바이트 이후)
            var command = (ushort)CommEventEnum.SrvCommandStartUser + (ushort)_stream.cmd;
            var packet = Packet.MakePaket((ushort)command, 0, json, 0, 0, _stream.tran);
            // 패킷 헤더 복사(바이트 0~15)
            PacketHeader.Encode(ref packet.Header, packet.Data, 0);

            SetTran(packet);

            _sendQueue.Add(packet);
        }

        public void Dispose()
        {
            OnDisconnected(this);
            Logout();
        }
    }
}
