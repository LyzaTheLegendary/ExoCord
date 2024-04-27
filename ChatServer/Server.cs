using Core;
using Core.Pools;
using Network;
using Packets;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    public class Server {

        readonly XAddr m_address;
        readonly Socket m_listener;
        readonly List<UserClient> m_users;
        readonly IdPool m_pool;
        readonly TaskPool m_taskPool;
        bool m_isAlive = false;

        public XAddr GetAddress() => m_address;
        public Server(XAddr address, int threads = 4) {
            m_address = address;

            m_taskPool = new TaskPool(threads);
            m_users = new List<UserClient>();
            IPEndPoint ep = new(address.Address, address.Port);

            m_listener = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_listener.Bind(ep);
            m_listener.Listen(10);

            m_taskPool.PendAction(Listen);
            StartSendQueue();
        }

        private void Listen() {
            Logger.WriteInfo($"Server listening to: {GetAddress()}");
            while (m_isAlive) {
                Thread.Yield();
                Socket remoteSocket = m_listener.Accept();

                // Do login here.
                
                //

                UserClient user = new(m_pool.GetNewId(), remoteSocket, OnConnect, OnDisconnect, OnMessage);
            }
        }

        public void OnConnect(UserClient user) {
            lock (m_users) {
                m_users.Add(user);
            }
        }

        public void OnDisconnect(UserClient user) {
            lock (m_users) {
                m_users.Remove(user);
            }

            m_pool.ReturnId(user.GetId());
        }

        public void OnMessage(Header header, Msg msg) { 
            // request
            // somehow get the all the values associated to the user
            // Session data, Ip address / THROTTLES / SPECIAL VALUES
        }

        public void StartSendQueue() {
            m_taskPool.PendAction(() => {
                foreach (Message message in UserClient.GetConsumingPipeIterator()) {
                    try {
                        message.socket.Send(message.packet.GetBytes());
                    } catch (Exception ex) {
                        Console.WriteLine(ex.ToString());
                        message.socket.Close();
                    }
                }
            });
        }
    }
}
