using System.Collections.Concurrent;

namespace Networking.Network
{
    public class IdPool
    {
        uint m_highest = 1;
        ConcurrentQueue<uint> m_cached = new ConcurrentQueue<uint>();

        public uint GetNewId()
        {
            uint id;
            if (m_cached.TryDequeue(out id))
                return id;

            id = m_highest++;

            return id;
            
        }

        public void ReturnId(uint id)
            => m_cached.Enqueue(id);


    }
}
