/**
 * Hello TCP/IP ���Ĥ@�Ӫ���
 * 
 * @see ���ں������|�j�A��, p30-32
 */ 
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
namespace hello_tcpip {
    class Program {
        static void Main(string[] args) {
            Socket serverSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Socket �P���� IP �M Port �i�����p
            IPEndPoint sockaddr = new IPEndPoint(GetServerIP(), 2058);               
            serverSck.Bind(sockaddr);

            // ��ť Client �s�u
            serverSck.Listen(5);            
            Console.WriteLine("Ready to serve my first client. Please connect ...");
            // ���ݱ����s�u
            Socket clientSck = serverSck.Accept();

            // �ǰe�T���� Client
            clientSck.Send( Encoding.ASCII.GetBytes("Hello, TCP/IP!\r\n"));
            Console.WriteLine("Another client served!");

            // ���� Client �M Server ���s�u
            clientSck.Close();
            serverSck.Close(); 
        }

        public static IPAddress GetServerIP() {

            IPHostEntry ieh = Dns.GetHostByName(Dns.GetHostName());
            return ieh.AddressList[0];
        } 
    }
}
