using Casting;
using Network.Channels;
using Packets;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;

namespace Network {
    public readonly struct Message { // add some kind of priority? this way I could sort things 
        public readonly Socket socket;
        public readonly Header header; // should be header instead
        public readonly Msg packet;

        public Message(Socket client, Header header, Msg packet) {
            this.socket = client;
            this.header = header;
            this.packet = packet;
        }
    }
    public class UserClient {
        private static BlockingCollection<Message> MessagePipe = new();
        private Action<UserClient> m_onConnect; // this should assign the ID 
        private Action<UserClient> m_onDisconnect; // this should remove the ID
        private Action<Header, Msg> m_onMessage;


        private readonly uint m_id;
        private readonly XAddr m_address;
        private readonly Socket m_socket;
        private readonly ResultManager resultManager = new();

        private bool IsServer { get => m_id != 0; }

        public UserClient(uint id, Socket remoteSocket, Action<UserClient> onConnect, Action<UserClient> onDisconnect, Action<Header, Msg> onMessage) {
            m_address = new XAddr(remoteSocket.RemoteEndPoint!.ToString()!);
            m_socket = remoteSocket;
            m_id = id;

            m_onConnect = onConnect;
            m_onDisconnect = onDisconnect;
            m_onMessage = onMessage;

            Task.Factory.StartNew(Listen, TaskCreationOptions.LongRunning);
        }

        // Can throw exception is failed to connect!
        public UserClient(XAddr host, Action<UserClient> onConnect, Action<UserClient> onDisconnect, Action<Header, Msg> onMessage) {
            m_socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            m_socket.Connect(host.Address, host.Port);
            m_address = host;
            m_id = 0;

            m_onConnect = onConnect;
            m_onDisconnect = onDisconnect;
            m_onMessage = onMessage;

            Task.Factory.StartNew(Listen, TaskCreationOptions.LongRunning);
        }

        public XAddr GetAddress() => m_address;
        public uint GetId() => m_id;

        // Normal message
        public void PendMessage(ushort id, Msg message) {
            Header header = new Header((ushort)message.GetSize(), id);
            MessagePipe.Add(new Message(this.m_socket, header, message));
        }
        
        // With a handler for the respones
        public void PendMessage(ushort id, Msg message, Action<ResultCode> result) {
            Header header = new Header((ushort)message.GetSize(), id, (ushort)HeaderFlags.RESULT,resultManager.GetNewId(), (DateTime.Now.Millisecond / 60));
            MessagePipe.Add(new Message(this.m_socket, header, message));
        }

        // Request a channel, Important to keep track of results so it can be removed if failed
        public void CreateChannel(uint channelId, byte timeoutInSeconds, Action<ResultCode, NetworkChannel> result) { 
            // As user it requests
            // As server it creates
        }

        public void DestroyChannel(uint channelId, Action<ResultCode> result) {
            // As user it requests
            // As server it Destroys
        }

        public static IEnumerable<Message> GetConsumingPipeIterator()
            => MessagePipe.GetConsumingEnumerable();

        private void Listen() {
            while(m_socket.Connected) {
                try {
                    Header header = m_socket.ReceiveStruct<Header>();
                    Msg msg = m_socket.ReceiveMsg(header.GetId(),header.GetSize());

                    HeaderFlags flags = (HeaderFlags)header.m_flags;

                    if (flags.HasFlag(HeaderFlags.RESULT)) { // Request has to be responded with header with same packet ID and a result type appended to it
                        resultManager.HandleResult(header.m_packetId, msg);
                        continue;
                    } else if (flags.HasFlag(HeaderFlags.CREATE_CHANNEL)) {
                        // Give back a result with a channel
                    } else if (flags.HasFlag(HeaderFlags.SEND_CHANNEL)) {
                        // Send to a channel
                    } else if (flags.HasFlag(HeaderFlags.CLOSE_CHANNEL)) {
                        // Close channel
                    }



                    m_onMessage.Invoke(header, msg);

                } catch(Exception) {
                    break;
                }
            }
            m_onDisconnect.Invoke(this);
        }

    }
}
