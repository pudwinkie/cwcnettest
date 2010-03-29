using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace hello_tcpip_3 {

    /// <summary>
    /// 
    /// </summary>
    public class CWin32TCPEnv : IBasicEnv {
        #region member data
        private int m_nPortServiceCount = 0;
        private CPortService[] m_PortServices;
        public AutoResetEvent evServicesDone;
        public Mutex mtServices;
        public int nServiceCount = 0;
        private const int m_nResourceTimeout = 10000;
        #endregion

        public CWin32TCPEnv() {
            m_PortServices = new CPortService[1];
            mtServices = new Mutex(false);
            evServicesDone = new AutoResetEvent(false);
        }

        public void Close() {
            WaitUntilAllDone();
            for (int i = 0; i < m_nPortServiceCount; ++i) {
                if (m_PortServices[i].m_socket != null) {
                    m_PortServices[i].m_socket.Close(); 
                }
            }
        }

        public void SpawnService(Socket sckClient, WaitCallback tstart) {
            CClientStub pStub = new CClientStub(sckClient, this);
            RegisterService();
            ThreadPool.QueueUserWorkItem(tstart, pStub);                            
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

        public CPortService PortInfo(int port) {
            if (m_nPortServiceCount == 0 || m_nPortServiceCount == 1 && m_PortServices[0].m_nPort != port) {
                return null;
            } else {
                return m_PortServices[0];
            }
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

        #region member function
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
            CPortService pInfo = PortInfo(port);
            if (pInfo == null) {
                return null;
            } else {
                return pInfo.m_socket; 
            }
        }

        #endregion

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

}
