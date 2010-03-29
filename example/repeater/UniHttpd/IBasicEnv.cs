using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace hello_tcpip_3 {
    public interface IBasicEnv {
        void ShowStatus(String str);
        void ShowError(String str);

        int SendRaw(Socket s, byte[] buf, SocketFlags sf);
        int RecvRaw(Socket s, byte[] buf, SocketFlags sf);
        void BreakConnection(Socket s);

        void ReleaseService();
    }
}
