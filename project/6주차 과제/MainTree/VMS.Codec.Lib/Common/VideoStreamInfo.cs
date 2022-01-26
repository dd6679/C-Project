using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VMS.Codec.Lib
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class VideoStreamInfo : CommonInOut
    {
        public const int SIZE = 64;

        public BitmapInfoHeader bih = new BitmapInfoHeader();
        public long ts { get; set; }
        public long tcl { get; set; }
        public StreamTypeEnum streamType;
        public FrameTypeEnum frameInfo;
        public byte reserved;
        public ushort streamInfo;

        public VideoStreamInfo()
        {
            Init();
        }

        public override string ToString()
        {
            return string.Format("VideoStreamInfo[{0}x{1},{2},{3},{4}]",
                bih.biWidth,
                bih.biHeight,
                streamType,
                frameInfo,
                DateTime.FromFileTime(tcl).ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        public void Init()
        {
        }

        public DateTime GetTs()
        {
            return DateTime.FromFileTime(ts);
        }

        public DateTime GetTcl()
        {
            return DateTime.FromFileTime(tcl);
        }


        public bool EncodeStream(System.IO.Stream handler)
        {
            bool success = false;
            try
            {
                do
                {
                    if (!bih.EncodeStream(handler))
                        break;

                    handler.Write(BitConverter.GetBytes(ts), 0, 8);
                    handler.Write(BitConverter.GetBytes(tcl), 0, 8);

                    handler.Write(BitConverter.GetBytes((int)streamType), 0, 4);
                    handler.WriteByte((byte)frameInfo);
                    handler.WriteByte((byte)reserved);
                    handler.Write(BitConverter.GetBytes(streamInfo), 0, 2);

                    success = true;
                } while (false);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return success;
        }

        public bool DecodeStream(System.IO.Stream handler)
        {
            bool success = false;
            try
            {
                byte[] buffer = new byte[SIZE];
                int pos = 0;

                if (handler.Read(buffer, 0, buffer.Length) != SIZE)
                {
                    //logger.Error("Read Failed");
                    Debug.WriteLine("Video Info Read Failed");
                    return false;
                }

                pos = bih.DecodeMemory(buffer, pos);
                ts = BitConverter.ToInt64(buffer, pos);
                pos += 8;
                tcl = BitConverter.ToInt64(buffer, pos);
                pos += 8;

                streamType = (StreamTypeEnum)BitConverter.ToInt32(buffer, pos);
                pos += 4;
                frameInfo = (FrameTypeEnum)buffer[pos];
                pos++;
                reserved = buffer[pos];
                pos++;
                streamInfo = BitConverter.ToUInt16(buffer, pos);
                pos += 2;

                System.Diagnostics.Debug.Assert(pos == SIZE);
                success = pos == SIZE;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return success;
        }

        public int EncodeMemory(byte[] data, int from)
        {
            try
            {
                if (data.Length - from < SIZE)
                {
                    Debug.WriteLine("Assigned memory is small");
                    return from;
                }

                int pos = from;

                pos = bih.EncodeMemory(data, pos);

                Array.Copy(BitConverter.GetBytes(ts), 0, data, pos, 8);
                pos += 8;
                Array.Copy(BitConverter.GetBytes(tcl), 0, data, pos, 8);
                pos += 8;

                Array.Copy(BitConverter.GetBytes((int)streamType), 0, data, pos, 4);
                pos += 4;
                data[pos] = (byte)frameInfo;
                pos++;
                data[pos] = (byte)reserved;
                pos++;
                Array.Copy(BitConverter.GetBytes(streamInfo), 0, data, pos, 2);
                pos += 2;

                System.Diagnostics.Debug.Assert(pos - from == SIZE);

                from = pos;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return from;
        }

        public int DecodeMemory(byte[] data) => this.DecodeMemory(data, 0);

        public int DecodeMemory(byte[] data, int from)
        {
            try
            {
                if (data.Length - from < SIZE)
                {
                    Debug.WriteLine("Assigned memory is small");
                    return from;
                }

                int pos = from;

                pos = bih.DecodeMemory(data, pos);
                ts = BitConverter.ToInt64(data, pos);
                pos += 8;
                tcl = BitConverter.ToInt64(data, pos);
                pos += 8;

                streamType = (StreamTypeEnum)BitConverter.ToUInt32(data, pos);
                pos += 4;
                frameInfo = (FrameTypeEnum)data[pos];
                pos++;
                reserved = data[pos];
                pos++;
                streamInfo = BitConverter.ToUInt16(data, pos);
                pos += 2;

                System.Diagnostics.Debug.Assert(pos - from == SIZE);
                from = pos;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return from;
        }
    }
}
