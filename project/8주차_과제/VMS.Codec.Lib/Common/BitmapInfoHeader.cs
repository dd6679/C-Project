using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VMS.Codec.Lib
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class BitmapInfoHeader : CommonInOut
    {
        public const int SIZE = 40;

        public BitmapInfoHeader()
        {
            Init();
        }

        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public void Init()
        {
            biSize = SIZE;
        }

        public override string ToString()
        {
            return string.Format("BitmapInfoHeader[{0}x{1}/{2:X}/{3}]",
                biWidth, biHeight, biCompression, biSizeImage);
        }

        public bool EncodeStream(System.IO.Stream handler)
        {
            bool success = false;
            try
            {
                handler.Write(BitConverter.GetBytes(biSize), 0, 4);
                handler.Write(BitConverter.GetBytes(biWidth), 0, 4);
                handler.Write(BitConverter.GetBytes(biHeight), 0, 4);
                handler.Write(BitConverter.GetBytes(biPlanes), 0, 2);
                handler.Write(BitConverter.GetBytes(biBitCount), 0, 2);
                handler.Write(BitConverter.GetBytes(biCompression), 0, 4);
                handler.Write(BitConverter.GetBytes(biSizeImage), 0, 4);
                handler.Write(BitConverter.GetBytes(biXPelsPerMeter), 0, 4);
                handler.Write(BitConverter.GetBytes(biYPelsPerMeter), 0, 4);
                handler.Write(BitConverter.GetBytes(biClrUsed), 0, 4);
                handler.Write(BitConverter.GetBytes(biClrImportant), 0, 4);

                success = true;
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
                handler.Read(buffer, 0, buffer.Length);

                biSize = BitConverter.ToUInt32(buffer, pos);
                pos += 4;
                biWidth = BitConverter.ToInt32(buffer, pos);
                pos += 4;
                biHeight = BitConverter.ToInt32(buffer, pos);
                pos += 4;
                biPlanes = BitConverter.ToUInt16(buffer, pos);
                pos += 2;
                biBitCount = BitConverter.ToUInt16(buffer, pos);
                pos += 2;
                biCompression = BitConverter.ToUInt32(buffer, pos);
                pos += 4;
                biSizeImage = BitConverter.ToUInt32(buffer, pos);
                pos += 4;
                biXPelsPerMeter = BitConverter.ToInt32(buffer, pos);
                pos += 4;
                biYPelsPerMeter = BitConverter.ToInt32(buffer, pos);
                pos += 4;
                biClrUsed = BitConverter.ToUInt32(buffer, pos);
                pos += 4;
                biClrImportant = BitConverter.ToUInt32(buffer, pos);
                pos += 4;

                Debug.Assert(pos == SIZE);
                success = true;
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
                int pos = from;
                Array.Copy(BitConverter.GetBytes(biSize), 0, data, pos, 4);
                pos += 4;
                Array.Copy(BitConverter.GetBytes(biWidth), 0, data, pos, 4);
                pos += 4;
                Array.Copy(BitConverter.GetBytes(biHeight), 0, data, pos, 4);
                pos += 4;
                Array.Copy(BitConverter.GetBytes(biPlanes), 0, data, pos, 2);
                pos += 2;
                Array.Copy(BitConverter.GetBytes(biBitCount), 0, data, pos, 2);
                pos += 2;
                Array.Copy(BitConverter.GetBytes(biCompression), 0, data, pos, 4);
                pos += 4;
                Array.Copy(BitConverter.GetBytes(biSizeImage), 0, data, pos, 4);
                pos += 4;
                Array.Copy(BitConverter.GetBytes(biXPelsPerMeter), 0, data, pos, 4);
                pos += 4;
                Array.Copy(BitConverter.GetBytes(biYPelsPerMeter), 0, data, pos, 4);
                pos += 4;
                Array.Copy(BitConverter.GetBytes(biClrUsed), 0, data, pos, 4);
                pos += 4;
                Array.Copy(BitConverter.GetBytes(biClrImportant), 0, data, pos, 4);
                pos += 4;

                Debug.Assert(pos - from == SIZE);

                from = pos;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return from;
        }

        public int DecodeMemory(byte[] data, int from)
        {
            try
            {
                int pos = from;

                biSize = BitConverter.ToUInt32(data, pos);
                pos += 4;
                biWidth = BitConverter.ToInt32(data, pos);
                pos += 4;
                biHeight = BitConverter.ToInt32(data, pos);
                pos += 4;
                biPlanes = BitConverter.ToUInt16(data, pos);
                pos += 2;
                biBitCount = BitConverter.ToUInt16(data, pos);
                pos += 2;
                biCompression = BitConverter.ToUInt32(data, pos);
                pos += 4;
                biSizeImage = BitConverter.ToUInt32(data, pos);
                pos += 4;
                biXPelsPerMeter = BitConverter.ToInt32(data, pos);
                pos += 4;
                biYPelsPerMeter = BitConverter.ToInt32(data, pos);
                pos += 4;
                biClrUsed = BitConverter.ToUInt32(data, pos);
                pos += 4;
                biClrImportant = BitConverter.ToUInt32(data, pos);
                pos += 4;

                Debug.Assert(pos - from == SIZE);
                from = pos;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return from;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class BitmapInfo
    {
        public BitmapInfo()
        {
            Init();
        }

        internal BitmapInfoHeader bmiHeader;
        internal uint bmiColor;

        internal void Init()
        {
            bmiHeader.biSize = (uint)Marshal.SizeOf(bmiHeader);
        }
    }
}
