namespace DotMSN
{
    using System;

    internal class ConversationQueueItem
    {
        // Methods
        public ConversationQueueItem(string mail, object clientData)
        {
            this.Mail = mail;
            this.ClientData = clientData;
        }


        // Fields
        public object ClientData;
        public string Mail;
    }}

