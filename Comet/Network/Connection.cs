using Comet.GraphicalUserInterface.Windowing;
using Core;
using Core.Extensions;
using Packets;
using System.Net.Sockets;

namespace Network {
    static public class Connection {
        static private UserClient client;
        static private XAddr address = default(XAddr);
        static public ResultCode Connect(string username, string password) {
            XAddr address = new XAddr(Settings.GetValue<string>("server.host", "127.0.0.1:6629"));
            Connection.address = address;
            try {
                Socket sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(address.Address, address.Port);

                using(NetworkStream stream = new(sock, false)) {
                    stream.WriteString(username);
                    stream.WriteString(password);
                    stream.Flush();

                    ResultCode result = (ResultCode)stream.ReadStruct<int>();

                    if(result != ResultCode.SUCCESS)
                        return result;
                }

                client = new UserClient(sock, OnConnect, OnDisconnect, OnMessage);

            } catch(Exception e) {

                Logger.WriteErr($"Failed to connect err: {e.Message}");
                return ResultCode.FAILED;
            }

            return ResultCode.SUCCESS;
        }

        static public  XAddr GetAddress() => address;

        static private void OnConnect(UserClient cient) {
            DebugWindow.connected = true;
        }

        static private void OnDisconnect(UserClient cient) {
            DebugWindow.connected = false;
        }

        static private void OnMessage(Header header, Msg msg) {

        }
    }
}
