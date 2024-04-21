using System.Net;

namespace Network {
    public struct XAddr {
        string m_host;
        
        public IPAddress Address {  get => IPAddress.Parse(m_host.Split()[0]); }
        public int Port { get => int.Parse(m_host.Split()[1]);  }
        public XAddr(string host) {

            if (!host.Contains(":"))
                throw new ArgumentException("Invalid host, no port found.");

            m_host = host;
        }

        public override string ToString()
            => m_host;
        

    }
}
