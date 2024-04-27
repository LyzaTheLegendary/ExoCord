using Packets;

namespace Network.Channels {
    /*
     * GOAL: 
     * 1. Share data over this location to all people who are connected to this channel id, this can be a single person or more.
     * 2. Be able to keep it in sync with the server
     * 3. Be able to close the channel and notify the server of it closing
     * 4. Network channels should be requested FROM the server a CLIENT should never be able to create one!
     */
    public class NetworkChannel {
        // list of ids that are linked to it? id as in user id
        private readonly uint m_channelId;
        private readonly int m_timeoutInSeconds;
        private bool m_alive;

        public uint Id { get => m_channelId; }
        public int TimeOutInSeconds { get => m_timeoutInSeconds; }
        public bool Alive { get => m_alive; set => m_alive = value; }

        public delegate void NetworkReceiveDelegate(ushort id, Msg packet);
        public NetworkReceiveDelegate? NetworkReceive;

        public delegate void ChannelDeletionDelegate();

        public event NetworkReceiveDelegate OnNetworkReceive {
            add => NetworkReceive += value;
            remove => NetworkReceive -= value;
        }

        public NetworkChannel(uint channelId, int timeoutInSeconds) {
            m_channelId = channelId;
            m_timeoutInSeconds = timeoutInSeconds;
            m_alive = true;
        }

        public void Send(byte[] data) {
            throw new NotImplementedException();
        }
        public void Close() {
            throw new NotImplementedException(); // Send close channel sequence in protocol that's not there yet!
        }
    }
}
