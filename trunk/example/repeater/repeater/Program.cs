using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Collections;

namespace repeater {
    class Program {
        static void Main(string[] args) {
            VNCRepeater vnc = new VNCRepeater();
            vnc.Run(); 
        }
    }

    class VNCRepeater {
        private static IPAddress GetServerIP() {
            IPHostEntry ieh = Dns.GetHostByName(Dns.GetHostName());
            return ieh.AddressList[0];
        }

        private static void ParseDisplay(String data, ref String host, ref int port) {
            int pos = data.IndexOf('\0');

            String[] tmp = data.Substring(0, pos).Split(new char[] { ':' });
            host = tmp[0];
            if (tmp.Length == 1) {
                port = 2900;
            } else {
                port = int.Parse(tmp[1]);
            }
        }

        private static Socket open_connection(String host, int port) {
            Socket remoteSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            remoteSck.Connect(host, port);
            return remoteSck;
        }

        public void Run() {
            int local_port = 5900;
            int server_port = 5500;
            Socket serverSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint sockaddr = new IPEndPoint(GetServerIP(), local_port);
            serverSck.Bind(sockaddr);
            serverSck.Listen(1);

            Socket viewerSck = serverSck.Accept();
            viewerSck.Send(Encoding.ASCII.GetBytes("RFB 000.000\n"));
            byte[] buf = new byte[250];
            int len = viewerSck.Receive(buf);
            int port = 0;
            String host = String.Empty;
            ParseDisplay(Encoding.ASCII.GetString(buf), ref host, ref port);
            //serverSck.Shutdown(SocketShutdown.Both);  
            Console.WriteLine("{0}:{1}", host, port);

            Socket remoteSck = open_connection(host, port);
            Console.WriteLine("do_repeater");
            bool b_local = true ;
            bool b_remote = true;
            byte[] rbuf = new byte[1024];
            byte[] lbuf = new byte[1024];
            int rbuf_len = 0;
            int lbuf_len = 0;

            // 準備讀取 remote/local 輸入資料 
            const int waitMS = 10;           
 
            while (b_local || b_remote) {
                ArrayList readList = new ArrayList();
                readList.Add(viewerSck);
                readList.Add(remoteSck);   
                Socket.Select(readList, new ArrayList(), new ArrayList(), waitMS);                
                // Read From Remote
                if (remoteSck.Poll(waitMS, SelectMode.SelectRead) && rbuf_len < rbuf.Length) {
                    len = remoteSck.Receive(rbuf, rbuf_len, rbuf.Length - rbuf_len, SocketFlags.None);
                    if (len == 0) {
                        b_remote = false;			
                        b_local = false;
                        viewerSck.Close();
                        remoteSck.Close();
                        Console.WriteLine("ERROR");
                        break;
                    } else if (len == -1) {
                        Console.WriteLine("ERROR"); 
                    } else {
                        rbuf_len += len;
                        Console.WriteLine("Read From Remote: {0} byte", len); 
                    }
                } 

                // Read from Viewer
                if (viewerSck.Poll(waitMS, SelectMode.SelectRead) && lbuf_len < lbuf.Length) {
                    len = viewerSck.Receive(lbuf, lbuf_len, lbuf.Length - lbuf_len, SocketFlags.None);
                    if (len == 0) {
                        b_remote = false;
                        b_local = false;
                        viewerSck.Close();
                        remoteSck.Close();  
                        Console.WriteLine("ERROR");
                        break;
                    } else if (len == -1) {
                        Console.WriteLine("ERROR");
                    } else {
                        lbuf_len += len;
                        Console.WriteLine("Read From Viewer: {0} byte", len);
                    }
                } 

                // Flush data to remote
                if (lbuf_len > 0) {
                    len = remoteSck.Send(lbuf, lbuf_len, SocketFlags.None);
                    if (len == -1) {
                        Console.WriteLine("Send Fail");
                    } else if (len>0){
                        lbuf_len -= len;
                        Console.WriteLine("Write To remote: {0} byte", len);
                        if (lbuf_len > 0) {
                            for (int i = len; i < lbuf_len+len; ++i) {
                                lbuf[i - len] = lbuf[i];
                            }
                        }
                    }
                }
                // flush data to viewer
                if (rbuf_len > 0) {
                    len = viewerSck.Send(rbuf, rbuf_len, SocketFlags.None);
                    if (len == -1) {
                        Console.WriteLine("Send Fail");
                    } else if (len > 0) {
                        rbuf_len -= len;
                        Console.WriteLine("Write To viewer: {0} byte", len);
                        if (rbuf_len > 0) {
                            for (int i = len; i < rbuf_len + len; ++i) {
                                lbuf[i - len] = lbuf[i];
                            }
                        }
                    }
                }
            }
            viewerSck.Close();  
        }
    }
}
