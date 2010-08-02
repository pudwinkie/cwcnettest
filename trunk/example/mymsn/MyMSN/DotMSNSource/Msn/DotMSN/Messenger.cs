namespace DotMSN
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Messenger
    {
        // Events
        public event ConnectionFailureHandler ConnectionFailure;
        public event ContactAddedHandler ContactAdded;
        public event ContactGroupAddedHandler ContactGroupAdded;
        public event DotMSN.Messenger.ContactGroupChangedHandler ContactGroupChanged;
        public event ContactGroupRemovedHandler ContactGroupRemoved;
        public event DotMSN.Messenger.ContactOfflineHandler ContactOffline;
        public event DotMSN.Messenger.ContactOnlineHandler ContactOnline;
        public event ContactRemovedHandler ContactRemoved;
        public event ContactStatusChangeHandler ContactStatusChange;
        public event ConversationCreatedHandler ConversationCreated;
        public event ErrorReceivedHandler ErrorReceived;
        public event ListReceivedHandler ListReceived;
        public event MailboxStatusHandler MailboxStatus;
        public event DotMSN.Messenger.MessageReceivedHandler MessageReceived;
        public event ReverseAddedHandler ReverseAdded;
        public event ReverseRemovedHandler ReverseRemoved;
        public event SynchronizationCompletedHandler SynchronizationCompleted;

        // Methods
        static Messenger()
        {
            Messenger.messageRe = new Regex(@"^(?<Command>[A-Z0-9]{3})\s+(?<Message>.*?)$", RegexOptions.Compiled);
            Messenger.CHLRe = new Regex(@"(?<Transaction>[0-9]+)\s+(?<Hash>\d+)", RegexOptions.Compiled);
            Messenger.ILNRe = new Regex(@"(?<Transaction>[0-9]+)\s+(?<Status>[A-Z]{3})\s+(?<Mail>\S+)\s+(?<Name>\S+)", RegexOptions.Compiled);
            Messenger.NLNRe = new Regex(@"(?<Status>[A-Z]{3})\s+(?<Mail>\S+)\s+(?<Name>\S+)", RegexOptions.Compiled);
            Messenger.FLNRe = new Regex(@"(?<Mail>\S+)", RegexOptions.Compiled);
            Messenger.LSTRe8 = new Regex(@"(?<Mail>\S+)\s+(?<Name>.*?)\s+(?<Lists>[0-9]+)\s?(?<Groups>[\S]+)?$", RegexOptions.Compiled);
            Messenger.LSTRe7 = new Regex(@"(?<Transaction>[0-9]+)\s+(?<Type>\w{2})\s+(?<Version>[0-9]+)\s+(?<Nr>[0-9]+)\s+(?<Total>[0-9]+)\s+(?<Mail>\S+)\s+(?<Name>.*?)\s?(?<GroupID>[0-9]+)?$", RegexOptions.Compiled);
            Messenger.XFRRe = new Regex(@"(?<Transaction>[0-9]+)\s+SB\s+(?<IP>[0-9\.]+):(?<Port>[0-9]+)\s+([A-Z]+)\s+(?<Hash>[0-9\.]+)$", RegexOptions.Compiled);
            Messenger.RNGRe = new Regex(@"(?<Session>[0-9]+)\s+(?<IP>[0-9\.]+):(?<Port>[0-9]+)\s+CKI\s+(?<Hash>[0-9\.]+)\s+(?<Mail>\S+)\s+(?<Name>.*?)$", RegexOptions.Compiled);
            Messenger.MSGRe = new Regex(@"(?<ServiceName>\w+)\s+(?<ServiceName2>\w+)\s+(?<Length>[0-9]+)$", RegexOptions.Compiled);
            Messenger.splitRe = new Regex("\r\n");
            Messenger.BPRRe7 = new Regex(@"(?<SyncID>[0-9]+)\s+(?<Mail>\S+)\s+(?<Type>[\w]+)\s?(?<Value>.*?)$", RegexOptions.Compiled);
            Messenger.BPRRe8 = new Regex(@"(?<Type>[\w]+)\s+(?<Value>.*?)$", RegexOptions.Compiled);
            Messenger.LSGRe = new Regex(@"(?<GroupID>[0-9]+)\s+(?<Name>[\w\._@]+)\s+[0-9]+$", RegexOptions.Compiled);
            Messenger.REARe = new Regex(@"(?<TransID>[0-9]+)\s+(?<PassportID>[0-9]+)\s+(?<Mail>\S+)\s+(?<Name>\S+)$", RegexOptions.Compiled);
            Messenger.ADDRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<Type>[\w]+)\s+(?<ListVersion>[0-9]+)\s+(?<Mail>\S+)\s+(?<Name>\S+)\s?(?<Group>[0-9]+)?", RegexOptions.Compiled);
            Messenger.REMRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<Type>[\w]+)\s+(?<ListVersion>[0-9]+)\s+(?<Mail>\S+)\s?(?<Group>[0-9]+)?", RegexOptions.Compiled);
            Messenger.SYNRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<SyncID>[0-9]+)\s?(?<UsersCount>[0-9]+)?\s?(?<GroupsCount>[0-9]+)?", RegexOptions.Compiled);
            Messenger.BLPRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<SyncID>[0-9]+)?\s?(?<Mode>[\w]+)", RegexOptions.Compiled);
            Messenger.GTCRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<SyncID>[0-9]+)?\s?(?<Mode>[\w]+)", RegexOptions.Compiled);
            Messenger.ADG = new Regex(@"(?<TransID>[0-9]+)\s+(?<ListSync>\d+)\s+(?<Name>\S+)\s+(?<GroupID>\d+)", RegexOptions.Compiled);
            Messenger.RMG = new Regex(@"(?<TransID>[0-9]+)\s+(?<ListSync>\d+)\s+(?<GroupID>\d+)", RegexOptions.Compiled);
            Messenger.REG = new Regex(@"(?<TransID>[0-9]+)\s+(?<ListSync>\d+)\s+(?<Name>\S+)\s+(?<GroupID>\d+)", RegexOptions.Compiled);
        }

        public Messenger()
        {
            this.messengerServer = IPAddress.Parse("64.4.13.17");
            this.lastContactSynced = null;
            this.syncContactsCount = 0;
            this.networkConnected = false;
            this.InitialStatus = MSNStatus.Online;
            this.log = new ArrayList();
            this.conversationQueue = new Queue();
            this.conversationList = new ArrayList();
            this.contacts = new ContactList();
            this.contactGroups = new Hashtable();
            this.currentTransaction = 0;
            this.TextEncoding = new UTF8Encoding();
            this.socketBuffer = new byte[32768];
            this.synSended = false;
            this.totalMessage = "";
            IPHostEntry entry1 = Dns.Resolve("messenger.hotmail.com");
            this.MessengerServer = entry1.AddressList[0];
        }

        public void AddContact(string mail)
        {
            object[] objArray1 = new object[7];
            objArray1[0] = "ADD ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " AL ";
            objArray1[3] = mail;
            objArray1[4] = " ";
            objArray1[5] = mail;
            objArray1[6] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
            Thread.Sleep(100);
            objArray1 = new object[7];
            objArray1[0] = "ADD ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " FL ";
            objArray1[3] = mail;
            objArray1[4] = " ";
            objArray1[5] = mail;
            objArray1[6] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        public void AddContact(string mail, ContactGroup group)
        {
            object[] objArray1 = new object[7];
            objArray1[0] = "ADD ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " AL ";
            objArray1[3] = mail;
            objArray1[4] = " ";
            objArray1[5] = mail;
            objArray1[6] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
            objArray1 = new object[9];
            objArray1[0] = "ADD ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " FL ";
            objArray1[3] = mail;
            objArray1[4] = " ";
            objArray1[5] = mail;
            objArray1[6] = " ";
            objArray1[7] = group.ID;
            objArray1[8] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        public void AddGroup(string groupName)
        {
            object[] objArray1 = new object[5];
            objArray1[0] = "ADG ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " ";
            objArray1[3] = Messenger.URLEncode(groupName);
            objArray1[4] = " 0\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        internal void AddToList(Contact contact, MSNList list)
        {
            object[] objArray1 = new object[9];
            objArray1[0] = "ADD ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " ";
            objArray1[3] = this.GetMSNList(list);
            objArray1[4] = " ";
            objArray1[5] = contact.Mail;
            objArray1[6] = " ";
            objArray1[7] = contact.Mail;
            objArray1[8] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        private string AuthenticatePassport(string twnString)
        {
            WebRequest request1;
            WebResponse response1;
            string text1;
            Regex regex1;
            Match match1;
            string text2;
            Uri uri1;
            string text3;
            string text4;
            try
            {
                request1 = WebRequest.Create("https://nexus.passport.com/rdr/pprdr.asp");
                response1 = request1.GetResponse();
                text1 = response1.Headers.Get("PassportURLs");
                regex1 = new Regex("DALogin=([^,]+)");
                match1 = regex1.Match(text1);
                if (!match1.Success)
                {
                    throw new MSNException("Regular expression failed; no DALogin (messenger login server) could be extracted");
                }
                text2 = match1.Groups[1].ToString();
                uri1 = new Uri(string.Concat("https://", text2));
                response1.Close();
                response1 = this.PassportServerLogin(uri1, twnString);
                text3 = response1.Headers.Get("Authentication-Info");
                regex1 = new Regex("from-PP=\'([^\']+)\'");
                match1 = regex1.Match(text3);
                if (!match1.Success)
                {
                    throw new MSNException("Regular expression failed; no ticket could be extracted");
                }
                text3 = match1.Groups[1].ToString();
                response1.Close();
                return text3;
            }
            catch (Exception exception1)
            {
                throw new MSNException(string.Concat("Authenticating with passport.com failed : ", exception1.ToString()), exception1);
            }
            return text4;
        }

        public void BlockContact(Contact contact)
        {
            object[] objArray1 = new object[5];
            objArray1[0] = "REM ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " AL ";
            objArray1[3] = contact.mail;
            objArray1[4] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
            Thread.Sleep(150);
            objArray1 = new object[7];
            objArray1[0] = "ADD ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " BL ";
            objArray1[3] = contact.mail;
            objArray1[4] = " ";
            objArray1[5] = contact.name;
            objArray1[6] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        public void ChangeGroup(Contact contact, ContactGroup group)
        {
            object[] objArray1 = new object[9];
            objArray1[0] = "ADD ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " FL ";
            objArray1[3] = contact.mail;
            objArray1[4] = " ";
            objArray1[5] = contact.name;
            objArray1[6] = " ";
            objArray1[7] = group.ID;
            objArray1[8] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
            objArray1 = new object[7];
            objArray1[0] = "REM ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " FL ";
            objArray1[3] = contact.mail;
            objArray1[4] = " ";
            objArray1[5] = contact.contactGroup;
            objArray1[6] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        internal void ChangeScreenName(string NewName)
        {
            object[] objArray1;
            if (this.owner == null)
            {
                throw new MSNException("Not a valid owner");
            }
            if (this.socket.Connected)
            {
                objArray1 = new object[7];
                objArray1[0] = "REA ";
                objArray1[1] = this.NewTrans();
                objArray1[2] = " ";
                objArray1[3] = this.owner.Mail;
                objArray1[4] = " ";
                objArray1[5] = Messenger.URLEncode(NewName);
                objArray1[6] = "\r\n";
                this.SocketSend(string.Concat(objArray1));
                return;
            }
            throw new MSNException("Not connected to the messenger network");
        }

        private string CheckedSendAndReceive(string text)
        {
            string text1 = this.SendAndReceive(text);
            if (text1.IndexOf("XFR", 0, 3) > -1)
            {
                this.SwitchNameserver(text1);
            }
            return text1;
        }

        public void CloseConnection()
        {
            if ((this.socket != null) && this.socket.Connected)
            {
                this.socket.Shutdown(SocketShutdown.Both);
                this.socket.Close();
            }
            this.networkConnected = false;
            this.conversationList.Clear();
            this.conversationQueue.Clear();
            this.contacts.Clear();
        }

        public void Connect(DotMSN.Connection connection, Owner owner)
        {
            this.connection = connection;
            this.owner = owner;
            this.owner.messenger = this;
            this.DoConnect();
        }

        public void Connect(string user, string password)
        {
            this.Connect(new DotMSN.Connection("messenger.hotmail.com", 1863), new Owner(user, password));
        }

        private void conversation_ConnectionClosed(Conversation sender, EventArgs e)
        {
            this.conversationList.Remove(sender);
        }

        private void DataReceivedCallback(IAsyncResult ar)
        {
            Match match1;
            int num2;
            string text1;
            string text2;
            Contact contact1;
            MSNStatus status1;
            Contact contact2;
            MSNStatus status2;
            Contact contact3;
            MSNStatus status3;
            Contact contact4;
            MSNList list1;
            MSNList list2;
            Contact contact5;
            string[] textArray1;
            string text3;
            IPEndPoint point1;
            Socket socket1;
            Conversation conversation1;
            IPEndPoint point2;
            Socket socket2;
            Conversation conversation2;
            Contact contact6;
            Contact contact7;
            int num3;
            string text4;
            Contact contact8;
            MSNList list3;
            Contact contact9;
            int num4;
            MSNList list4;
            Contact contact10;
            int num5;
            int num6;
            int num7;
            ContactGroup group1;
            int num8;
            ContactGroup group2;
            int num9;
            string text5;
            int num10;
            object obj1;
            object[] objArray1;
            char[] chArray1;
            string[] textArray2;
            int num11;
            string text6;
            if (<PrivateImplementationDetails>.$$method0x600002f-1 == null)
            {
                Hashtable hashtable1 = new Hashtable(40, 0.5f);
                hashtable1.Add("CHL", 0);
                Hashtable hashtable2 = new Hashtable(40, 0.5f);
                hashtable2.Add("ILN", 1);
                Hashtable hashtable3 = new Hashtable(40, 0.5f);
                hashtable3.Add("NLN", 2);
                Hashtable hashtable4 = new Hashtable(40, 0.5f);
                hashtable4.Add("FLN", 3);
                Hashtable hashtable5 = new Hashtable(40, 0.5f);
                hashtable5.Add("LST", 4);
                Hashtable hashtable6 = new Hashtable(40, 0.5f);
                hashtable6.Add("XFR", 5);
                Hashtable hashtable7 = new Hashtable(40, 0.5f);
                hashtable7.Add("RNG", 6);
                Hashtable hashtable8 = new Hashtable(40, 0.5f);
                hashtable8.Add("BPR", 7);
                Hashtable hashtable9 = new Hashtable(40, 0.5f);
                hashtable9.Add("LSG", 8);
                Hashtable hashtable10 = new Hashtable(40, 0.5f);
                hashtable10.Add("SYN", 9);
                Hashtable hashtable11 = new Hashtable(40, 0.5f);
                hashtable11.Add("REA", 10);
                Hashtable hashtable12 = new Hashtable(40, 0.5f);
                hashtable12.Add("ADD", 11);
                Hashtable hashtable13 = new Hashtable(40, 0.5f);
                hashtable13.Add("REM", 12);
                Hashtable hashtable14 = new Hashtable(40, 0.5f);
                hashtable14.Add("BLP", 13);
                Hashtable hashtable15 = new Hashtable(40, 0.5f);
                hashtable15.Add("GTC", 14);
                Hashtable hashtable16 = new Hashtable(40, 0.5f);
                hashtable16.Add("ADG", 15);
                Hashtable hashtable17 = new Hashtable(40, 0.5f);
                hashtable17.Add("RMG", 16);
                Hashtable hashtable18 = new Hashtable(40, 0.5f);
                hashtable18.Add("REG", 17);
                Hashtable hashtable19 = new Hashtable(40, 0.5f);
                hashtable19.Add("MSG", 18);
                <PrivateImplementationDetails>.$$method0x600002f-1 = new Hashtable(40, 0.5f);
            }
            if ((this.socket == null) || !this.socket.Connected)
            {
                return;
            }
            int num1 = 0;
            try
            {
                num1 = this.socket.EndReceive(ar);
                if (num1 <= 0)
                {
                    this.CloseConnection();
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            this.totalMessage = string.Concat(this.totalMessage, this.TextEncoding.GetString(this.socketBuffer, 0, num1));
            goto Label_145A;
            do
            {
                text1 = this.totalMessage.Substring(0, num2);
                this.totalMessage = this.totalMessage.Remove(0, (num2 + 2));
                if (this.MessageReceived != null)
                {
                    this.MessageReceived.Invoke(this, text1);
                }
                match1 = Messenger.messageRe.Match(text1);
                if (!match1.Success)
                {
                    goto Label_145A;
                }
                text1 = match1.Groups["Message"].ToString();
                obj1 = match1.Groups[1].ToString();
                if (obj1 == null)
                {
                    goto Label_140D;
                }
                obj1 = <PrivateImplementationDetails>.$$method0x600002f-1[obj1];
                if (obj1 == null)
                {
                    goto Label_140D;
                }
                switch (((int) obj1))
                {
                    case 0:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.CHLRe, text1);
                        if (!match1.Success)
                        {
                            goto Label_145A;
                        }
                        text2 = this.HashMD5(string.Concat(match1.Groups["Hash"].ToString(), "Q1P7W2E4J9R8U3S5"));
                        objArray1 = new object[4];
                        objArray1[0] = "QRY ";
                        objArray1[1] = this.NewTrans();
                        objArray1[2] = " msmsgs@msnmsgr.com 32\r\n";
                        objArray1[3] = text2;
                        this.SocketSend(string.Concat(objArray1));
                        goto Label_145A;
                    }
                    case 1:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.ILNRe, text1);
                        if (!match1.Success)
                        {
                            goto Label_145A;
                        }
                        contact1 = this.GetContact(match1.Groups["Mail"].ToString());
                        contact1.SetName(match1.Groups["Name"].ToString());
                        status1 = contact1.Status;
                        contact1.SetStatus(this.ParseStatus(match1.Groups["Status"].ToString()));
                        if (((status1 == MSNStatus.Unknown) || (status1 == MSNStatus.Offline)) && (this.ContactOnline != null))
                        {
                            this.ContactOnline.Invoke(this, new ContactEventArgs(contact1));
                        }
                        if (this.ContactStatusChange == null)
                        {
                            goto Label_145A;
                        }
                        this.ContactStatusChange.Invoke(this, new ContactStatusChangeEventArgs(contact1, status1));
                        goto Label_145A;
                    }
                    case 2:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.NLNRe, text1);
                        if (!match1.Success)
                        {
                            goto Label_145A;
                        }
                        contact2 = this.GetContact(match1.Groups["Mail"].ToString());
                        contact2.SetName(match1.Groups["Name"].ToString());
                        status2 = contact2.status;
                        contact2.SetStatus(this.ParseStatus(match1.Groups["Status"].ToString()));
                        if (this.ContactStatusChange != null)
                        {
                            this.ContactStatusChange.Invoke(this, new ContactStatusChangeEventArgs(contact2, status2));
                        }
                        match1.Groups["Name"].ToString();
                        if (this.ContactOnline == null)
                        {
                            goto Label_145A;
                        }
                        this.ContactOnline.Invoke(this, new ContactEventArgs(contact2));
                        goto Label_145A;
                    }
                    case 3:
                    {
                        match1 = Messenger.FLNRe.Match(text1);
                        if (!match1.Success)
                        {
                            goto Label_145A;
                        }
                        contact3 = this.GetContact(match1.Groups["Mail"].ToString());
                        status3 = contact3.status;
                        contact3.SetStatus(MSNStatus.Offline);
                        if (this.ContactStatusChange != null)
                        {
                            this.ContactStatusChange.Invoke(this, new ContactStatusChangeEventArgs(contact3, status3));
                        }
                        if (this.ContactOffline == null)
                        {
                            goto Label_145A;
                        }
                        this.ContactOffline.Invoke(this, new ContactEventArgs(contact3));
                        goto Label_145A;
                    }
                    case 4:
                    {
                        this.syncContactsCount -= 1;
                        match1 = Messenger.RunRegularExpression(Messenger.LSTRe7, text1);
                        if (!match1.Success)
                        {
                            goto Label_06B5;
                        }
                        contact4 = this.GetContact(match1.Groups["Mail"].ToString());
                        list1 = this.GetMSNList(match1.Groups["Type"].ToString());
                        Contact contact11 = contact4;
                        contact4.lists = (contact11.lists | list1);
                        contact4.SetName(match1.Groups["Name"].ToString());
                        if (match1.Groups["GroupID"].ToString().Length > 0)
                        {
                            contact4.SetContactGroup(int.Parse(match1.Groups["GroupID"].ToString()));
                        }
                        contact4.updateVersion = int.Parse(match1.Groups["Version"].ToString());
                        if (match1.Groups["Nr"].ToString() != match1.Groups["Total"].ToString())
                        {
                            goto Label_145A;
                        }
                        list2 = this.GetMSNList(match1.Groups["Type"].ToString());
                        if (this.ListReceived == null)
                        {
                            goto Label_145A;
                        }
                        this.ListReceived.Invoke(this, new ListReceivedEventArgs(list2));
                        goto Label_145A;
                    }
                    case 5:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.XFRRe, text1);
                        if (!match1.Success)
                        {
                            goto Label_145A;
                        }
                        point1 = new IPEndPoint(IPAddress.Parse(match1.Groups["IP"].ToString()), int.Parse(match1.Groups["Port"].ToString()));
                        socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket1.Connect(point1);
                        if (!socket1.Connected)
                        {
                            this.log.Add("Could not connect");
                            objArray1 = new object[4];
                            objArray1[0] = "Could not connect to switchboard server @ ";
                            objArray1[1] = match1.Groups["IP"];
                            objArray1[2] = ":";
                            objArray1[3] = match1.Groups["Port"];
                            throw new MSNException(string.Concat(objArray1));
                        }
                        if (this.conversationQueue.Count > 0)
                        {
                            conversation1 = ((Conversation) this.conversationQueue.Dequeue());
                        }
                        else
                        {
                            throw new MSNException("Server sends chatanswer but there are no conversations left in the queue");
                        }
                        conversation1.SetConnection(socket1, match1.Groups["Hash"].ToString());
                        this.conversationList.Add(conversation1);
                        conversation1.ConnectionClosed += new ConnectionClosedHandler(this.conversation_ConnectionClosed);
                        if (this.ConversationCreated != null)
                        {
                            this.ConversationCreated.Invoke(this, new ConversationEventArgs(conversation1));
                        }
                        conversation1.InitiateRequest();
                        goto Label_145A;
                    }
                    case 6:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.RNGRe, text1);
                        if (!match1.Success)
                        {
                            goto Label_145A;
                        }
                        point2 = new IPEndPoint(IPAddress.Parse(match1.Groups["IP"].ToString()), int.Parse(match1.Groups["Port"].ToString()));
                        socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket2.Connect(point2);
                        if (!socket2.Connected)
                        {
                            this.log.Add("Could not connect");
                            objArray1 = new object[4];
                            objArray1[0] = "Could not connect to switchboard server @ ";
                            objArray1[1] = match1.Groups["IP"];
                            objArray1[2] = ":";
                            objArray1[3] = match1.Groups["Port"];
                            throw new MSNException(string.Concat(objArray1));
                        }
                        conversation2 = new Conversation(socket2, match1.Groups["Hash"].ToString(), this.GetContact(match1.Groups["Mail"].ToString()), this, int.Parse(match1.Groups["Session"].ToString()));
                        this.conversationList.Add(conversation2);
                        conversation2.ConnectionClosed += new ConnectionClosedHandler(this.conversation_ConnectionClosed);
                        if (this.ConversationCreated != null)
                        {
                            this.ConversationCreated.Invoke(this, new ConversationEventArgs(conversation2));
                        }
                        conversation2.InitiateAnswer();
                        goto Label_145A;
                    }
                    case 7:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.BPRRe7, text1);
                        if (!match1.Success)
                        {
                            goto Label_0BCA;
                        }
                        contact6 = this.GetContact(match1.Groups["Mail"].ToString());
                        goto Label_0AE3;
                    }
                    case 8:
                    {
                        goto Label_0CF8;
                    }
                    case 9:
                    {
                        goto Label_0D79;
                    }
                    case 10:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.REARe, text1);
                        if (!match1.Success)
                        {
                            goto Label_145A;
                        }
                        if ((this.owner == null) || (match1.Groups["Mail"].ToString() != this.owner.mail))
                        {
                            goto Label_0E81;
                        }
                        this.owner.SetName(match1.Groups["Name"].ToString());
                        goto Label_145A;
                    }
                    case 11:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.ADDRe, text1);
                        if (!match1.Success)
                        {
                            goto Label_145A;
                        }
                        list3 = this.GetMSNList(match1.Groups["Type"].ToString());
                        contact9 = this.GetContact(match1.Groups["Mail"].ToString());
                        contact9.SetName(match1.Groups["Name"].ToString());
                        num4 = 0;
                        if (match1.Groups["Group"].ToString().Length > 0)
                        {
                            num4 = int.Parse(match1.Groups["Group"].ToString());
                        }
                        contact9.AddToList(list3);
                        if (num4 > 0)
                        {
                            contact9.SetContactGroup(num4);
                        }
                        if ((list3 == MSNList.ReverseList) && (this.ReverseAdded != null))
                        {
                            this.ReverseAdded.Invoke(this, new ContactEventArgs(contact9));
                        }
                        if (this.ContactAdded == null)
                        {
                            goto Label_145A;
                        }
                        this.ContactAdded.Invoke(this, new ListMutateEventArgs(contact9, list3));
                        goto Label_145A;
                    }
                    case 12:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.REMRe, text1);
                        if (!match1.Success)
                        {
                            goto Label_145A;
                        }
                        list4 = this.GetMSNList(match1.Groups["Type"].ToString());
                        contact10 = this.GetContact(match1.Groups["Mail"].ToString());
                        num5 = 0;
                        if (match1.Groups["Group"].ToString().Length > 0)
                        {
                            num5 = int.Parse(match1.Groups["Group"].ToString());
                        }
                        contact10.RemoveFromList(list4);
                        if (num5 > 0)
                        {
                            contact10.SetContactGroup(num5);
                        }
                        if ((list4 == MSNList.ReverseList) && (this.ReverseRemoved != null))
                        {
                            this.ReverseRemoved.Invoke(this, new ContactEventArgs(contact10));
                        }
                        if (this.ContactRemoved == null)
                        {
                            goto Label_145A;
                        }
                        this.ContactRemoved.Invoke(this, new ListMutateEventArgs(contact10, list4));
                        goto Label_145A;
                    }
                    case 13:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.BLPRe, text1);
                        if (!match1.Success || (this.owner == null))
                        {
                            goto Label_145A;
                        }
                        goto Label_10DB;
                    }
                    case 14:
                    {
                        match1 = Messenger.RunRegularExpression(Messenger.GTCRe, text1);
                        if (!match1.Success || (this.owner == null))
                        {
                            goto Label_145A;
                        }
                        goto Label_1168;
                    }
                    case 15:
                    {
                        goto Label_11C7;
                    }
                    case 16:
                    {
                        goto Label_1269;
                    }
                    case 17:
                    {
                        goto Label_12ED;
                    }
                    case 18:
                    {
                        goto Label_1395;
                    }
                }
                goto Label_140D;
            Label_06B5:
                match1 = Messenger.RunRegularExpression(Messenger.LSTRe8, text1);
                if (!match1.Success)
                {
                    goto Label_145A;
                }
                contact5 = this.GetContact(match1.Groups["Mail"].ToString());
                this.lastContactSynced = contact5;
                contact5.lists = ((MSNList) int.Parse(match1.Groups["Lists"].ToString()));
                contact5.updateVersion = this.lastSync;
                contact5.SetName(match1.Groups["Name"].ToString());
                chArray1 = new char[1];
                chArray1[0] = ',';
                textArray1 = match1.Groups["Groups"].ToString().Split(chArray1);
                textArray2 = textArray1;
                for (num11 = 0; (num11 < textArray2.Length); num11 += 1)
                {
                    text3 = textArray2[num11];
                    if (text3 != "")
                    {
                        contact5.SetContactGroup(int.Parse(text3));
                    }
                }
                if ((this.syncContactsCount > 0) || (this.SynchronizationCompleted == null))
                {
                    goto Label_145A;
                }
                this.SynchronizationCompleted.Invoke(this, new EventArgs());
                goto Label_145A;
            Label_0AE3:
                text6 = match1.Groups["Type"].ToString();
                if (text6 == null)
                {
                    goto Label_145A;
                }
                text6 = string.IsInterned(text6);
                if (text6 != "PHH")
                {
                    if (text6 == "PHW")
                    {
                        goto Label_0B58;
                    }
                    if (text6 == "PHM")
                    {
                        goto Label_0B7E;
                    }
                    if (text6 == "MOB")
                    {
                        goto Label_0BA4;
                    }
                    goto Label_145A;
                }
                contact6.homePhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                goto Label_145A;
            Label_0B58:
                contact6.workPhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                goto Label_145A;
            Label_0B7E:
                contact6.mobilePhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                goto Label_145A;
            Label_0BA4:
                contact6.mobilePager = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                goto Label_145A;
            Label_0BCA:
                match1 = Messenger.RunRegularExpression(Messenger.BPRRe8, text1);
                if (!match1.Success)
                {
                    goto Label_145A;
                }
                contact7 = this.lastContactSynced;
                if (contact7 == null)
                {
                    goto Label_0CED;
                }
                text6 = match1.Groups["Type"].ToString();
                if (text6 == null)
                {
                    goto Label_145A;
                }
                text6 = string.IsInterned(text6);
                if (text6 != "PHH")
                {
                    if (text6 == "PHW")
                    {
                        goto Label_0C7B;
                    }
                    if (text6 == "PHM")
                    {
                        goto Label_0CA1;
                    }
                    if (text6 == "MOB")
                    {
                        goto Label_0CC7;
                    }
                    goto Label_145A;
                }
                contact7.homePhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                goto Label_145A;
            Label_0C7B:
                contact7.workPhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                goto Label_145A;
            Label_0CA1:
                contact7.mobilePhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                goto Label_145A;
            Label_0CC7:
                contact7.mobilePager = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                goto Label_145A;
            Label_0CED:
                throw new MSNException("Phone numbers are sent but lastContact == null");
            Label_0CF8:
                match1 = Messenger.RunRegularExpression(Messenger.LSGRe, text1);
                if (!match1.Success)
                {
                    goto Label_145A;
                }
                try
                {
                    this.contactGroups.Add(int.Parse(match1.Groups["GroupID"].ToString()), new ContactGroup(match1.Groups["Name"].ToString(), int.Parse(match1.Groups["GroupID"].ToString()), this));
                    goto Label_145A;
                }
                catch (FormatException)
                {
                    goto Label_145A;
                }
            Label_0D79:
                match1 = Messenger.RunRegularExpression(Messenger.SYNRe, text1);
                if (!match1.Success)
                {
                    goto Label_145A;
                }
                num3 = int.Parse(match1.Groups["SyncID"].ToString());
                if (this.lastSync == num3)
                {
                    this.syncContactsCount = 0;
                    if (this.SynchronizationCompleted == null)
                    {
                        goto Label_145A;
                    }
                    this.SynchronizationCompleted.Invoke(this, new EventArgs());
                    goto Label_145A;
                }
                this.lastSync = num3;
                this.lastContactSynced = null;
                text4 = match1.Groups["UsersCount"].Value;
                this.syncContactsCount = int.Parse(text4);
                goto Label_145A;
            Label_0E81:
                contact8 = this.GetContact(match1.Groups["Mail"].ToString());
                contact8.SetName(match1.Groups["Name"].ToString());
                goto Label_145A;
            Label_10DB:
                text6 = match1.Groups["Mode"].ToString();
                if (text6 == null)
                {
                    goto Label_145A;
                }
                text6 = string.IsInterned(text6);
                if (text6 != "AL")
                {
                    if (text6 == "BL")
                    {
                        goto Label_1129;
                    }
                    goto Label_145A;
                }
                this.owner.privacy = MSNPrivacy.AllExceptBlocked;
                goto Label_145A;
            Label_1129:
                this.owner.privacy = MSNPrivacy.NoneButAllowed;
                goto Label_145A;
            Label_1168:
                text6 = match1.Groups["Mode"].ToString();
                if (text6 == null)
                {
                    goto Label_145A;
                }
                text6 = string.IsInterned(text6);
                if (text6 != "A")
                {
                    if (text6 == "N")
                    {
                        goto Label_11B6;
                    }
                    goto Label_145A;
                }
                this.owner.notifyPrivacy = MSNNotifyPrivacy.PromptOnAdd;
                goto Label_145A;
            Label_11B6:
                this.owner.notifyPrivacy = MSNNotifyPrivacy.AutomaticAdd;
                goto Label_145A;
            Label_11C7:
                match1 = Messenger.RunRegularExpression(Messenger.ADG, text1);
                if (!match1.Success)
                {
                    goto Label_145A;
                }
                try
                {
                    num6 = int.Parse(match1.Groups["GroupID"].ToString());
                    this.contactGroups[num6] = new ContactGroup(Messenger.URLDecode(match1.Groups["Name"].ToString()), num6, this);
                    if (this.ContactGroupAdded == null)
                    {
                        goto Label_145A;
                    }
                    this.ContactGroupAdded.Invoke(this, new ContactGroupEventArgs(((ContactGroup) this.contactGroups[num6])));
                    goto Label_145A;
                }
                catch (Exception)
                {
                    goto Label_145A;
                }
            Label_1269:
                match1 = Messenger.RunRegularExpression(Messenger.RMG, text1);
                if (!match1.Success)
                {
                    goto Label_145A;
                }
                try
                {
                    num7 = int.Parse(match1.Groups["GroupID"].ToString());
                    group1 = ((ContactGroup) this.contactGroups[num7]);
                    this.contactGroups.Remove(num7);
                    if (this.ContactGroupRemoved == null)
                    {
                        goto Label_145A;
                    }
                    this.ContactGroupRemoved.Invoke(this, new ContactGroupEventArgs(group1));
                    goto Label_145A;
                }
                catch (Exception)
                {
                    goto Label_145A;
                }
            Label_12ED:
                match1 = Messenger.RunRegularExpression(Messenger.REG, text1);
                if (!match1.Success)
                {
                    goto Label_145A;
                }
                try
                {
                    num8 = int.Parse(match1.Groups["GroupID"].ToString());
                    group2 = ((ContactGroup) this.contactGroups[num8]);
                    group2.Name = Messenger.URLDecode(match1.Groups["Name"].ToString());
                    if (this.ContactGroupChanged == null)
                    {
                        goto Label_145A;
                    }
                    this.ContactGroupChanged.Invoke(this, new ContactGroupEventArgs(((ContactGroup) this.contactGroups[num8])));
                    goto Label_145A;
                }
                catch (Exception)
                {
                    goto Label_145A;
                }
            Label_1395:
                match1 = Messenger.MSGRe.Match(match1.Groups["Message"].ToString());
                if (!match1.Success)
                {
                    goto Label_145A;
                }
                try
                {
                    num9 = int.Parse(match1.Groups["Length"].ToString());
                    text5 = this.totalMessage.Substring(0, num9);
                    this.totalMessage = this.totalMessage.Remove(0, num9);
                    this.ParseHotmailMessages(text5);
                    goto Label_145A;
                }
                catch (FormatException)
                {
                    goto Label_145A;
                }
            Label_140D:
                try
                {
                    num10 = int.Parse(match1.Groups[1].ToString());
                    if (this.ErrorReceived != null)
                    {
                        this.ErrorReceived.Invoke(this, new MSNErrorEventArgs(((MSNError) Enum.ToObject(typeof(MSNError), num10))));
                    }
                }
                catch (FormatException)
                {
                }
            Label_145A:
                num2 = this.totalMessage.IndexOf("\r\n");
            }
            while ((num2 > 0));
            this.socketBuffer = new byte[this.socketBuffer.Length];
            if (!this.socket.Connected)
            {
                return;
            }
            try
            {
                this.socket.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
            }
            catch (SocketException exception1)
            {
                if (this.ConnectionFailure == null)
                {
                    return;
                }
                this.ConnectionFailure.Invoke(this, new ConnectionErrorEventArgs(exception1));
            }
        }

        protected void DoConnect()
        {
            Regex regex1;
            Match match1;
            string text2;
            string text3;
            this.log.Add("Starting connection");
            if (this.Connected)
            {
                this.CloseConnection();
            }
            Encoding.ASCII;
            new byte[1024];
            IPHostEntry entry1 = Dns.GetHostByName("messenger.hotmail.com");
            if (entry1.AddressList.Length <= 0)
            {
                throw new MSNException("Could not resolve IP adress for messenger.hotmail.com. Check your DNS settings.");
            }
            this.MessengerServer = entry1.AddressList[0];
            IPEndPoint point1 = new IPEndPoint(this.MessengerServer, 1863);
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socket.Connect(point1);
            if (!this.socket.Connected)
            {
                this.log.Add("Could not connect");
                throw new MSNException("Could not connect");
            }
            this.SendAndReceive(string.Concat("VER ", this.NewTrans(), " MSNP8 CVR0"));
            object[] objArray1 = new object[4];
            objArray1[0] = "CVR ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " 0x0409 win 4.10 i386 MSNMSGR 5.0.0544 MSMSGS ";
            objArray1[3] = this.Owner.Mail;
            string text1 = this.SendAndReceive(string.Concat(objArray1));
            objArray1 = new object[4];
            objArray1[0] = "USR ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " TWN I ";
            objArray1[3] = this.owner.Mail;
            text1 = this.SendAndReceive(string.Concat(objArray1));
            if (text1.IndexOf("NS") >= 0)
            {
                this.SwitchNameserver(text1);
                objArray1 = new object[4];
                objArray1[0] = "CVR ";
                objArray1[1] = this.NewTrans();
                objArray1[2] = " 0x0409 win 4.10 i386 MSNMSGR 5.0.0544 MSMSGS ";
                objArray1[3] = this.Owner.Mail;
                this.CheckedSendAndReceive(string.Concat(objArray1));
                objArray1 = new object[4];
                objArray1[0] = "USR ";
                objArray1[1] = this.NewTrans();
                objArray1[2] = " TWN I ";
                objArray1[3] = this.owner.Mail;
                text1 = this.CheckedSendAndReceive(string.Concat(objArray1));
                regex1 = new Regex(@"USR\s+[0-9]+\s+TWN\s+S\s+(\S+)\r\n");
                match1 = regex1.Match(text1);
                if (!match1.Success)
                {
                    throw new MSNException("Regular expression failed; no TWN string could be extracted");
                }
                text2 = match1.Groups[1].ToString();
                text3 = this.AuthenticatePassport(text2);
                objArray1 = new object[4];
                objArray1[0] = "USR ";
                objArray1[1] = this.NewTrans();
                objArray1[2] = " TWN S ";
                objArray1[3] = text3;
                this.CheckedSendAndReceive(string.Concat(objArray1));
            }
            this.networkConnected = true;
            this.socket.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
        }

        ~Messenger()
        {
            this.CloseConnection();
        }

        public Contact GetContact(string mail)
        {
            Contact contact1;
            mail = mail.ToLower();
            if (!this.contacts.ContainsKey(mail))
            {
                contact1 = new Contact();
                contact1.mail = mail;
                contact1.name = mail;
                contact1.messenger = this;
                contact1.contactGroup = 0;
                this.contacts.Add(mail, contact1);
                return ((Contact) this.contacts[mail]);
            }
            return ((Contact) this.contacts[mail]);
        }

        public ContactGroup GetContactGroup(string groupName)
        {
            foreach (ContactGroup group1 in this.contactGroups.Values)
            {
                if (group1.Name != groupName)
                {
                    continue;
                }
                return group1;
            }
            return null;
        }

        public ListEnumerator GetListEnumerator(MSNList type)
        {
            MSNList list1 = type;
            switch (list1)
            {
                case MSNList.ForwardList:
                {
                    goto Label_0020;
                }
                case MSNList.AllowedList:
                {
                    goto Label_0035;
                }
                case (MSNList.AllowedList | MSNList.ForwardList):
                {
                    goto Label_003C;
                }
                case MSNList.BlockedList:
                {
                    goto Label_0027;
                }
            }
            if (list1 == MSNList.ReverseList)
            {
                goto Label_002E;
            }
            goto Label_003C;
        Label_0020:
            return this.ForwardList;
        Label_0027:
            return this.BlockedList;
        Label_002E:
            return this.ReverseList;
        Label_0035:
            return this.AllowedList;
        Label_003C:
            return this.AllList;
        }

        public ArrayList GetLog()
        {
            return this.log;
        }

        protected string GetMSNList(MSNList list)
        {
            MSNList list1 = list;
            switch (list1)
            {
                case MSNList.ForwardList:
                {
                    goto Label_0026;
                }
                case MSNList.AllowedList:
                {
                    goto Label_0020;
                }
                case (MSNList.AllowedList | MSNList.ForwardList):
                {
                    goto Label_0038;
                }
                case MSNList.BlockedList:
                {
                    goto Label_002C;
                }
            }
            if (list1 == MSNList.ReverseList)
            {
                goto Label_0032;
            }
            goto Label_0038;
        Label_0020:
            return "AL";
        Label_0026:
            return "FL";
        Label_002C:
            return "BL";
        Label_0032:
            return "RL";
        Label_0038:
            throw new MSNException("Unknown MSNList type");
        }

        protected MSNList GetMSNList(string name)
        {
            string text1 = name;
            if (text1 == null)
            {
                goto Label_004C;
            }
            text1 = string.IsInterned(text1);
            if (text1 != "AL")
            {
                if (text1 == "FL")
                {
                    goto Label_0046;
                }
                if (text1 == "BL")
                {
                    goto Label_0048;
                }
                if (text1 == "RL")
                {
                    goto Label_004A;
                }
                goto Label_004C;
            }
            return MSNList.AllowedList;
        Label_0046:
            return MSNList.ForwardList;
        Label_0048:
            return MSNList.BlockedList;
        Label_004A:
            return MSNList.ReverseList;
        Label_004C:
            throw new MSNException("Unknown MSNList type");
        }

        private string[] GetWords(string line)
        {
            return Regex.Split(line, @"\s+");
        }

        protected string HashMD5(string input)
        {
            int num1;
            byte[] numArray1 = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(this.TextEncoding.GetBytes(input));
            string text1 = "";
            for (num1 = 0; (num1 < numArray1.Length); num1 += 1)
            {
                text1 = string.Concat(text1, numArray1[num1].ToString("x2"));
            }
            return text1;
        }

        protected int NewTrans()
        {
            int num1 = this.currentTransaction;
            this.currentTransaction = (num1 + 1);
            return num1;
        }

        private void ParseHotmailMessages(string body)
        {
            int num1;
            int num2;
            string text1;
            string text2;
            string text3;
            Regex regex1 = new Regex(@"Content-Type:\s+(?<ContentType>[\w/\-0-9]+)", (RegexOptions.Compiled | RegexOptions.Multiline));
            Match match1 = regex1.Match(body);
            if (!match1.Success)
            {
                return;
            }
            match1.Groups["ContentType"].ToString();
            string text5 = match1.Groups["ContentType"].ToString();
            if (text5 == null)
            {
                return;
            }
            text5 = string.IsInterned(text5);
            if (((text5 != "text/x-msmsgsinitialemailnotification") && (text5 != "text/x-msmsgsemailnotification")) && ((text5 != "application/x-msmsgsemailnotification") && (text5 != "application/x-msmsgsinitialemailnotification")))
            {
                if (text5 != "text/x-msmsgsprofile")
                {
                    return;
                }
            }
            else
            {
                try
                {
                    num1 = int.Parse(Regex.Match(body, @"Inbox-Unread:\s+(?<InboxUnread>[0-9]+)").Groups[1].ToString());
                    num2 = int.Parse(Regex.Match(body, @"Folders-Unread:\s+(?<FoldersUnread>[0-9]+)").Groups[1].ToString());
                    text1 = Regex.Match(body, @"Inbox-URL:\s+(?<InboxURL>\S+)").Groups[1].ToString();
                    text2 = Regex.Match(body, @"Folders-URL:\s+(?<FoldersURL>\S+)").Groups[1].ToString();
                    text3 = Regex.Match(body, @"Post-URL:\s+(?<PostURL>\S+)").Groups[1].ToString();
                    if (this.MailboxStatus == null)
                    {
                        return;
                    }
                    this.MailboxStatus.Invoke(this, new MailboxStatusEventArgs(num1, num2, text1, text2, text3));
                    return;
                }
                catch (Exception)
                {
                    return;
                }
            }
            string text4 = Regex.Match(body, @"ClientIP:\s+(?<IP>\S+)").Groups[1].ToString();
            int num3 = int.Parse(Regex.Match(body, @"ClientPort:\s+(?<Port>\d+)").Groups[1].ToString());
            num3 = (((num3 & 255) * 256) + ((num3 & 65280) / 256));
            this.clientAddress = new IPEndPoint(IPAddress.Parse(text4), num3);
        }

        private string ParseMessage(string line)
        {
            Regex regex1 = new Regex(@"^[A-Z0-9]{3}\s+[0-9]+\s+(.*?)$", RegexOptions.Compiled);
            Match match1 = regex1.Match(line);
            if (match1.Success)
            {
                return match1.Groups[1].ToString();
            }
            throw new MSNException(string.Concat("Could not parse transaction-based message in line: ", line));
        }

        protected string ParseStatus(MSNStatus status)
        {
            MSNStatus status1 = status;
            switch (status1)
            {
                case MSNStatus.Offline:
                {
                    goto Label_005A;
                }
                case MSNStatus.Hidden:
                {
                    goto Label_0060;
                }
                case MSNStatus.Online:
                {
                    goto Label_0030;
                }
                case MSNStatus.Away:
                {
                    goto Label_0048;
                }
                case MSNStatus.Busy:
                {
                    goto Label_0036;
                }
                case MSNStatus.BRB:
                {
                    goto Label_0042;
                }
                case MSNStatus.Lunch:
                {
                    goto Label_0054;
                }
                case MSNStatus.Phone:
                {
                    goto Label_004E;
                }
                case MSNStatus.Idle:
                {
                    goto Label_003C;
                }
            }
            goto Label_0066;
        Label_0030:
            return "NLN";
        Label_0036:
            return "BSY";
        Label_003C:
            return "IDL";
        Label_0042:
            return "BRB";
        Label_0048:
            return "AWY";
        Label_004E:
            return "PHN";
        Label_0054:
            return "LUN";
        Label_005A:
            return "FLN";
        Label_0060:
            return "HDN";
        Label_0066:
            return "Unknown status";
        }

        protected MSNStatus ParseStatus(string status)
        {
            if (<PrivateImplementationDetails>.$$method0x6000027-1 == null)
            {
                Hashtable hashtable1 = new Hashtable(20, 0.5f);
                hashtable1.Add("NLN", 0);
                Hashtable hashtable2 = new Hashtable(20, 0.5f);
                hashtable2.Add("BSY", 1);
                Hashtable hashtable3 = new Hashtable(20, 0.5f);
                hashtable3.Add("IDL", 2);
                Hashtable hashtable4 = new Hashtable(20, 0.5f);
                hashtable4.Add("BRB", 3);
                Hashtable hashtable5 = new Hashtable(20, 0.5f);
                hashtable5.Add("AWY", 4);
                Hashtable hashtable6 = new Hashtable(20, 0.5f);
                hashtable6.Add("PHN", 5);
                Hashtable hashtable7 = new Hashtable(20, 0.5f);
                hashtable7.Add("LUN", 6);
                Hashtable hashtable8 = new Hashtable(20, 0.5f);
                hashtable8.Add("FLN", 7);
                Hashtable hashtable9 = new Hashtable(20, 0.5f);
                hashtable9.Add("HDN", 8);
                <PrivateImplementationDetails>.$$method0x6000027-1 = new Hashtable(20, 0.5f);
            }
            object obj1 = status;
            if (obj1 == null)
            {
                goto Label_0113;
            }
            obj1 = <PrivateImplementationDetails>.$$method0x6000027-1[obj1];
            if (obj1 == null)
            {
                goto Label_0113;
            }
            switch (((int) obj1))
            {
                case 0:
                {
                    goto Label_0100;
                }
                case 1:
                {
                    goto Label_0102;
                }
                case 2:
                {
                    goto Label_0104;
                }
                case 3:
                {
                    goto Label_0107;
                }
                case 4:
                {
                    goto Label_0109;
                }
                case 5:
                {
                    goto Label_010B;
                }
                case 6:
                {
                    goto Label_010D;
                }
                case 7:
                {
                    goto Label_010F;
                }
                case 8:
                {
                    goto Label_0111;
                }
            }
            goto Label_0113;
        Label_0100:
            return MSNStatus.Online;
        Label_0102:
            return MSNStatus.Busy;
        Label_0104:
            return MSNStatus.Idle;
        Label_0107:
            return MSNStatus.BRB;
        Label_0109:
            return MSNStatus.Away;
        Label_010B:
            return MSNStatus.Phone;
        Label_010D:
            return MSNStatus.Lunch;
        Label_010F:
            return MSNStatus.Offline;
        Label_0111:
            return MSNStatus.Hidden;
        Label_0113:
            return MSNStatus.Unknown;
        }

        private string ParseTransaction(string line)
        {
            Regex regex1 = new Regex(@"^[A-Z0-9]{3}\s+([0-9]+)", RegexOptions.Compiled);
            Match match1 = regex1.Match(line);
            if (match1.Success)
            {
                return match1.Groups[1].ToString();
            }
            throw new MSNException(string.Concat("Could not parse transaction ID line: ", line));
        }

        private WebResponse PassportServerLogin(Uri serverUri, string twnString)
        {
            string text2;
            HttpWebRequest request1 = ((HttpWebRequest) WebRequest.Create(serverUri));
            request1.Headers.Clear();
            string[] textArray1 = new string[6];
            textArray1[0] = "Authorization: Passport1.4 OrgVerb=GET,OrgURL=http%3A%2F%2Fmessenger%2Emsn%2Ecom,sign-in=";
            textArray1[1] = HttpUtility.UrlEncode(this.Owner.Mail);
            textArray1[2] = ",pwd=";
            textArray1[3] = HttpUtility.UrlEncode(this.Owner.Password);
            textArray1[4] = ",";
            textArray1[5] = twnString;
            string text1 = string.Concat(textArray1);
            request1.Headers.Add(text1);
            request1.Headers.ToString();
            request1.AllowAutoRedirect = false;
            request1.PreAuthenticate = false;
            HttpWebResponse response1 = ((HttpWebResponse) request1.GetResponse());
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                return response1;
            }
            if (response1.StatusCode == HttpStatusCode.Found)
            {
                text2 = response1.Headers.Get("Location");
                response1.Close();
                return this.PassportServerLogin(new Uri(text2), twnString);
            }
            if (response1.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new MSNException(string.Concat("Failed to login. Response of passport server: ", response1.Headers[0]));
            }
            throw new MSNException("Passport server responded with an unknown header");
        }

        public void RemoveContact(Contact contact)
        {
            object[] objArray1 = new object[5];
            objArray1[0] = "REM ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " FL ";
            objArray1[3] = contact.mail;
            objArray1[4] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
            Thread.Sleep(150);
            objArray1 = new object[5];
            objArray1[0] = "REM ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " AL ";
            objArray1[3] = contact.mail;
            objArray1[4] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        internal void RemoveFromList(Contact contact, MSNList list)
        {
            object[] objArray1 = new object[9];
            objArray1[0] = "REM ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " ";
            objArray1[3] = this.GetMSNList(list);
            objArray1[4] = " ";
            objArray1[5] = contact.Mail;
            objArray1[6] = " ";
            objArray1[7] = contact.Mail;
            objArray1[8] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        public void RemoveGroup(ContactGroup group)
        {
            object[] objArray1 = new object[5];
            objArray1[0] = "RMG ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " ";
            objArray1[3] = group.ID;
            objArray1[4] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        public void RemoveGroup(string groupName)
        {
            ContactGroup group1 = this.GetContactGroup(groupName);
            if (group1 != null)
            {
                this.RemoveGroup(group1);
                return;
            }
            throw new MSNException("Groupname was not found");
        }

        internal void RenameGroup(ContactGroup group, string newGroupName)
        {
            object[] objArray1 = new object[7];
            objArray1[0] = "REG ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " ";
            objArray1[3] = group.ID;
            objArray1[4] = " ";
            objArray1[5] = HttpUtility.UrlEncode(newGroupName);
            objArray1[6] = " 0\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        public Conversation RequestConversation(string mail)
        {
            return this.RequestConversation(mail, null);
        }

        public Conversation RequestConversation(string mail, object clientData)
        {
            Conversation conversation1 = new Conversation(this, this.GetContact(mail), clientData);
            this.conversationQueue.Enqueue(conversation1);
            this.SocketSend(string.Concat("XFR ", this.NewTrans(), " SB\r\n"));
            return conversation1;
        }

        internal void RequestScreenName(Contact contact)
        {
            object[] objArray1 = new object[7];
            objArray1[0] = "REA ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " ";
            objArray1[3] = contact.Mail;
            objArray1[4] = " ";
            objArray1[5] = contact.Name;
            objArray1[6] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        private static Match RunRegularExpression(Regex process, string target)
        {
            Match match1;
            try
            {
                return process.Match(target);
            }
            catch (FormatException exception1)
            {
                throw new RegularExpressionException(process, target, exception1);
            }
            return match1;
        }

        public void SendAllowedListRequest()
        {
            this.SendListRequest("AL");
        }

        private string SendAndReceive(string text)
        {
            this.log.Add(string.Concat(">", text));
            if (this.socket.Send(this.TextEncoding.GetBytes(string.Concat(text, "\r\n"))) == 0)
            {
                throw new MSNException("Send failed");
            }
            byte[] numArray1 = new byte[1024];
            int num1 = 0;
            if (!this.socket.Connected)
            {
                throw new MSNException("Connection dropped");
            }
            num1 = this.socket.Receive(numArray1, numArray1.Length, SocketFlags.None);
            char[] chArray1 = new char[num1];
            System.Text.Decoder decoder1 = Encoding.UTF8.GetDecoder();
            decoder1.GetChars(numArray1, 0, num1, chArray1, 0);
            return new string(chArray1);
        }

        public void SendBlockedListRequest()
        {
            this.SendListRequest("BL");
        }

        public void SendForwardListRequest()
        {
            this.SendListRequest("FL");
        }

        private void SendListRequest(string type)
        {
            if (this.socket == null)
            {
                throw new MSNException("Socket is null");
            }
            object[] objArray1 = new object[5];
            objArray1[0] = "LST ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " ";
            objArray1[3] = type;
            objArray1[4] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        public void SendReverseListRequest()
        {
            this.SendListRequest("RL");
        }

        internal void SetNotifyPrivacy(MSNNotifyPrivacy privacy)
        {
            if (privacy == MSNNotifyPrivacy.AutomaticAdd)
            {
                this.SocketSend(string.Concat("GTC ", this.NewTrans(), " N\r\n"));
            }
            if (privacy == MSNNotifyPrivacy.PromptOnAdd)
            {
                this.SocketSend(string.Concat("GTC ", this.NewTrans(), " A\r\n"));
            }
        }

        internal void SetPrivacy(MSNPrivacy privacy)
        {
            if (privacy == MSNPrivacy.AllExceptBlocked)
            {
                this.SocketSend(string.Concat("BLP ", this.NewTrans(), " AL\r\n"));
            }
            if (privacy == MSNPrivacy.NoneButAllowed)
            {
                this.SocketSend(string.Concat("BLP ", this.NewTrans(), " BL\r\n"));
            }
        }

        public void SetStatus(MSNStatus status)
        {
            if (!this.synSended)
            {
                throw new MSNException("Can\'t set status. You must call SynchronizeList() before you can set an initial status.");
            }
            object[] objArray1 = new object[5];
            objArray1[0] = "CHG ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " ";
            objArray1[3] = this.ParseStatus(status);
            objArray1[4] = " 0\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        protected void SocketSend(string text)
        {
            if (this.socket.Send(this.TextEncoding.GetBytes(text)) == 0)
            {
                throw new MSNException("Send failed");
            }
        }

        private void SwitchNameserver(string serverString)
        {
            Regex regex1 = new Regex(@"([0-9\.]+):([0-9]+)");
            Match match1 = regex1.Match(serverString);
            if (!match1.Success)
            {
                throw new MSNException("Regular expression failed; no Name server could be extracted");
            }
            IPAddress address1 = IPAddress.Parse(match1.Groups[1].ToString());
            IPEndPoint point1 = new IPEndPoint(address1, int.Parse(match1.Groups[2].ToString()));
            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Close();
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socket.Connect(point1);
            if (!this.socket.Connected)
            {
                this.log.Add("Could not connect");
                throw new MSNException("Could not connect");
            }
            this.log.Add(string.Concat("Changing nameserver: ", address1));
            this.SendAndReceive(string.Concat("VER ", this.NewTrans(), " MSNP8 CVR0"));
        }

        public void SynchronizeList()
        {
            this.SocketSend(string.Concat("SYN ", this.NewTrans(), " 0\r\n"));
            this.synSended = true;
        }

        public void UnBlockContact(Contact contact)
        {
            object[] objArray1 = new object[5];
            objArray1[0] = "REM ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " BL ";
            objArray1[3] = contact.mail;
            objArray1[4] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
            Thread.Sleep(150);
            objArray1 = new object[7];
            objArray1[0] = "ADD ";
            objArray1[1] = this.NewTrans();
            objArray1[2] = " AL ";
            objArray1[3] = contact.mail;
            objArray1[4] = " ";
            objArray1[5] = contact.name;
            objArray1[6] = "\r\n";
            this.SocketSend(string.Concat(objArray1));
        }

        public static string URLDecode(string text)
        {
            return HttpUtility.UrlDecode(text);
        }

        public static string URLEncode(string text)
        {
            return Regex.Replace(HttpUtility.UrlEncode(text), @"\+", "%20");
        }


        // Properties
        public ListEnumerator AllList
        {
            get
            {
                return new ListEnumerator(this.contacts.GetEnumerator());
            }
        }

        public AllowedListEnumerator AllowedList
        {
            get
            {
                return new AllowedListEnumerator(this.contacts.GetEnumerator());
            }
        }

        public BlockedListEnumerator BlockedList
        {
            get
            {
                return new BlockedListEnumerator(this.contacts.GetEnumerator());
            }
        }

        public IPEndPoint ClientAddress
        {
            get
            {
                return this.clientAddress;
            }
        }

        public bool Connected
        {
            get
            {
                if ((this.socket != null) && this.socket.Connected)
                {
                    return this.networkConnected;
                }
                return false;
            }
        }

        public Hashtable ContactGroups
        {
            get
            {
                return this.contactGroups;
            }
        }

        public ArrayList Conversations
        {
            get
            {
                return this.conversationList;
            }
        }

        public ForwardListEnumerator ForwardList
        {
            get
            {
                return new ForwardListEnumerator(this.contacts.GetEnumerator());
            }
        }

        public IPAddress MessengerServer
        {
            get
            {
                return this.messengerServer;
            }
            set
            {
                this.messengerServer = value;
            }
        }

        public Owner Owner
        {
            get
            {
                return this.owner;
            }
        }

        public ReverseListEnumerator ReverseList
        {
            get
            {
                return new ReverseListEnumerator(this.contacts.GetEnumerator());
            }
        }


        // Fields
        private static Regex ADDRe;
        private static Regex ADG;
        private static Regex BLPRe;
        private static Regex BPRRe7;
        private static Regex BPRRe8;
        private static Regex CHLRe;
        private IPEndPoint clientAddress;
        public object ClientData;
        private DotMSN.Connection connection;
        private Hashtable contactGroups;
        private ContactList contacts;
        private ArrayList conversationList;
        private Queue conversationQueue;
        private int currentTransaction;
        private static Regex FLNRe;
        private static Regex GTCRe;
        private static Regex ILNRe;
        public MSNStatus InitialStatus;
        private Contact lastContactSynced;
        protected int lastSync;
        private ArrayList log;
        private static Regex LSGRe;
        private static Regex LSTRe7;
        private static Regex LSTRe8;
        private static Regex messageRe;
        private IPAddress messengerServer;
        private static Regex MSGRe;
        private bool networkConnected;
        private static Regex NLNRe;
        private Owner owner;
        private static Regex REARe;
        private static Regex REG;
        private static Regex REMRe;
        private static Regex RMG;
        private static Regex RNGRe;
        internal Socket socket;
        private byte[] socketBuffer;
        private static Regex splitRe;
        private int syncContactsCount;
        private static Regex SYNRe;
        protected bool synSended;
        internal UTF8Encoding TextEncoding;
        private string totalMessage;
        private static Regex XFRRe;

        // Nested Types
        public delegate void ConnectionFailureHandler(Messenger sender, ConnectionErrorEventArgs e);


        public delegate void ContactAddedHandler(Messenger sender, ListMutateEventArgs e);


        public delegate void ContactGroupAddedHandler(Messenger sender, ContactGroupEventArgs e);


        public delegate void ContactGroupChangedHandler(Messenger sender, ContactGroupEventArgs e);


        public delegate void ContactGroupRemovedHandler(Messenger sender, ContactGroupEventArgs e);


        public class ContactList : Hashtable
        {
            // Methods
            public ContactList()
            {
            }


            // Nested Types
            public class AllowedListEnumerator : ListEnumerator
            {
                // Methods
                public AllowedListEnumerator(IDictionaryEnumerator listEnum) : base(listEnum)
                {
                }

                public override bool MoveNext()
                {
                    while (this.baseEnum.MoveNext())
                    {
                        if ((((Contact) this.baseEnum.Value).lists & MSNList.AllowedList) > ((MSNList) 0))
                        {
                            return true;
                        }
                    }
                    return false;
                }

            }

            public class BlockedListEnumerator : ListEnumerator
            {
                // Methods
                public BlockedListEnumerator(IDictionaryEnumerator listEnum) : base(listEnum)
                {
                }

                public override bool MoveNext()
                {
                    while (this.baseEnum.MoveNext())
                    {
                        if (((Contact) this.baseEnum.Value).Blocked)
                        {
                            return true;
                        }
                    }
                    return false;
                }

            }

            public class ForwardListEnumerator : ListEnumerator
            {
                // Methods
                public ForwardListEnumerator(IDictionaryEnumerator listEnum) : base(listEnum)
                {
                }

                public override bool MoveNext()
                {
                    while (this.baseEnum.MoveNext())
                    {
                        if ((((Contact) this.baseEnum.Value).lists & MSNList.ForwardList) > ((MSNList) 0))
                        {
                            return true;
                        }
                    }
                    return false;
                }

            }

            public class ListEnumerator : IEnumerator
            {
                // Methods
                public ListEnumerator(IDictionaryEnumerator listEnum)
                {
                    this.baseEnum = listEnum;
                }

                public IEnumerator GetEnumerator()
                {
                    return this;
                }

                public virtual bool MoveNext()
                {
                    return this.baseEnum.MoveNext();
                }

                public void Reset()
                {
                    this.baseEnum.Reset();
                }


                // Properties
                public object Current
                {
                    get
                    {
                        return this.baseEnum.Value;
                    }
                }


                // Fields
                protected IDictionaryEnumerator baseEnum;
            }

            public class ReverseListEnumerator : ListEnumerator
            {
                // Methods
                public ReverseListEnumerator(IDictionaryEnumerator listEnum) : base(listEnum)
                {
                }

                public override bool MoveNext()
                {
                    while (this.baseEnum.MoveNext())
                    {
                        if ((((Contact) this.baseEnum.Value).lists & MSNList.ReverseList) > ((MSNList) 0))
                        {
                            return true;
                        }
                    }
                    return false;
                }

            }
        }

        public delegate void ContactOfflineHandler(Messenger sender, ContactEventArgs e);


        public delegate void ContactOnlineHandler(Messenger sender, ContactEventArgs e);


        public delegate void ContactRemovedHandler(Messenger sender, ListMutateEventArgs e);


        public delegate void ContactStatusChangeHandler(Messenger sender, ContactStatusChangeEventArgs e);


        public delegate void ConversationCreatedHandler(Messenger sender, ConversationEventArgs e);


        public delegate void ErrorReceivedHandler(Messenger sender, MSNErrorEventArgs e);


        public delegate void ListReceivedHandler(Messenger sender, ListReceivedEventArgs e);


        public delegate void MailboxStatusHandler(Messenger sender, MailboxStatusEventArgs e);


        public delegate void MessageReceivedHandler(Messenger sender, string Message);


        public delegate void OnConnectionClosed(Messenger sender, EventArgs e);


        public delegate void OnConnectionEstablished(Messenger sender, EventArgs e);


        public delegate void ReverseAddedHandler(Messenger sender, ContactEventArgs e);


        public delegate void ReverseRemovedHandler(Messenger sender, ContactEventArgs e);


        public delegate void SynchronizationCompletedHandler(Messenger sender, EventArgs e);

    }}

