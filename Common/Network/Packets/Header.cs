using Casting;
using System.Runtime.InteropServices;

namespace Packets {
    
    [Flags] public enum HeaderFlags: ushort {
        NONE = 0,
        RESULT = 1,
        CREATE_CHANNEL = 2,
        CLOSE_CHANNEL = 4,
        SEND_CHANNEL = 8,
        // EXTRA data?
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Header {
        [MarshalAs(UnmanagedType.U2)]
        public ushort m_len;
        [MarshalAs(UnmanagedType.U2)]
        public ushort m_id;
        [MarshalAs(UnmanagedType.I4)]
        public int m_checksum; // used to verify cryptography integrity
        [MarshalAs(UnmanagedType.U2)]
        public ushort m_flags;
        [MarshalAs(UnmanagedType.U4)]
        public uint m_packetId;  //   Used to keep track of a packet for results OR channels
        [MarshalAs(UnmanagedType.U1)]
        public int m_timeoutInSeconds; // Used to know how long a packet can expect a result 

        public Header(ushort len,  ushort id, ushort flags = 0, uint packetId = 0, int timeoutInSeconds = 0) {
            m_len = len;
            m_id = id;
            m_flags = flags;
            m_packetId = packetId;
            m_timeoutInSeconds = timeoutInSeconds;
            m_flags = 0;
            CreateCheckSum();
        }

        public ushort GetId() => m_id;
        public ushort GetSize() => m_len;

        public void CreateCheckSum() {
            m_checksum = m_len.BitCast().Concat(m_id.BitCast()).ToArray().BitCast<int>();
        }

        public bool ValidateChecksum() 
            => m_len.BitCast().Concat(m_id.BitCast()).ToArray().BitCast<int>() == m_checksum;
        
    }
}
