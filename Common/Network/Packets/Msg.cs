using Casting;

namespace Packets {
    public struct Msg {
        private readonly ushort id;
        private readonly byte[] data;

        public Msg(ushort id, byte[] data) {
            this.id = id;
            this.data = data;
        }

        public ushort GetId() => id;
        public int GetSize() => data.Length;

        public byte[] GetBytes() => data;

        public T GetStruct<T>() where T : struct 
           => data.BitCast<T>();
        
    }
}
