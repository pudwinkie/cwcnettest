/**
 * UniHttpd 雛型
 * 
 * @see 網際網路的四大服務, p81-84
 */ 
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace hello_tcpip_3 {
    class Program {
        static int ReceiveUnitlStr(CBasicClient pClient, byte[] buffer, byte[] stop_chars) {
            int i = 0;
            byte[] b = new byte[1];
            for (i = 0; i < buffer.Length; ++i) {
                if (pClient.RecvRaw(b, SocketFlags.None) == 0) {
                    break;
                }
                buffer[i] = b[0];
                if (b[0] == stop_chars[stop_chars.Length - 1] && i>= stop_chars.Length ) {
                    bool bMatch = true;
                    int idx = i;
                    for (int pos = stop_chars.Length - 1; pos > 0 && bMatch; --pos, --idx) {
                        if (stop_chars[pos] != buffer[idx]) {
                            bMatch = false;
                        }
                    }

                    if (bMatch) {
                        break;
                    }
                }
            }

            return i+1;
        }

        static void HttpdThreadProc(Object stateInfo) {
            CClientStub pStub = stateInfo as CClientStub;
            
            if (pStub != null){
                CBasicClient client = new CBasicClient(pStub); 
                byte[] buffer = new byte[1024];
                int len = ReceiveUnitlStr(client, buffer, new byte[4] {0x0d, 0x0a, 0x0d, 0x0a});

                client.ShowStatus("SERVICE> Request header received as follows:");
                client.ShowStatus( Encoding.ASCII.GetString(buffer,0, len) );

                String msg = "HTTP/1.0 503 Service Unavailable\r\nCONTENT-LENGTH: 40\r\n\r\nThis site is temporarily out of service.";
                client.SendRaw(Encoding.ASCII.GetBytes(msg), SocketFlags.None);   
                client.Close(); 
                client.ShowStatus("SERVICE: Another client served!");     
            }            
        }

        static void Main(string[] args) {
            CWin32TCPEnv env = new CWin32TCPEnv();
            const int WEB_PORT = 80;
            try {
                env.ReadyPort(WEB_PORT); 
                for(int id=1; true; ++id){                                    
                    env.ShowStatus(String.Format("SERVER> Ready to serve browser #{0}...", id));
                    env.SpawnService(env.BlockForClient(WEB_PORT), new WaitCallback(HttpdThreadProc));                       
                }
                env.ShowStatus("SERVER> waiting for services threas to end...");                
            } catch (CWin32TCPEnv.ETooManyServices) {
                env.ShowError("ERR: too many services are running"); 
            }
        }
    }
}
