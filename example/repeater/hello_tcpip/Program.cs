/**
 * Hello TCP/IP 的第一個版本
 * 
 * @see 網際網路的四大服務, p30-32
 */ 
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
namespace hello_tcpip {
    class Program {
        static void Main(string[] args) {
            Socket serverSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Socket 與本機 IP 和 Port 進行關聯
            IPEndPoint sockaddr = new IPEndPoint(GetServerIP(), 2058);               
            serverSck.Bind(sockaddr);

            // 監聽 Client 連線
            serverSck.Listen(5);            
            Console.WriteLine("Ready to serve my first client. Please connect ...");
            // 等待接收連線
            Socket clientSck = serverSck.Accept();

            // 傳送訊息給 Client
            clientSck.Send( Encoding.ASCII.GetBytes("Hello, TCP/IP!\r\n"));
            Console.WriteLine("Another client served!");

            // 切換 Client 和 Server 的連線
            clientSck.Close();
            serverSck.Close(); 
        }

        public static IPAddress GetServerIP() {

            IPHostEntry ieh = Dns.GetHostByName(Dns.GetHostName());
            return ieh.AddressList[0];
        } 
    }
}
