namespace DotMSN
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    public class FileTransfer
    {
        // Events
        public event DotMSN.FileTransfer.FileTransferHandler Accepted;
        public event DotMSN.FileTransfer.FileTransferHandler Completed;
        public event FileTransferInvitationCancelledHandler InvitationCancelled;
        public event DotMSN.FileTransfer.FileTransferHandler Progressing;
        public event DotMSN.FileTransfer.FileTransferHandler Started;
        public event FileTransferCancelledHandler TransferCancelled;

        // Methods
        internal FileTransfer(Conversation parent, Contact sender, Contact receiver)
        {
            this.ClientData = null;
            this.BlockSize = 2048;
            this.CloseInputStream = true;
            this.ReceiveStream = new MemoryStream(4096);
            this.RemoteConnectivity = true;
            this.socketBuffer = new byte[1024];
            this.encoding = new ASCIIEncoding();
            this.FILRe = new Regex(@"FIL (?<Size>\d+)", RegexOptions.Compiled);
            this.transferThread = null;
            this.cancelTransfer = false;
            this.Conversation = parent;
            this.Sender = sender;
            this.Receiver = receiver;
        }

        internal FileTransfer(Conversation parent, Contact sender, Contact receiver, string file)
        {
            this.ClientData = null;
            this.BlockSize = 2048;
            this.CloseInputStream = true;
            this.ReceiveStream = new MemoryStream(4096);
            this.RemoteConnectivity = true;
            this.socketBuffer = new byte[1024];
            this.encoding = new ASCIIEncoding();
            this.FILRe = new Regex(@"FIL (?<Size>\d+)", RegexOptions.Compiled);
            this.transferThread = null;
            this.cancelTransfer = false;
            this.Conversation = parent;
            this.Sender = sender;
            this.Receiver = receiver;
            this.FileName = file;
            this.dataStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            this.FileSize = this.dataStream.Length;
        }

        public FileTransfer(Conversation parent, Contact sender, Contact receiver, string filename, Stream dataStream)
        {
            this.ClientData = null;
            this.BlockSize = 2048;
            this.CloseInputStream = true;
            this.ReceiveStream = new MemoryStream(4096);
            this.RemoteConnectivity = true;
            this.socketBuffer = new byte[1024];
            this.encoding = new ASCIIEncoding();
            this.FILRe = new Regex(@"FIL (?<Size>\d+)", RegexOptions.Compiled);
            this.transferThread = null;
            this.cancelTransfer = false;
            this.Conversation = parent;
            this.Sender = sender;
            this.Receiver = receiver;
            this.FileName = filename;
            this.dataStream = dataStream;
            this.FileSize = dataStream.Length;
        }

        public void Cancel()
        {
            this.cancelTransfer = true;
        }

        internal void FireAccepted()
        {
            if (this.Accepted != null)
            {
                this.Accepted.Invoke(this, new EventArgs());
            }
        }

        internal void FireInvitationCancel(MSNInvitationCancelCode cancelCode)
        {
            if (this.InvitationCancelled != null)
            {
                this.InvitationCancelled.Invoke(this, new FileTransferInvitationCancelledEventArgs(cancelCode));
            }
        }

        internal void RemoteConnectionCallback(IAsyncResult ar)
        {
            Socket socket1 = ((Socket) ar.AsyncState);
            socket1.EndConnect(ar);
            socket1.Send(this.encoding.GetBytes("VER MSNFTP\r\n"));
            int num1 = 0;
            num1 = socket1.Receive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None);
            if (num1 <= 0)
            {
                return;
            }
            if (Encoding.UTF8.GetString(this.socketBuffer, 0, num1).IndexOf("MSNFTP") <= 0)
            {
                socket1.Close();
                return;
            }
            string[] textArray1 = new string[5];
            textArray1[0] = "USR ";
            textArray1[1] = this.Sender.Mail;
            textArray1[2] = " ";
            textArray1[3] = this.authCookie.ToString();
            textArray1[4] = "\r\n";
            socket1.Send(this.encoding.GetBytes(string.Concat(textArray1)));
            try
            {
                this.StartTransfer(socket1);
            }
            finally
            {
                socket1.Close();
                this.Conversation.fileTransferHandler.RemoveFileTransfer(this);
            }
        }

        internal void StartTransfer(Socket socket)
        {
            byte[] numArray1;
            byte[] numArray2;
            int num1;
            bool flag1;
            bool flag2;
            int num2;
            string text1;
            Match match1;
            uint num3;
            byte[] numArray3;
            int num4;
            Match match2;
            NetworkStream stream1;
            int num5;
            long num6;
            this.transferThread = Thread.CurrentThread;
            try
            {
                numArray1 = new byte[2048];
                numArray2 = new byte[this.BlockSize];
                if (!this.Incoming)
                {
                    if ((this.dataStream == null) || !this.dataStream.CanRead)
                    {
                        throw new MSNException("Input datastream can not be read");
                    }
                    num6 = this.dataStream.Length;
                    socket.Send(this.encoding.GetBytes(string.Concat("FIL ", num6.ToString(), "\r\n")));
                    if (socket.Receive(numArray1, 0, numArray1.Length, SocketFlags.None) <= 0)
                    {
                        return;
                    }
                    if (this.Started != null)
                    {
                        this.Started.Invoke(this, new EventArgs());
                    }
                    this.BytesProcessed = 0;
                    this.dataStream.Seek(((long) 0), SeekOrigin.Begin);
                    num1 = 0;
                    flag1 = false;
                    flag2 = false;
                    goto Label_0205;
                    do
                    {
                        numArray2[0] = 0;
                        numArray2[1] = ((byte) (num1 % 256));
                        numArray2[2] = ((byte) (num1 / 256));
                        socket.Send(numArray2, 0, (num1 + 3), SocketFlags.None);
                        this.BytesProcessed += num1;
                        if (this.Progressing != null)
                        {
                            this.Progressing.Invoke(this, new EventArgs());
                        }
                        if (socket.Available > 0)
                        {
                            num2 = socket.Receive(numArray1, 0, numArray1.Length, SocketFlags.None);
                            text1 = this.encoding.GetString(numArray1, 0, num2);
                            match1 = new Regex(@"BYE (?<Code>\d+)").Match(text1);
                            if (match1.Success)
                            {
                                num3 = uint.Parse(match1.Groups["Code"].ToString());
                                if ((num3 == 16777987) || (num3 == 16777989))
                                {
                                    if (this.Completed != null)
                                    {
                                        this.Completed.Invoke(this, new EventArgs());
                                    }
                                    flag2 = true;
                                }
                                else
                                {
                                    flag1 = true;
                                    if (this.TransferCancelled != null)
                                    {
                                        this.TransferCancelled.Invoke(this, new FileTransferCancelledEventArgs((num3 - 2000000000)));
                                    }
                                }
                            }
                            else if (text1.IndexOf("CCL") >= 0)
                            {
                                flag1 = true;
                                if (this.TransferCancelled != null)
                                {
                                    this.TransferCancelled.Invoke(this, new FileTransferCancelledEventArgs(MSNFileTransferCancelCode.ReceiverCancelled));
                                }
                            }
                        }
                    Label_0205:
                        if (flag1 || this.cancelTransfer)
                        {
                            break;
                        }
                        num1 = this.dataStream.Read(numArray2, 3, (this.BlockSize - 3));
                    }
                    while ((num1 > 0));
                    if (flag1 || flag2)
                    {
                        return;
                    }
                    if (socket.Poll(3000000, SelectMode.SelectRead))
                    {
                        if (this.Completed == null)
                        {
                            return;
                        }
                        this.Completed.Invoke(this, new EventArgs());
                        return;
                    }
                    this.Conversation.SendMessage(string.Concat("Invitation-Command: CANCEL\r\nInvitation-Cookie: ", this.cookie.ToString(), "\r\nCancel-Code: FTTIMEOUT"), "MIME-Version: 1.0\r\nContent-Type: text/x-msmsgsinvite; charset=UTF-8");
                    this.TransferCancelled.Invoke(this, new FileTransferCancelledEventArgs(MSNFileTransferCancelCode.TimeOut));
                    return;
                }
                numArray3 = new byte[16384];
                num4 = 0;
                num4 = socket.Receive(numArray1, 0, numArray1.Length, SocketFlags.None);
                if (num4 <= 0)
                {
                    return;
                }
                match2 = this.FILRe.Match(this.encoding.GetString(numArray1, 0, num4));
                if (!match2.Success)
                {
                    return;
                }
                this.FileSize = ((long) int.Parse(match2.Groups["Size"].ToString()));
                if (this.Started != null)
                {
                    this.Started.Invoke(this, new EventArgs());
                }
                socket.Send(this.encoding.GetBytes("TFR\r\n"));
                stream1 = new NetworkStream(socket, false);
                while ((((((long) this.BytesProcessed) < this.FileSize) && !this.cancelTransfer) && (stream1.ReadByte() == 0)))
                {
                    num5 = (stream1.ReadByte() + (stream1.ReadByte() * 256));
                    if (numArray3.Length < num5)
                    {
                        numArray3 = new byte[num5];
                    }
                    stream1.Read(numArray3, 0, num5);
                    this.ReceiveStream.Write(numArray3, 0, num5);
                    this.BytesProcessed += num5;
                    if (this.Progressing == null)
                    {
                        continue;
                    }
                    this.Progressing.Invoke(this, new EventArgs());
                }
                if (((long) this.BytesProcessed) == this.FileSize)
                {
                    socket.Send(this.encoding.GetBytes("BYE 16777989\r\n"));
                    if (this.Completed != null)
                    {
                        this.Completed.Invoke(this, new EventArgs());
                    }
                }
                else
                {
                    num4 = stream1.Read(numArray3, 0, numArray3.Length);
                    if (num4 > 0)
                    {
                        this.TransferCancelled.Invoke(this, new FileTransferCancelledEventArgs(MSNFileTransferCancelCode.SenderCancelled));
                    }
                }
                stream1.Close();
            }
            catch (ThreadAbortException)
            {
                this.cancelTransfer = true;
            }
            catch (Exception exception1)
            {
                if (this.TransferCancelled != null)
                {
                    this.TransferCancelled.Invoke(this, new FileTransferCancelledEventArgs(MSNFileTransferCancelCode.GeneralError));
                }
                throw new MSNException(string.Concat("An error occured during the filetransfer: ", exception1.ToString()), exception1);
            }
            finally
            {
                if (this.cancelTransfer && socket.Connected)
                {
                    if (this.Incoming)
                    {
                        socket.Send(this.encoding.GetBytes("CCL\r\n"));
                    }
                    else
                    {
                        socket.Send(this.encoding.GetBytes("BYE 2164261683\r\n"));
                    }
                    if (this.TransferCancelled != null)
                    {
                        if (this.Incoming)
                        {
                            this.TransferCancelled.Invoke(this, new FileTransferCancelledEventArgs(MSNFileTransferCancelCode.ReceiverCancelled));
                        }
                        else
                        {
                            this.TransferCancelled.Invoke(this, new FileTransferCancelledEventArgs(MSNFileTransferCancelCode.SenderCancelled));
                        }
                    }
                }
                this.ReceiveStream.Close();
                if (this.CloseInputStream && (this.dataStream != null))
                {
                    this.dataStream.Close();
                }
            }
        }


        // Properties
        public bool Incoming
        {
            get
            {
                if (this.Conversation.Messenger.Owner.Name == this.Receiver.Name)
                {
                    return true;
                }
                return false;
            }
        }

        public Stream InputStream
        {
            get
            {
                return this.dataStream;
            }
            set
            {
                this.dataStream = this.InputStream;
            }
        }


        // Fields
        internal int authCookie;
        public int BlockSize;
        public int BytesProcessed;
        private bool cancelTransfer;
        public object ClientData;
        public bool CloseInputStream;
        public Conversation Conversation;
        internal int cookie;
        private Stream dataStream;
        private ASCIIEncoding encoding;
        public string FileName;
        public long FileSize;
        private Regex FILRe;
        public Contact Receiver;
        public Stream ReceiveStream;
        public bool RemoteConnectivity;
        public Contact Sender;
        private byte[] socketBuffer;
        private Thread transferThread;

        // Nested Types
        public delegate void FileTransferCancelledHandler(FileTransfer sender, FileTransferCancelledEventArgs e);


        public delegate void FileTransferHandler(FileTransfer sender, EventArgs e);


        public delegate void FileTransferInvitationCancelledHandler(FileTransfer sender, FileTransferInvitationCancelledEventArgs e);

    }}

