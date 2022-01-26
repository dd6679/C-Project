using System.IO;

namespace VMS.Codec.Lib
{
    public interface CommonInOut
    {
        bool EncodeStream(Stream handler);
        bool DecodeStream(Stream handler);

        int EncodeMemory(byte[] data, int from);
        int DecodeMemory(byte[] data, int from);
    }
}
