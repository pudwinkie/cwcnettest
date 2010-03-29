using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace hello_tcpip_3 {
    /// <summary>
    /// 
    /// </summary>
    public class CClientStub {
        public CClientStub(Socket s, IBasicEnv env) {
            m_socket = s;
            m_env = env;            
        }

        public Socket m_socket;
        public IBasicEnv m_env;

    }
}
