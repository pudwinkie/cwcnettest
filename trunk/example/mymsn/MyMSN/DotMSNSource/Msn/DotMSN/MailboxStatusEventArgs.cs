namespace DotMSN
{
    using System;

    public class MailboxStatusEventArgs : EventArgs
    {
        // Methods
        public MailboxStatusEventArgs(int inboxUnread, int foldersUnread, string inboxURL, string foldersURL, string postURL)
        {
            this.InboxUnread = inboxUnread;
            this.FoldersUnread = foldersUnread;
            this.InboxURL = inboxURL;
            this.FoldersURL = foldersURL;
            this.PostURL = postURL;
        }


        // Fields
        public int FoldersUnread;
        public string FoldersURL;
        public int InboxUnread;
        public string InboxURL;
        public string PostURL;
    }}

