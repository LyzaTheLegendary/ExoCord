using Core.Extensions;
using Packets;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Network
{
    public static class SocketExtensions {
        public static T ReceiveStruct<T>(this Socket socket) where T : struct {
            int structSize = Marshal.SizeOf<T>();

            byte[] bytes = new byte[structSize];

            socket.Receive(bytes);

            return bytes.BitCast<T>();
        }
        public static Msg ReceiveMsg(this Socket socket, ushort id, ushort length) {
            byte[] bytes = new byte[length];

            socket.Receive(bytes);

            return new Msg(id, bytes);
        }
    }
}
