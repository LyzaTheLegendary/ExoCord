using System.Runtime.InteropServices;

namespace DataCore.Structures {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DataHeader {
        [MarshalAs(UnmanagedType.U2)]
        public ushort fragments;
        [MarshalAs(UnmanagedType.I4)]
        public int flags;
    }
}
