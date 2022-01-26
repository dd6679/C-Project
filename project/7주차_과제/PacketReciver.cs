using System;

namespace VMS.Client2.Net
{
    public delegate int ReceiveHandler(byte[] buffer, int offset, int length);
    public delegate void CompleteHandler(Packet packet);

    internal class PacketReciver : IPacketReciver
    {
        private Packet currentPacket;
        private enum CommSteps
        {
            RecvHeadStep,
            RecvBodyStep
        }

        private ReceiveHandler OnReceive;
        private CompleteHandler OnComplete;

        public PacketReciver(ReceiveHandler onReceive, CompleteHandler onComplete)
        {
            OnReceive = onReceive;
            OnComplete = onComplete;
        }
           
        int recvBufferPos = 0;              // 수신된 데이터
        int currentPacketRecvPos = 0;       // 처리된 데이터
        int pos = 0;                        //   
        static int recvBufferSize = 8192;   // 버퍼 갯수 
        byte[] recvBuffer = new byte[recvBufferSize];

        public bool Receive()
        {
            CommSteps currentStep = CommSteps.RecvHeadStep;
            try
            {
                while (true)
                {
                    int length = OnReceive(recvBuffer, recvBufferPos, recvBufferSize - recvBufferPos);
                    if (length > 0) // 리턴 사이즈가 없거나, 모자라면 끊긴것으로 봄.
                    {
                        recvBufferPos += length;
                        while (pos < recvBufferPos)
                        {
                            if (currentStep == CommSteps.RecvHeadStep)
                            {
                                if (recvBufferPos - pos >= PacketHeader.CommHeadSize)
                                {
                                    pos = Packet.CopyHeader(recvBuffer, pos, ref currentPacket);
                                    if (pos < 0)
                                    {
                                        Console.WriteLine("Packet Header make failed"); // 헤더를 만들지 못하게 된다면 끊김.
                                        return false;
                                    }

                                    currentPacketRecvPos = PacketHeader.CommHeadSize;
                                    currentStep = CommSteps.RecvBodyStep;
                                    continue; // 헤더 처리 종료
                                }
                                else 
                                {
                                    RearangeBuffer();
                                    break;  
                                }
                            }
                            else
                            {
                                int len = (int)Math.Min(currentPacket.Header.AllocSize - currentPacketRecvPos, recvBufferPos - pos);
                                Buffer.BlockCopy(recvBuffer, pos, currentPacket.Data, currentPacketRecvPos, len);
                                currentPacketRecvPos += len;
                                pos += len;

                                if (currentPacketRecvPos == currentPacket.Header.AllocSize)
                                {
                                    OnComplete(currentPacket);
                                    currentStep = CommSteps.RecvHeadStep;

                                    RearangeBuffer();
                                    return true;
                                }
                                else
                                {
                                    RearangeBuffer();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("접속 끊김");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        private void RearangeBuffer()
        {
            if (pos < recvBufferPos)
            {
                Buffer.BlockCopy(recvBuffer, pos, recvBuffer, 0, recvBufferPos - pos);
                recvBufferPos = recvBufferPos - pos;
            }
            else                    // 수신데이터가 남지 않았으면 수신위치 리셋 
            {
                recvBufferPos = 0;
            }
            pos = 0;
        }
    }
}
