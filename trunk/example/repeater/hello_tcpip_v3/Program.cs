/**
 * Hello TCP/IP 的第三個版本
 * 
 * @see 網際網路的四大服務, p63-73
 */ 
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace hello_tcpip_3 {
    class Program {
        private const int PORT = 2058;
        static void ThreadProc(Object stateInfo) {
            CClientStub pStub = stateInfo as CClientStub;
            
            if (pStub != null){
                CBasicClient client = new CBasicClient(pStub); 
                //Socket s = pStub.m_socket;
                //CWin32TCPEnv env = pStub.m_env;
                //CServiceMutex central = pStub.m_central;

                //CBasicClient client = new CBasicClient (s, env);
                client.SendRaw(Encoding.ASCII.GetBytes("Hello, TCP/IP!\r\nPress a key to disconnect.\r\n"), SocketFlags.None);   
                byte[] key = new byte[1] ;
                if (1 != client.RecvRaw(key, SocketFlags.None)) {
                    client.ShowStatus("SERVICE: Client disconnected without pressing a key."); 
                } else {
                    client.ShowStatus( String.Format( "SERVICE: Client press [{0}] key", key[0]));
                }

                client.Close(); 
                client.ShowStatus("SERVICE: Another client served!");     
//                pStub.m_central.ReleaseService();   
            }            
        }

        static void Main(string[] args) {
            CWin32TCPEnv env = new CWin32TCPEnv();
            
            try {
                env.ReadyPort(PORT);
                for(int id=1; id<=2; ++id){                                    
                    env.ShowStatus(String.Format("SERVER: ready to serve client #{0}. Please connect...", id));
                    env.SpawnService(env.BlockForClient(PORT), new WaitCallback(ThreadProc));                       
                }
                env.ShowStatus("SERVER: waiting for services threas to end...");                
            } catch (CWin32TCPEnv.ETooManyServices) {
                env.ShowError("ERR: too many services are running"); 
            }
        }
    }
}
