using System.Runtime.InteropServices;

namespace Packets {

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChannelPacket {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        private readonly byte[] channelPath;
        [MarshalAs(UnmanagedType.Bool)]
        private readonly int channelAction;
        private readonly byte[] channelData;
    }
}
