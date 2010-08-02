namespace DotMSN
{
    using System;
    using System.Collections;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    public class Conversation
    {
        // Events
        public event AllContactsLeftHandler AllContactsLeft;
        public event ConnectionClosedHandler ConnectionClosed;
        public event ConnectionEstablishedHandler ConnectionEstablished;
        public event ContactJoinHandler ContactJoin;
        public event ContactLeaveHandler ContactLeave;
        public event DotMSN.Conversation.MessageReceivedHandler MessageReceived;
        public event UserTypingHandler UserTyping;

        // Methods
        static Conversation()
        {
            Conversation.commandRe = new Regex(@"^(?<Command>[A-Z0-9]{3})\s?(?<Transaction>[0-9]+)?\s?(?<Message>.*?)$", (RegexOptions.Compiled | RegexOptions.Multiline));
            Conversation.messageRe = new Regex(@"^MSG\s+(?<Mail>\S+)\s+(?<Name>.*?)\s+(?<Length>[0-9]+)$", (RegexOptions.Compiled | RegexOptions.Multiline));
            Conversation.CALRe = new Regex(@"^RINGING\s+(?<sessionID>[0-9]+)$");
            Conversation.MSGRe = new Regex(@"^(?<Mail>[\S]+)(?<Name>.*?)(?<Length>[0-9]+)$", (RegexOptions.Compiled | RegexOptions.Multiline));
            Conversation.JOIRe = new Regex(@"^JOI\s+(?<Mail>\S+)\s+(?<Name>\S+)$", RegexOptions.Compiled);
            Conversation.IRORe = new Regex(@"^IRO\s+\d+\s+\d+\s+\d+\s+(?<Mail>\S+)\s+(?<Name>\S+)$", RegexOptions.Compiled);
            Conversation.BYERe = new Regex(@"^BYE\s+(?<Mail>\S+)$", RegexOptions.Compiled);
            Conversation.TypingRe = new Regex(@"TypingUser:\s+(?<Mail>[\w_\.@]+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            Conversation.CTRe = new Regex("Content-Type: text/x-msmsgsinvite", (RegexOptions.Compiled | RegexOptions.Multiline));
            Conversation.APPRe = new Regex("Application-Name: (?<Application>.*?)", (RegexOptions.Compiled | RegexOptions.Multiline));
            Conversation.CookieRe = new Regex(@"Invitation-Cookie: (?<Cookie>\d+)", (RegexOptions.Compiled | RegexOptions.Multiline));
        }

        internal Conversation(Messenger msn, Contact otherUser, object clientData)
        {
            this.users = new Hashtable();
            this.transaction = 0;
            this.sessionID = 0;
            this.ClientData = null;
            this.socketBuffer = new byte[4096];
            this.messenger = msn;
            this.fileTransferHandler = new DotMSN.FileTransferHandler(this);
            this.contactToCall = otherUser;
            this.ClientData = clientData;
        }

        internal Conversation(Socket switchBoard, string sbHash, Contact otherUser, Messenger msn, int session)
        {
            this.users = new Hashtable();
            this.transaction = 0;
            this.sessionID = 0;
            this.ClientData = null;
            this.socketBuffer = new byte[4096];
            this.contactToCall = otherUser;
            this.hash = sbHash;
            this.socketSB = switchBoard;
            this.messenger = msn;
            this.sessionID = session;
            this.fileTransferHandler = new DotMSN.FileTransferHandler(this);
        }

        public void Close()
        {
            if (this.socketSB.Connected)
            {
                this.socketSB.Shutdown(SocketShutdown.Both);
                this.socketSB.Close();
                if (this.ConnectionClosed != null)
                {
                    this.ConnectionClosed.Invoke(this, new EventArgs());
                }
            }
        }

        private void DataReceivedCallback(IAsyncResult ar)
        {
            Match match1;
            int num2;
            Contact contact1;
            Contact contact2;
            Contact contact3;
            int num3;
            int num4;
            string text3;
            string text4;
            int num5;
            Message message1;
            string text5;
            int num1 = this.socketSB.EndReceive(ar);
            if (num1 <= 0)
            {
                this.Close();
                return;
            }
            string text1 = this.messenger.TextEncoding.GetString(this.socketBuffer);
            string text2 = "";
            goto Label_052E;
            do
            {
                text2 = text1.Substring(0, num2);
                text1 = text1.Substring((num2 + 2));
                match1 = Conversation.commandRe.Match(text2);
                if (!match1.Success)
                {
                    goto Label_052E;
                }
                text5 = match1.Groups["Command"].ToString();
                if (text5 == null)
                {
                    goto Label_052E;
                }
                text5 = string.IsInterned(text5);
                if (text5 != "USR")
                {
                    if (text5 == "CAL")
                    {
                        goto Label_0145;
                    }
                    if (text5 == "JOI")
                    {
                        goto Label_0195;
                    }
                    if (text5 == "BYE")
                    {
                        goto Label_0235;
                    }
                    if (text5 == "IRO")
                    {
                        goto Label_02F6;
                    }
                    if (text5 == "MSG")
                    {
                        goto Label_0396;
                    }
                    goto Label_052E;
                }
                if (match1.Groups["Message"].ToString().IndexOf("OK") < 0)
                {
                    goto Label_052E;
                }
                if (this.ConnectionEstablished != null)
                {
                    this.ConnectionEstablished.Invoke(this, new EventArgs());
                }
                this.Invite(this.contactToCall);
                goto Label_052E;
            Label_0145:
                match1 = Conversation.CALRe.Match(match1.Groups["Message"].ToString());
                if (!match1.Success)
                {
                    goto Label_052E;
                }
                this.sessionID = int.Parse(match1.Groups["sessionID"].ToString());
                goto Label_052E;
            Label_0195:
                match1 = Conversation.JOIRe.Match(text2);
                if (!match1.Success)
                {
                    goto Label_052E;
                }
                contact1 = this.Messenger.GetContact(match1.Groups["Mail"].ToString());
                contact1.SetName(match1.Groups["Name"].ToString());
                if (!this.users.Contains(contact1.Mail))
                {
                    this.users.Add(contact1.Mail, contact1);
                }
                if (this.ContactJoin == null)
                {
                    goto Label_052E;
                }
                this.ContactJoin.Invoke(this, new ContactEventArgs(contact1));
                goto Label_052E;
            Label_0235:
                match1 = Conversation.BYERe.Match(text2);
                if (!match1.Success || !this.users.ContainsKey(match1.Groups["Mail"].ToString()))
                {
                    goto Label_052E;
                }
                contact2 = ((Contact) this.users[match1.Groups["Mail"].ToString()]);
                this.users.Remove(contact2.Mail);
                if (this.ContactLeave != null)
                {
                    this.ContactLeave.Invoke(this, new ContactEventArgs(contact2));
                }
                if ((this.users.Count != 0) || (this.AllContactsLeft == null))
                {
                    goto Label_052E;
                }
                this.AllContactsLeft.Invoke(this, new EventArgs());
                goto Label_052E;
            Label_02F6:
                match1 = Conversation.IRORe.Match(text2);
                if (!match1.Success)
                {
                    goto Label_052E;
                }
                contact3 = this.Messenger.GetContact(match1.Groups["Mail"].ToString());
                contact3.SetName(match1.Groups["Name"].ToString());
                if (!this.users.Contains(contact3.Mail))
                {
                    this.users.Add(contact3.Mail, contact3);
                }
                if (this.ContactJoin == null)
                {
                    goto Label_052E;
                }
                this.ContactJoin.Invoke(this, new ContactEventArgs(contact3));
                goto Label_052E;
            Label_0396:
                match1 = Conversation.messageRe.Match(text2);
                if (match1.Success)
                {
                    num3 = int.Parse(match1.Groups["Length"].ToString());
                    text2 = text1.Substring(0, num3);
                    text1 = text1.Substring(num3);
                    if (Conversation.TypingRe.Match(text2).Success)
                    {
                        if (this.UserTyping != null)
                        {
                            this.UserTyping.Invoke(this, new ContactEventArgs(this.messenger.GetContact(match1.Groups["Mail"].ToString())));
                        }
                    }
                    else
                    {
                        num4 = text2.IndexOf("\r\n\r\n");
                        text3 = text2.Substring(0, num4);
                        if (num4 > 0)
                        {
                            text4 = text2.Substring((num4 + 4));
                        }
                        else
                        {
                            text4 = "";
                        }
                        if (Conversation.CTRe.Match(text3).Success)
                        {
                            Conversation.APPRe.Match(text4);
                            num5 = int.Parse(Conversation.CookieRe.Match(text4).Groups["Cookie"].ToString());
                            this.fileTransferHandler.HandleMessage(this, this.messenger.GetContact(match1.Groups["Mail"].ToString()), "File transfer", num5, text3, text4);
                        }
                        else
                        {
                            message1 = new Message(text4, text3);
                            message1.ParseHeader();
                            if (this.MessageReceived != null)
                            {
                                this.MessageReceived.Invoke(this, new MessageEventArgs(message1, this.messenger.GetContact(match1.Groups["Mail"].ToString())));
                            }
                        }
                    }
                }
            Label_052E:
                num2 = text1.IndexOf("\r\n");
            }
            while ((num2 > 0));
            this.socketBuffer = new byte[this.socketBuffer.Length];
            if (this.socketSB.Connected)
            {
                this.socketSB.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
            }
        }

        ~Conversation()
        {
            if (!this.socketSB.Connected)
            {
                return;
            }
            this.socketSB.Shutdown(SocketShutdown.Both);
            this.socketSB.Close();
        }

        public override int GetHashCode()
        {
            return this.hash.GetHashCode();
        }

        internal void InitiateAnswer()
        {
            if (!this.socketSB.Connected)
            {
                throw new MSNException("Switchboard socket not connected when setting up chat connection");
            }
            object[] objArray1 = new object[9];
            objArray1[0] = "ANS ";
            int num1 = (this.transaction + 1);
            this.transaction = num1;
            objArray1[1] = num1;
            objArray1[2] = " ";
            objArray1[3] = this.messenger.Owner.Mail;
            objArray1[4] = " ";
            objArray1[5] = this.hash;
            objArray1[6] = " ";
            objArray1[7] = this.sessionID;
            objArray1[8] = "\r\n";
            this.socketSB.Send(this.messenger.TextEncoding.GetBytes(string.Concat(objArray1)));
            this.socketSB.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
        }

        internal void InitiateRequest()
        {
            if (!this.socketSB.Connected)
            {
                throw new MSNException("Switchboard socket not connected when setting up chat connection");
            }
            object[] objArray1 = new object[7];
            objArray1[0] = "USR ";
            int num1 = (this.transaction + 1);
            this.transaction = num1;
            objArray1[1] = num1;
            objArray1[2] = " ";
            objArray1[3] = this.messenger.Owner.Mail;
            objArray1[4] = " ";
            objArray1[5] = this.hash;
            objArray1[6] = "\r\n";
            this.socketSB.Send(this.messenger.TextEncoding.GetBytes(string.Concat(objArray1)));
            this.socketSB.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
        }

        public void Invite(Contact contact)
        {
            object[] objArray1 = new object[5];
            objArray1[0] = "CAL ";
            int num1 = (this.transaction + 1);
            this.transaction = num1;
            objArray1[1] = num1;
            objArray1[2] = " ";
            objArray1[3] = contact.Mail;
            objArray1[4] = "\r\n";
            this.socketSB.Send(this.messenger.TextEncoding.GetBytes(string.Concat(objArray1)));
        }

        internal void SendCommand(string message)
        {
            this.socketSB.Send(this.messenger.TextEncoding.GetBytes(message));
        }

        public void SendMessage(string body)
        {
            this.SendMessage(body, "MIME-Version: 1.0\r\nContent-Type: text/plain; charset=UTF-8\r\nX-MMS-IM-Format: FN=Microsoft%20Sans%20Serif; EF=; CO=0; CS=0; PF=22");
        }

        public void SendMessage(string body, MSNCharset charset)
        {
            this.SendMessage(body, string.Concat("MIME-Version: 1.0\r\nContent-Type: text/plain; charset=UTF-8\r\nX-MMS-IM-Format: FN=Microsoft%20Sans%20Serif; EF=; CO=0; CS=", charset, "; PF=22"));
        }

        public void SendMessage(string body, string header)
        {
            if (!this.socketSB.Connected)
            {
                throw new MSNException("Failed to send message: connection already closed");
            }
            byte[] numArray1 = this.messenger.TextEncoding.GetBytes(string.Concat(header, body));
            int num1 = (numArray1.Length + 4);
            object[] objArray1 = new object[8];
            objArray1[0] = "MSG ";
            int num2 = (this.transaction + 1);
            this.transaction = num2;
            objArray1[1] = num2;
            objArray1[2] = " N ";
            objArray1[3] = num1;
            objArray1[4] = "\r\n";
            objArray1[5] = header;
            objArray1[6] = "\r\n\r\n";
            objArray1[7] = body;
            this.socketSB.Send(this.messenger.TextEncoding.GetBytes(string.Concat(objArray1)));
        }

        public void SendMessage(string body, string header, string commandHeader)
        {
            if (!this.socketSB.Connected)
            {
                throw new MSNException("Failed to send message: connection already closed");
            }
            this.socketSB.Send(this.messenger.TextEncoding.GetBytes(string.Concat(commandHeader, header, body)));
        }

        internal void SetConnection(Socket switchBoard, string sbHash)
        {
            this.hash = sbHash;
            this.socketSB = switchBoard;
        }


        // Properties
        public bool Connected
        {
            get
            {
                if (this.socketSB != null)
                {
                    return this.socketSB.Connected;
                }
                return false;
            }
        }

        public DotMSN.FileTransferHandler FileTransferHandler
        {
            get
            {
                return this.fileTransferHandler;
            }
        }

        public bool Invited
        {
            get
            {
                return (this.sessionID != 0);
            }
        }

        public Messenger Messenger
        {
            get
            {
                return this.messenger;
            }
        }

        public Hashtable Users
        {
            get
            {
                return this.users;
            }
        }


        // Fields
        private static Regex APPRe;
        private static Regex BYERe;
        private static Regex CALRe;
        public object ClientData;
        private static Regex commandRe;
        private Contact contactToCall;
        private static Regex CookieRe;
        private static Regex CTRe;
        internal DotMSN.FileTransferHandler fileTransferHandler;
        private string hash;
        private static Regex IRORe;
        private static Regex JOIRe;
        private static Regex messageRe;
        private Messenger messenger;
        private static Regex MSGRe;
        private int sessionID;
        private byte[] socketBuffer;
        private Socket socketSB;
        private int transaction;
        private static Regex TypingRe;
        private Hashtable users;

        // Nested Types
        public delegate void AllContactsLeftHandler(Conversation sender, EventArgs e);


        public delegate void ConnectionClosedHandler(Conversation sender, EventArgs e);


        public delegate void ConnectionEstablishedHandler(Conversation sender, EventArgs e);


        public delegate void ContactJoinHandler(Conversation sender, ContactEventArgs e);


        public delegate void ContactLeaveHandler(Conversation sender, ContactEventArgs e);


        public delegate void MessageReceivedHandler(Conversation sender, MessageEventArgs e);


        public delegate void UserTypingHandler(Conversation sender, ContactEventArgs e);

    }}

