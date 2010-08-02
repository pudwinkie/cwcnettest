namespace DotMSN
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    public class FileTransferHandler : IInvitationHandler
    {
        // Events
        public event FileTransferInvitationHandler InvitationReceived;

        // Methods
        static FileTransferHandler()
        {
            DotMSN.FileTransferHandler.CommandRe = new Regex(@"Invitation-Command:\s+(?<Command>[\w]+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            DotMSN.FileTransferHandler.CancelCode = new Regex(@"Cancel-Code:\s+(?<Code>[\w]+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            DotMSN.FileTransferHandler.CookieRe = new Regex(@"Invitation-Cookie:\s+(?<Cookie>[\d]+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            DotMSN.FileTransferHandler.FileRe = new Regex(@"Application-File:\s+(?<File>[^\r\n]+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            DotMSN.FileTransferHandler.FileSizeRe = new Regex(@"Application-FileSize:\s+(?<FileSize>[\d]+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            DotMSN.FileTransferHandler.IPRe = new Regex(@"IP-Address:\s+(?<IP>.+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            DotMSN.FileTransferHandler.PortRe = new Regex(@"Port:\s+(?<Port>[\d]+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            DotMSN.FileTransferHandler.AuthCookieRe = new Regex(@"AuthCookie:\s+(?<Cookie>[\d]+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            DotMSN.FileTransferHandler.ConnectRe = new Regex(@"Connectivity:\s+(?<Connectivity>\w+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            DotMSN.FileTransferHandler.randomGenerator = new Random();
            DotMSN.FileTransferHandler.portsUse = new int[40];
            DotMSN.FileTransferHandler.filetransfers = new ArrayList();
            DotMSN.FileTransferHandler.serverSocket = null;
            DotMSN.FileTransferHandler.serverSocketInternal = null;
        }

        internal FileTransferHandler(Conversation conversation)
        {
            this.socketBuffer = new byte[1024];
            this.TransferInfoRe = new Regex(@"USR (?<Mail>.*?) (?<AuthCookie>\d+)", RegexOptions.Compiled);
            this.Conversation = conversation;
        }

        internal void AddFileTransfer(FileTransfer transfer)
        {
            DotMSN.FileTransferHandler.filetransfers.Add(transfer);
        }

        private void ConnectClient(FileTransfer transfer, IPEndPoint remoteIP)
        {
            Socket socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket1.BeginConnect(remoteIP, new AsyncCallback(transfer.RemoteConnectionCallback), socket1);
        }

        private void ConnectClient(FileTransfer transfer, string acceptBody)
        {
            IPEndPoint point1 = new IPEndPoint(IPAddress.Parse(DotMSN.FileTransferHandler.IPRe.Match(acceptBody).Groups["IP"].ToString()), int.Parse(DotMSN.FileTransferHandler.PortRe.Match(acceptBody).Groups["Port"].ToString()));
            this.ConnectClient(transfer, point1);
        }

        protected void HandleAccept(FileTransfer transfer, string header, string body)
        {
            Match match1 = DotMSN.FileTransferHandler.AuthCookieRe.Match(body);
            if (match1.Success)
            {
                transfer.authCookie = int.Parse(match1.Groups["Cookie"].ToString());
                transfer.FireAccepted();
                this.ConnectClient(transfer, body);
                return;
            }
            transfer.FireAccepted();
            FileTransferSocketPair pair1 = this.StartServer(transfer);
            string[] textArray1 = new string[13];
            textArray1[0] = "IP-Address: ";
            textArray1[1] = transfer.Conversation.Messenger.ClientAddress.Address.ToString();
            textArray1[2] = "\r\nIP-Address-Internal: ";
            textArray1[3] = ((IPEndPoint) pair1.SecondarySocket.LocalEndPoint).Address.ToString();
            textArray1[4] = "\r\nPort: ";
            int num1 = ((IPEndPoint) pair1.PrimarySocket.LocalEndPoint).Port;
            textArray1[5] = num1.ToString();
            textArray1[6] = "\r\nPortX: ";
            num1 = ((IPEndPoint) pair1.SecondarySocket.LocalEndPoint).Port;
            textArray1[7] = num1.ToString();
            textArray1[8] = "\r\nAuthCookie: ";
            textArray1[9] = transfer.authCookie.ToString();
            textArray1[10] = "\r\nInvitation-Command: ACCEPT\r\nInvitation-Cookie: ";
            textArray1[11] = transfer.cookie.ToString();
            textArray1[12] = "\r\nLaunch-Application: FALSE\r\nRequest-Data: IP-Address:";
            transfer.Conversation.SendMessage(string.Concat(textArray1), "MIME-Version: 1.0\r\nContent-Type: text/x-msmsgsinvite; charset=UTF-8");
        }

        protected void HandleInvitation(Conversation conversation, Contact sender, int cookie, string header, string body)
        {
            FileTransferInvitationEventArgs args1;
            FileTransferSocketPair pair1;
            string[] textArray1;
            int num1;
            bool flag1 = true;
            FileTransfer transfer1 = new FileTransfer(conversation, sender, conversation.Messenger.Owner);
            Match match1 = DotMSN.FileTransferHandler.FileRe.Match(body);
            if (match1.Success)
            {
                transfer1.FileName = HttpUtility.UrlDecode(match1.Groups["File"].ToString());
            }
            match1 = DotMSN.FileTransferHandler.FileSizeRe.Match(body);
            if (match1.Success)
            {
                transfer1.FileSize = ((long) int.Parse(match1.Groups["FileSize"].ToString()));
            }
            match1 = DotMSN.FileTransferHandler.ConnectRe.Match(body);
            if (match1.Success)
            {
                if (match1.Groups["Connectivity"].ToString().ToLower() == "n")
                {
                    flag1 = false;
                }
                else
                {
                    flag1 = true;
                }
            }
            if (this.InvitationReceived != null)
            {
                args1 = new FileTransferInvitationEventArgs(transfer1);
                this.InvitationReceived.Invoke(this, args1);
                if (args1.Accept)
                {
                    if (flag1)
                    {
                        conversation.SendMessage(string.Concat("Invitation-Command: ACCEPT\r\nInvitation-Cookie: ", cookie.ToString(), "\r\nLaunch-Application: FALSE\r\nSender-Connect: FALSE\r\nRequest-Data: IP-Address:"), "MIME-Version: 1.0\r\nContent-Type: text/x-msmsgsinvite; charset=UTF-8");
                        return;
                    }
                    transfer1.authCookie = new Random().Next(1, 1000000);
                    pair1 = this.StartServer(transfer1);
                    textArray1 = new string[13];
                    textArray1[0] = "IP-Address: ";
                    textArray1[1] = conversation.Messenger.ClientAddress.Address.ToString();
                    textArray1[2] = "\r\nIP-Address-Internal: ";
                    textArray1[3] = ((IPEndPoint) pair1.SecondarySocket.LocalEndPoint).Address.ToString();
                    textArray1[4] = "\r\nPort: ";
                    num1 = ((IPEndPoint) pair1.PrimarySocket.LocalEndPoint).Port;
                    textArray1[5] = num1.ToString();
                    textArray1[6] = "\r\nPortX: ";
                    num1 = ((IPEndPoint) pair1.SecondarySocket.LocalEndPoint).Port;
                    textArray1[7] = num1.ToString();
                    textArray1[8] = "\r\nAuthCookie: ";
                    textArray1[9] = transfer1.authCookie.ToString();
                    textArray1[10] = "\r\nSender-Connect: TRUE\r\nInvitation-Command: ACCEPT\r\nInvitation-Cookie: ";
                    textArray1[11] = cookie.ToString();
                    textArray1[12] = "\r\nLaunch-Application: FALSE\r\nRequest-Data: IP-Address:";
                    conversation.SendMessage(string.Concat(textArray1), "MIME-Version: 1.0\r\nContent-Type: text/x-msmsgsinvite; charset=UTF-8");
                    return;
                }
                conversation.SendCommand(string.Concat("Invitation-Command: CANCEL\r\nInvitation-Cookie: ", cookie.ToString(), "\r\nCancel-Code: REJECT"));
                return;
            }
            conversation.SendCommand(string.Concat("Invitation-Command: CANCEL\r\nInvitation-Cookie: ", cookie.ToString(), "\r\nCancel-Code: REJECT"));
        }

        public void HandleMessage(Conversation conversation, Contact sender, string applicationName, int cookie, string header, string body)
        {
            FileTransfer transfer2;
            IEnumerator enumerator1;
            IDisposable disposable1;
            Match match1 = DotMSN.FileTransferHandler.CommandRe.Match(body);
            if (!match1.Success)
            {
                return;
            }
            if (match1.Groups["Command"].ToString().ToLower() == "invite")
            {
                this.HandleInvitation(conversation, sender, cookie, header, body);
                return;
            }
            if (match1.Groups["Command"].ToString().ToLower() == "accept")
            {
                foreach (FileTransfer transfer1 in DotMSN.FileTransferHandler.filetransfers)
                {
                    if (transfer1.cookie != cookie)
                    {
                        continue;
                    }
                    this.HandleAccept(transfer1, header, body);
                }
                return;
            }
            if (match1.Groups["Command"].ToString().ToLower() != "cancel")
            {
                return;
            }
            match1 = DotMSN.FileTransferHandler.CancelCode.Match(body);
            if (!match1.Success)
            {
                return;
            }
            MSNInvitationCancelCode code1 = MSNInvitationCancelCode.FAIL;
            string text1 = match1.Groups["Code"].ToString().ToLower();
            if (text1 == null)
            {
                goto Label_018D;
            }
            text1 = string.IsInterned(text1);
            if (text1 != "fail")
            {
                if (text1 == "outbandcancel")
                {
                    goto Label_017F;
                }
                if (text1 == "reject")
                {
                    goto Label_0183;
                }
                if (text1 == "reject_not_installed")
                {
                    goto Label_0187;
                }
                if (text1 == "timeout")
                {
                    goto Label_018B;
                }
                goto Label_018D;
            }
            code1 = MSNInvitationCancelCode.FAIL;
            goto Label_018D;
        Label_017F:
            code1 = MSNInvitationCancelCode.FAIL;
            goto Label_018D;
        Label_0183:
            code1 = MSNInvitationCancelCode.FAIL;
            goto Label_018D;
        Label_0187:
            code1 = MSNInvitationCancelCode.FAIL;
            goto Label_018D;
        Label_018B:
            code1 = MSNInvitationCancelCode.FAIL;
        Label_018D:
            enumerator1 = DotMSN.FileTransferHandler.filetransfers.GetEnumerator();
            try
            {
                while (enumerator1.MoveNext())
                {
                    transfer2 = ((FileTransfer) enumerator1.Current);
                    if (transfer2.cookie != cookie)
                    {
                        continue;
                    }
                    transfer2.FireInvitationCancel(code1);
                }
            }
            finally
            {
                disposable1 = (enumerator1 as IDisposable);
                if (disposable1 != null)
                {
                    disposable1.Dispose();
                }
            }
        }

        protected void IncomingConnectionCallback(IAsyncResult ar)
        {
            FileTransferSocketState state1 = ((FileTransferSocketState) ar.AsyncState);
            Socket socket1 = state1.ActiveSocket.EndAccept(ar);
            int num1 = 0;
            if (socket1.Receive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None) <= 0)
            {
                return;
            }
            socket1.Send(Encoding.ASCII.GetBytes("VER MSNFTP\r\n"));
            num1 = socket1.Receive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None);
            if (num1 <= 0)
            {
                return;
            }
            Match match1 = this.TransferInfoRe.Match(Encoding.UTF8.GetString(this.socketBuffer, 0, num1));
            if (!match1.Success)
            {
                return;
            }
            int num2 = int.Parse(match1.Groups["AuthCookie"].ToString());
            foreach (FileTransfer transfer1 in DotMSN.FileTransferHandler.filetransfers)
            {
                if (transfer1.authCookie != num2)
                {
                    continue;
                }
                try
                {
                    transfer1.StartTransfer(socket1);
                }
                finally
                {
                    socket1.Close();
                    this.RemoveFileTransfer(transfer1);
                }
            }
        }

        protected void IncomingConnectionReceiverCallback(IAsyncResult ar)
        {
            string[] textArray1;
            FileTransferSocketState state1 = ((FileTransferSocketState) ar.AsyncState);
            FileTransferSocketPair pair1 = state1.FileTransferSocketPair;
            if (!ar.IsCompleted)
            {
                return;
            }
            if (pair1.Handled)
            {
                state1.ActiveSocket.Close();
                return;
            }
            pair1.Handled = true;
            int num1 = 0;
            if (state1.ActiveSocket == pair1.PrimarySocket)
            {
                num1 = (((IPEndPoint) pair1.PrimarySocket.LocalEndPoint).Port - 6891);
                pair1.SecondarySocket.Close();
            }
            else
            {
                num1 = (((IPEndPoint) pair1.SecondarySocket.LocalEndPoint).Port - 11178);
                pair1.PrimarySocket.Close();
            }
            Socket socket1 = state1.ActiveSocket.EndAccept(ar);
            ((IPEndPoint) socket1.LocalEndPoint).ToString();
            FileTransfer transfer1 = state1.FileTransfer;
            int num2 = 0;
            try
            {
                socket1.Send(Encoding.ASCII.GetBytes("VER MSNFTP\r\n"));
                num2 = socket1.Receive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None);
                if (num2 <= 0)
                {
                    return;
                }
                if (Encoding.UTF8.GetString(this.socketBuffer, 0, num2).IndexOf("MSNFTP") <= 0)
                {
                    socket1.Close();
                    return;
                }
                textArray1 = new string[5];
                textArray1[0] = "USR ";
                textArray1[1] = transfer1.Receiver.Mail;
                textArray1[2] = " ";
                textArray1[3] = transfer1.authCookie.ToString();
                textArray1[4] = "\r\n";
                socket1.Send(Encoding.ASCII.GetBytes(string.Concat(textArray1)));
                transfer1.StartTransfer(socket1);
            }
            finally
            {
                socket1.Close();
                state1.ActiveSocket.Close();
                this.RemoveFileTransfer(transfer1);
                if ((num1 >= 0) && (num1 < 40))
                {
                    DotMSN.FileTransferHandler.portsUse[num1] = 0;
                }
            }
        }

        internal void RemoveFileTransfer(FileTransfer transfer)
        {
            DotMSN.FileTransferHandler.filetransfers.Remove(transfer);
        }

        private FileTransferSocketPair StartServer(FileTransfer transfer)
        {
            int num1;
            Socket socket1;
            Socket socket2;
            FileTransferSocketPair pair1;
            int[] numArray1;
            if (transfer.Incoming)
            {
                num1 = 1;
                numArray1 = DotMSN.FileTransferHandler.portsUse;
                lock (numArray1)
                {
                    while (((DotMSN.FileTransferHandler.portsUse[num1] == 1) && (num1 < DotMSN.FileTransferHandler.portsUse.Length)))
                    {
                        num1 += 1;
                    }
                    if (num1 >= DotMSN.FileTransferHandler.portsUse.Length)
                    {
                        throw new MSNException("Maximum number of ports used for filetransfers");
                    }
                    DotMSN.FileTransferHandler.portsUse[num1] = 1;
                }
                socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket1.Bind(new IPEndPoint(((IPEndPoint) transfer.Conversation.Messenger.socket.LocalEndPoint).Address, (6891 + (num1 * 2))));
                socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket2.Bind(new IPEndPoint(((IPEndPoint) transfer.Conversation.Messenger.socket.LocalEndPoint).Address, (11178 + (num1 * 2))));
                socket1.Listen(1);
                socket2.Listen(1);
                pair1 = new FileTransferSocketPair(socket1, socket2);
                socket1.BeginAccept(new AsyncCallback(this.IncomingConnectionReceiverCallback), new FileTransferSocketState(transfer, socket1, pair1));
                socket2.BeginAccept(new AsyncCallback(this.IncomingConnectionReceiverCallback), new FileTransferSocketState(transfer, socket2, pair1));
                return pair1;
            }
            if (DotMSN.FileTransferHandler.serverSocket == null)
            {
                DotMSN.FileTransferHandler.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                DotMSN.FileTransferHandler.serverSocket.Bind(new IPEndPoint(((IPEndPoint) transfer.Conversation.Messenger.socket.LocalEndPoint).Address, 6891));
                DotMSN.FileTransferHandler.serverSocketInternal = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                DotMSN.FileTransferHandler.serverSocketInternal.Bind(new IPEndPoint(((IPEndPoint) transfer.Conversation.Messenger.socket.LocalEndPoint).Address, 11178));
                DotMSN.FileTransferHandler.serverSocket.Listen(20);
                DotMSN.FileTransferHandler.serverSocketInternal.Listen(20);
            }
            FileTransferSocketPair pair2 = new FileTransferSocketPair(DotMSN.FileTransferHandler.serverSocket, DotMSN.FileTransferHandler.serverSocketInternal);
            DotMSN.FileTransferHandler.serverSocket.BeginAccept(new AsyncCallback(this.IncomingConnectionCallback), new FileTransferSocketState(DotMSN.FileTransferHandler.serverSocket, pair2));
            DotMSN.FileTransferHandler.serverSocketInternal.BeginAccept(new AsyncCallback(this.IncomingConnectionCallback), new FileTransferSocketState(DotMSN.FileTransferHandler.serverSocketInternal, pair2));
            DotMSN.FileTransferHandler.portsUse[0] = 1;
            return pair2;
        }

        public void TransferFile(FileTransfer transfer)
        {
            transfer.cookie = DotMSN.FileTransferHandler.randomGenerator.Next(10, 1000000);
            transfer.authCookie = DotMSN.FileTransferHandler.randomGenerator.Next(10, 1000000);
            this.AddFileTransfer(transfer);
            string text1 = "Y";
            if (this.Conversation.Messenger.ClientAddress.Address != ((IPEndPoint) this.Conversation.Messenger.socket.LocalEndPoint).Address)
            {
                text1 = "N";
            }
            string[] textArray1 = new string[8];
            textArray1[0] = "Application-Name: File Transfer\r\nApplication-GUID: {5D3E02AB-6190-11d3-BBBB-00C04F795683}\r\nInvitation-Command: INVITE\r\nInvitation-Cookie: ";
            textArray1[1] = transfer.cookie.ToString();
            textArray1[2] = "\r\nApplication-File: ";
            textArray1[3] = Path.GetFileName(transfer.FileName);
            textArray1[4] = "\r\nApplication-FileSize: ";
            textArray1[5] = transfer.FileSize.ToString();
            textArray1[6] = "\r\nConnectivity: ";
            textArray1[7] = text1;
            this.Conversation.SendMessage(string.Concat(textArray1), "MIME-Version: 1.0\r\nContent-Type: text/x-msmsgsinvite; charset=UTF-8");
        }

        public FileTransfer TransferFile(string contactMail, string file)
        {
            FileTransfer transfer1 = new FileTransfer(this.Conversation, this.Conversation.Messenger.Owner, ((Contact) this.Conversation.Users[contactMail]), file);
            this.TransferFile(transfer1);
            return transfer1;
        }


        // Fields
        private static Regex AuthCookieRe;
        private static Regex CancelCode;
        private static Regex CommandRe;
        private static Regex ConnectRe;
        public Conversation Conversation;
        private static Regex CookieRe;
        private static Regex FileRe;
        private static Regex FileSizeRe;
        protected static ArrayList filetransfers;
        private static Regex IPRe;
        private static Regex PortRe;
        private static int[] portsUse;
        private static Random randomGenerator;
        private static Socket serverSocket;
        private static Socket serverSocketInternal;
        private byte[] socketBuffer;
        private Regex TransferInfoRe;

        // Nested Types
        public delegate void FileTransferInvitationHandler(DotMSN.FileTransferHandler sender, FileTransferInvitationEventArgs e);

    }}

