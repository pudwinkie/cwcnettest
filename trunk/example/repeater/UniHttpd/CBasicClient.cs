using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace hello_tcpip_3 {
    /// <summary>
    /// 
    /// </summary>
    public class CBasicClient {
        public CBasicClient(CClientStub pStub) {
            m_env = pStub.m_env ;
            m_socket = pStub.m_socket ;            
        }

        public void Close() {
            m_env.BreakConnection(m_socket);
        }

        public int SendRaw(byte[] buf, SocketFlags sf) {
            return m_env.SendRaw(m_socket, buf, sf);
        }

        public int SendUnitlDone(byte[] buf, SocketFlags sf) {
            return 0;
        }

        public int RecvRaw(byte[] buf, SocketFlags sf) {
            return m_env.RecvRaw(m_socket, buf, sf);
        }        

        public int RecvUnitlDone(byte[] buf, SocketFlags sf) {
            return 0;
        }

        public void ShowStatus(String str) {
            m_env.ShowStatus(str);
        }

        public void ShowError(String str) {
            m_env.ShowError(str);
        }

        private IBasicEnv m_env;
        protected Socket m_socket;
    }
}
