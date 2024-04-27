using Networking.Network;
using Packets;
using System.Collections.Concurrent;
namespace Network.Channels
{
    
    public class ChannelManager
    {
        /* GOAL:
         * 1. Create channels via packets that request it
         * 2. Keep track of channels and send requested data
         * 3. Be able to close channels
         */
        private readonly IdPool m_pool = new();
        private readonly ConcurrentDictionary<uint, NetworkChannel> m_channels = new();
        // Channel creation a channel should be returned via lambda just like the result packet
        public ResultCode HandleChannelRequest(uint channelId, ushort packetId, Msg data) {
            if (!m_channels.TryGetValue(channelId, out NetworkChannel? channel)) {
                return ResultCode.NOT_FOUND;
            }

            channel.NetworkReceive?.Invoke(packetId, data);
            return ResultCode.SUCCESS;
        }

        public NetworkChannel CreateChannel(int timeout = 10) {
            NetworkChannel channel = new(m_pool.GetNewId(), timeout);

            m_channels[channel.Id] = channel;

            return channel;
        }

        public void DestroyChannel(uint channelId) {
            if (!m_channels.Remove(channelId, out NetworkChannel? channel))
                return;

            m_pool.ReturnId(channel.Id);
        }
    }
}
