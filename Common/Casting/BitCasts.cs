using System.Runtime.InteropServices;

namespace Casting {
    public static class Bitcasts {

        public static T BitCast<T>(this byte[] bytes) where T : struct {
            int size = Marshal.SizeOf<T>();
            if (bytes.Length < size)
                throw new Exception("Invalid parameter");

            IntPtr ptr = Marshal.AllocHGlobal(size);
            try {
                Marshal.Copy(bytes, 0, ptr, size);
                return (T)Marshal.PtrToStructure(ptr, typeof(T))!;
            } finally {
                Marshal.FreeHGlobal(ptr);
            }
        }
        public static byte[] BitCast<T>(this T structure) where T : struct {
            int structSize = Marshal.SizeOf<T>();
            byte[] bytes = new byte[structSize];

            IntPtr ptr = Marshal.AllocHGlobal(structSize);
            try {
                Marshal.StructureToPtr(structure!, ptr, false);
                Marshal.Copy(ptr, bytes, 0, structSize);
            } finally {
                Marshal.FreeHGlobal(ptr);
            }

            return bytes;
        }

    }
}
