using System;
using System.Collections.Generic;
using System.Text;
using MSNPSharp;
using MSNPSharp.Core;
using System.Diagnostics;

namespace MSNPtest {
    class Program {
        private static  MyMessenger msn;
        static void Main(string[] args) {
            //var messenger = new Messenger();
            //messenger.Credentials.ClientID = "PROD0119GSJUC$18";
            //messenger.Credentials.ClientCode = "ILTXC!4IXB5FB*PX";


            //messenger.Nameserver.SignedIn += new EventHandler<EventArgs>(Nameserver_SignedIn);
            //messenger.Nameserver.AuthenticationError += new EventHandler<ExceptionEventArgs>(Nameserver_AuthenticationError);
            //messenger.Nameserver.ExceptionOccurred += new EventHandler<ExceptionEventArgs>(Nameserver_ExceptionOccurred);

            //messenger.NameserverProcessor.ConnectionEstablished += new EventHandler<EventArgs>(NameserverProcessor_ConnectionEstablished);
            //messenger.NameserverProcessor.ConnectingException += new EventHandler<ExceptionEventArgs>(NameserverProcessor_ConnectingException);
            //messenger.NameserverProcessor.ConnectionException += new EventHandler<ExceptionEventArgs>(NameserverProcessor_ConnectionException);

            //messenger.Connect();
            msn = new MyMessenger();
            msn.LoginEvent += new EventHandler<LoginEventArgs>(msn_LoginEvent);
            msn.Login("", "");
            Console.ReadLine();            
            //messenger.Disconnect();
        }

        static void msn_LoginEvent(object sender, LoginEventArgs e) {
            //msn.AddContactGroup(Guid.NewGuid().ToString());
            //msn.ListContacts();

            msn.SendTextMessage("cwchiu@hotmail.com", "hello world");

        }
    }

    class LoginEventArgs : EventArgs {
    }

    class MyMessenger :IDisposable{
        private bool disposed = false;
        public Messenger messenger;
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public void Login(String id, String password) {
            messenger = new Messenger();
            messenger.Credentials.ClientID = "PROD0119GSJUC$18";
            messenger.Credentials.ClientCode = "ILTXC!4IXB5FB*PX";

            messenger.Credentials.Account = "";
            messenger.Credentials.Password = "";

            messenger.Nameserver.SignedIn += new EventHandler<EventArgs>(Nameserver_SignedIn);
            messenger.Nameserver.AuthenticationError += new EventHandler<ExceptionEventArgs>(Nameserver_AuthenticationError);
            messenger.Nameserver.ExceptionOccurred += new EventHandler<ExceptionEventArgs>(Nameserver_ExceptionOccurred);
            messenger.Nameserver.SignedOff += new EventHandler<SignedOffEventArgs>(Nameserver_SignedOff);
            messenger.NameserverProcessor.ConnectionEstablished += new EventHandler<EventArgs>(NameserverProcessor_ConnectionEstablished);
            messenger.NameserverProcessor.ConnectingException += new EventHandler<ExceptionEventArgs>(NameserverProcessor_ConnectingException);
            messenger.NameserverProcessor.ConnectionException += new EventHandler<ExceptionEventArgs>(NameserverProcessor_ConnectionException);
            
            //messenger.ConversationCreated += new EventHandler<ConversationCreatedEventArgs>(messenger_ConversationCreated);
            messenger.OIMService.OIMReceived += new EventHandler<OIMReceivedEventArgs>(OIMService_OIMReceived);

            messenger.Connect();
        }

        void OIMService_OIMReceived(object sender, OIMReceivedEventArgs e) {
            throw new NotImplementedException();
        }

        void messenger_ConversationCreated(object sender, ConversationCreatedEventArgs e) {
            throw new NotImplementedException();
        }

        private void Nameserver_SignedOff(object sender, SignedOffEventArgs e) {
            OnLogout(sender, e);
        }

        /// <summary>
        /// 登出
        /// </summary>
        public void Logout() {
            try {
                if (messenger.Connected /*&& messenger.Nameserver.IsSignedIn*/) {
                    messenger.Disconnect();
                }
            } catch(MSNPSharpException) {
            }

            messenger.Nameserver.SignedIn -= new EventHandler<EventArgs>(Nameserver_SignedIn);
            messenger.Nameserver.AuthenticationError -= new EventHandler<ExceptionEventArgs>(Nameserver_AuthenticationError);
            messenger.Nameserver.ExceptionOccurred -= new EventHandler<ExceptionEventArgs>(Nameserver_ExceptionOccurred);
            messenger.Nameserver.SignedOff -= new EventHandler<SignedOffEventArgs>(Nameserver_SignedOff); 
            messenger.NameserverProcessor.ConnectionEstablished -= new EventHandler<EventArgs>(NameserverProcessor_ConnectionEstablished);
            messenger.NameserverProcessor.ConnectingException -= new EventHandler<ExceptionEventArgs>(NameserverProcessor_ConnectingException);            

            messenger = null;
        }

        public void ListContacts() {
            //var cg = messenger.ContactGroups;
            //cg.Remove(cg.GetByName("659517c2-dea1-4838-9899-ddf9045dac0f"));

            //foreach (ContactGroup c in cg) {
                //Console.WriteLine(c.Name);
                
                //c.NSMessageHandler.ContactService.RemoveContact("");

            //}
        }

        private Conversation conversation;
        public void SendTextMessage(string contactEmail, string text) {
            conversation = messenger.CreateConversation();

            conversation.Switchboard.SessionClosed += new EventHandler<EventArgs>(Switchboard_SessionClosed);
            conversation.Switchboard.ContactJoined += new EventHandler<ContactEventArgs>(Switchboard_ContactJoined);
            //conversation.Switchboard.ContactLeft += new ContactChangedEventHandler(Switchboard_ContactLeft);

            conversation.Switchboard.EmoticonDefinitionReceived += new EventHandler<EmoticonDefinitionEventArgs>(Switchboard_EmoticonDefinitionReceived);
            conversation.Switchboard.NudgeReceived += new EventHandler<ContactEventArgs>(Switchboard_NudgeReceived);
            conversation.Switchboard.TextMessageReceived += new EventHandler<TextMessageEventArgs>(Switchboard_TextMessageReceived);
            // 加入訊息佇列
            msgQueue.Enqueue(contactEmail + "=" + text);

            conversation.Invite(contactEmail, ClientType.PassportMember);
        }

        // TODO: fail
        void Switchboard_EmoticonDefinitionReceived(object sender, EmoticonDefinitionEventArgs e) {

            //Console.WriteLine("Switchboard_EmoticonDefinitionReceived");
        }

        void Switchboard_TextMessageReceived(object sender, TextMessageEventArgs e) {


            Console.WriteLine("Switchboard_TextMessageReceived: " + e.Sender.Name + ":" + e.Message.Text);
        }

        void Switchboard_NudgeReceived(object sender, ContactEventArgs e) {
            Console.WriteLine("Switchboard_NudgeReceived");
        }


        private void Switchboard_SessionClosed(object sender, EventArgs e) {
        }

        private void Switchboard_ContactJoined(object sender, ContactEventArgs e) {
            InternalSendTextMessage(e.Contact);
        }
        private Queue<string> msgQueue = new Queue<string>();	// 欲傳送的訊息佇列.
        private void InternalSendTextMessage(Contact contact) {
            //SetStatus("傳送訊息給 " + contact.Name);
            string targetMsg;
            string email;
            int i;

            while (msgQueue.Count > 0) {
                targetMsg = msgQueue.Dequeue();
                string[] msg = targetMsg.Split('=');
                //i = targetMsg.IndexOf('=');
                //if (i >= 0) {
                if (msg.Length == 2){
                    //email = targetMsg.Substring(0, i).ToLower();
                    email = msg[0].ToLower();
                    if (email.Equals(contact.Mail.ToLower()))	// 比對是否為訊息目標
					{
                        //txtMsg.Text = targetMsg.Substring(i + 1);
                        //conversation.Switchboard.SendTextMessage(txtMsg);
                        conversation.Switchboard.SendTextMessage(new TextMessage(msg[1]));
                    }
                }
            }
        }

        public void AddContactGroup(String name) {
            messenger.ContactGroups.Add(name);            
        }

        private void NameserverProcessor_ConnectionEstablished(object sender, EventArgs e) {
            Debug.WriteLine("NameserverProcessor_ConnectionEstablished: ");
        }

        private void NameserverProcessor_ConnectingException(object sender, ExceptionEventArgs e) {
            Debug.WriteLine("NameserverProcessor_ConnectingException: " + e.Exception.Message);
        }

        private void NameserverProcessor_ConnectionException(object sender, ExceptionEventArgs e) {
            Debug.WriteLine("NameserverProcessor_ConnectionException: " + e.Exception.Message);
        }

        private void Nameserver_ExceptionOccurred(object sender, ExceptionEventArgs e) {
            Debug.WriteLine("Nameserver_ExceptionOccurred: " + e.Exception.Message);
        }

        private void Nameserver_AuthenticationError(object sender, ExceptionEventArgs e) {
            Debug.WriteLine("Nameserver_AuthenticationError: " + e.Exception.Message);
        }

        private void Nameserver_SignedIn(object sender, EventArgs e) {
            Debug.WriteLine("Nameserver_SignedIn: ");

            //var conv = messenger.CreateConversation();

            OnLogin(sender, new LoginEventArgs());
        }

        public event EventHandler<LoginEventArgs> LoginEvent;
        protected void OnLogin(Object sender, LoginEventArgs e) {
            if (LoginEvent != null) {
                LoginEvent(sender, e);
            }
        }

        public event EventHandler LogoutEvent;
        public void OnLogout(Object sender, EventArgs e) {
            if (LogoutEvent != null) {
                LogoutEvent(sender, e);
            }
        }


        #region IDisposable
        public void Dispose() {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            // Check to see if Dispose has already been called.
            if (!this.disposed) {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing) {
                    // Dispose managed resources.
                    //component.Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                if (messenger != null) {
                    Logout();
                }

                // Note disposing has been done.
                disposed = true;
            }
        }

        ~MyMessenger() {
            Dispose(false);
        }

        #endregion

    }
}
