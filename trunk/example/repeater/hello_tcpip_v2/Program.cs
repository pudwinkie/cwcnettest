/**
 * Hello TCP/IP 的第二個版本
 * 
 * @see 網際網路的四大服務, p44-53
 */ 
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace hello_tcpip_v2 {
    class Program {
        static void ThreadProc(Object stateInfo) {
            CClientStub pStub = stateInfo as CClientStub;   
            if (pStub != null){
                Socket s = pStub.m_socket;
                CWin32TCPEnv env = pStub.m_env;
                CServiceMutex central = pStub.m_central;

                CBasicClient client = new CBasicClient (s, env);
                client.SendRaw(Encoding.ASCII.GetBytes("Hello, TCP/IP!\r\nPress a key to disconnect.\r\n"), SocketFlags.None);   
                byte[] key = new byte[1] ;
                if (1 != client.RecvRaw(key, SocketFlags.None)) {
                    client.ShowStatus("SERVICE: Client disconnected without pressing a key."); 
                } else {
                    client.ShowStatus( String.Format( "SERVICE: Client press [{0}] key", key[0]));
                }
                client.Close(); 
                client.ShowStatus("SERVICE: Another client served!");     
                pStub.m_central.ReleaseService();   
            }            
        }

        static void Main(string[] args) {
            CWin32TCPEnv env = new CWin32TCPEnv();
            CServiceMutex ServiceCentral = new CServiceMutex();
            try {
                env.ReadyPort(2058);
                long id = 0;
                do {
                    ++id;
                    env.ShowStatus(String.Format("SERVER: ready to serve client #{0}. Please connect...", id));
                    CClientStub pStub = new CClientStub(env.BlockForClient(2058), env, ServiceCentral);
                    ServiceCentral.RegisterService();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), pStub);                            
                } while (id < 2);
                env.ShowStatus("SERVER: waiting for services threas to end...");
                ServiceCentral.WaitUntilAllDone();  
            } catch (CWin32TCPEnv.ETooManyServices) {
                env.ShowError("ERR: too many services are running"); 
            }
        }
    }

    interface IBasicEnv {
        void ShowStatus(String str);
        void ShowError(String str);

        int SendRaw(Socket s, byte[] buf, SocketFlags sf);
        int RecvRaw(Socket s, byte[] buf, SocketFlags sf);
        void BreakConnection(Socket s);
    }

    /// <summary>
    /// 
    /// </summary>
    public class CWin32TCPEnv : IBasicEnv {
        private int m_nPortServiceCount;
        private CPortService[] m_PortServices;

        public CWin32TCPEnv() {
            m_PortServices = new CPortService[1];
        }

        #region implement
        public void ShowStatus(String str) {
            Console.WriteLine(str); 
        }

        public void ShowError(String str) {
            Console.Error.WriteLine(str);
        }

        public int SendRaw(Socket s, byte[] buf, SocketFlags sf) {
            return s.Send(buf, sf);            
        }

        public int RecvRaw(Socket s, byte[] buf, SocketFlags sf) {            
            return s.Receive(buf, sf);            
        }

        public void BreakConnection(Socket s) {
            s.Close(); 
        }
        #endregion

        #region Exception
        /// <summary>
        /// 
        /// </summary>
        public class ETooManyServices : Exception {
        }

        /// <summary>
        /// 
        /// </summary>
        public class EPortNotOpen : Exception {
        }
        #endregion

        /// <summary>
        /// 服務與 Port 關聯
        /// </summary>
        /// <param name="port"></param>
        public void ReadyPort(int port) {
            if (m_nPortServiceCount >= 1) {
                throw new ETooManyServices(); 
            }

            ++m_nPortServiceCount;
            m_PortServices[m_nPortServiceCount - 1] = new CPortService(); 
            m_PortServices[m_nPortServiceCount - 1].m_nPort = port;

            Socket serverSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Socket 與本機 IP 和 Port 進行關聯
            IPEndPoint sockaddr = new IPEndPoint(GetServerIP(), port);
            serverSck.Bind(sockaddr);
            // 監聽 Client 連線
            serverSck.Listen(5); 
            m_PortServices[m_nPortServiceCount - 1].m_socket = serverSck;
        }

        /// <summary>
        /// 接受連線的使用者
        /// </summary>
        /// <param name="port"></param>
        public Socket BlockForClient(int port) {
            Socket serverSck = Port2Socket(port);
            if (serverSck == null) {
                throw new EPortNotOpen();
            }

            return serverSck.Accept();  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        protected Socket Port2Socket(int port) {
            if (m_nPortServiceCount == 0 || m_nPortServiceCount == 1 && m_PortServices[0].m_nPort != port) {
                return null;
            } else {
                return m_PortServices[0].m_socket; 
            }
        }

        #region static member function
        private static IPAddress GetServerIP() {
            IPHostEntry ieh = Dns.GetHostByName(Dns.GetHostName());
            return ieh.AddressList[0];
        }
        #endregion

        #region nest class
        public class CPortService {
            public int m_nPort;
            public Socket m_socket;
        }
        #endregion

        
    }

    /// <summary>
    /// 
    /// </summary>
    public class CClientStub {
        public CClientStub(Socket s, CWin32TCPEnv env, CServiceMutex central) {
            m_socket = s;
            m_env = env;
            m_central = central;
        }

        public Socket m_socket;
        public CWin32TCPEnv m_env;
        public CServiceMutex m_central;
    }

    /// <summary>
    /// 
    /// </summary>
    public class CServiceMutex {
        /// <summary>
        /// 
        /// </summary>
        public CServiceMutex() {
            mtServices = new Mutex(false);
            evServicesDone = new AutoResetEvent(false);   
        }

        /// <summary>
        /// 
        /// </summary>
        public void RegisterService() {
            mtServices.WaitOne(m_nResourceTimeout, false);// ??
            if (nServiceCount++ == 0) {
                evServicesDone.Reset();
            }
            mtServices.ReleaseMutex();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReleaseService() {
            mtServices.WaitOne(m_nResourceTimeout, false);// ??
            if (--nServiceCount == 0) {
                evServicesDone.Set();  
            }
            mtServices.ReleaseMutex();  
        }

        /// <summary>
        /// 
        /// </summary>
        public void WaitUntilAllDone() {
            evServicesDone.WaitOne();  
        }

        public AutoResetEvent evServicesDone;
        public Mutex mtServices;
        public int nServiceCount = 0;
        private const int m_nResourceTimeout = 10000;
    }

    /// <summary>
    /// 
    /// </summary>
    public class CBasicClient {
        public CBasicClient(Socket sc, CWin32TCPEnv env) {
            m_env = env;
            m_socket = sc;
        }

        public void Close() {
            m_env.BreakConnection(m_socket);   
        }

        public int SendRaw(byte[] buf, SocketFlags sf) {
            return m_env.SendRaw(m_socket,  buf, sf);  
        }

        public int RecvRaw(byte[] buf, SocketFlags sf) {
            return m_env.RecvRaw(m_socket, buf, sf);  
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
