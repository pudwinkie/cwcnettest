using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Messenger;
using System.Web;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
namespace MyBot {
    public class Sunnet :IMessengerAddIn{
        private MessengerClient m_messenger;
        private const string Menu = @"1. Give GUID\n
2. Give Random";
        public void Initialize(MessengerClient messenger) {
            m_messenger = messenger;

            messenger.Shutdown += new EventHandler(messenger_Shutdown);
            messenger.IncomingTextMessage += new EventHandler<IncomingTextMessageEventArgs>(messenger_IncomingTextMessage);            
            var prop = messenger.AddInProperties;
            prop.Creator = "Chui-Wen Chiu";
            prop.FriendlyName = "SUNNET BOT";            
            
        }

        Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
        string qswhEval3(string Expression) {
            return Microsoft.JScript.Eval.JScriptEvaluate(Expression, ve).ToString();
        }

        public static string MD5(string originalPassword) {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }

        void messenger_IncomingTextMessage(object sender, IncomingTextMessageEventArgs e) {
            var usr = e.UserFrom;
            try {

                // command GET_FORMAT_PARAMS
                //Debug.WriteLine(e.TextMessage);
                string data = String.Empty;
                string msg = e.TextMessage;                

                if (msg == "guid") {
                    data = Guid.NewGuid().ToString();
                }else if (msg == "author") {
                    data = "Author: Chui-Wen Chiu\nBlog:http://chuiwenchiu.spaces.live.com\nCompany: SUNNET(http://www.sun.net.tw)";                    
                }else if (msg == "info"){
                    data = "Your Infomation:\n" +  String.Format("Id: {3}\nPersonStatusMessage: {2}\nFriendName: {1} \nEmail: {0}\nStatus: {4}",
                    usr.Email, usr.FriendlyName, usr.PersonalStatusMessage, usr.UniqueId, usr.Status.ToString());                 
                }else if (msg == "ip"){
                    string url = "http://www.labpixies.com/campaigns/ip/getip.php";
                    var wc = new WebClient();
                    Regex reg = new Regex("({\"ip\":\"(.*)\"})");
                    
                    var ipdata = wc.DownloadString(url);
                    Match match = reg.Match(ipdata); 
                    data = match.Groups[1].Value;
                } else{
                    string[] cmd = msg.Split(new char[]{' '}, 2,  StringSplitOptions.RemoveEmptyEntries );
                    if (cmd.Length == 2){
                        var arg = cmd[1];
                        if (cmd[0] == "md5") {
                            data = MD5(arg);
                        }else if (cmd[0] == "urldecode"){
                            data = HttpUtility.UrlDecode(arg);
                        }else if (cmd[0] == "urlencode"){
                            data = HttpUtility.UrlEncode(arg);
                        } else if (cmd[0] == "eval") {
                            try {
                                data = qswhEval3(arg);
                            } catch (Exception ex){
                                data = ex.Message;
                            }
                        }
                    }
                } 

                if (String.IsNullOrEmpty(data)) {
                    var sb = new StringBuilder();
                    sb.AppendLine("Menu: ");
                    sb.AppendLine("guid");
                    sb.AppendLine("md5 [data]");
                    sb.AppendLine("info");
                    sb.AppendLine("author");
                    sb.AppendLine("Your Message: " + e.TextMessage);
                    data = sb.ToString();
                    //data = "Menu:\nguid\nmd5 [data]\ninfo\nauthor\nYour Message: " + e.TextMessage;
                }

                m_messenger.SendTextMessage(data, usr);
            } catch (System.Runtime.InteropServices.COMException ex) {
                MessageBox.Show("Exception");
            }
            //Debug.Flush();

            //m_messenger.SendActionMessage("搖搖頭", usr);
            //m_messenger.SendTextMessage("Add-In test", usr);
            //m_messenger.SendNudgeMessage(usr);
        }

        void messenger_Shutdown(object sender, EventArgs e) {
            // TODO: 釋放註冊事件
            //m_messenger.Shutdown -= new EventHandler(messenger_Shutdown);
            //m_messenger.IncomingTextMessage -= new EventHandler<IncomingTextMessageEventArgs>(messenger_IncomingTextMessage);
        }
    }
}
