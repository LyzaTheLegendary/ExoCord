using System.Runtime.InteropServices;

namespace Packets {

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChannelPacket {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        private readonly byte[] channelPath;
        [MarshalAs(UnmanagedType.Bool)]
        private readonly int channelAction;
        [MarshalAs(UnmanagedType.U2)]
        private readonly ushort channelDataSize;
        //private readonly byte[] channelData; // present data as a size!
    }
}
