using System.Runtime.InteropServices;
using System.Text;

namespace Core.Extensions {
    public static class StreamExtensions {

        static public T ReadStruct<T>(this Stream stream) where T : struct {
            byte[] buff = new byte[Marshal.SizeOf<T>()];
            stream.Read(buff);

            return buff.BitCast<T>();
        }

        static public void WriteStruct<T> (this Stream stream, T structure) where T : struct 
            => stream.Write(structure.BitCast());
        
        static public string ReadString(this Stream stream) {
            int strLen = stream.ReadStruct<int>();

            byte[] strBytes = new byte[strLen];
            stream.ReadExactly(strBytes, 0, strLen);

            return Encoding.UTF8.GetString(strBytes);
        }
    }
}
