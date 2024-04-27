using System.Runtime.InteropServices;

namespace Packets {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ResultPacket
    {
        [MarshalAs(UnmanagedType.U2)]
        public readonly ushort result;

        public ResultPacket(ushort code) {
            result = code;
        }
    }
}
