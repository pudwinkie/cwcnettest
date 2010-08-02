namespace DotMSN
{
    using System;

    public class MessageEventArgs : EventArgs
    {
        // Methods
        public MessageEventArgs(Message message, Contact sender)
        {
            this.Message = message;
            this.Sender = sender;
        }


        // Fields
        public Message Message;
        public Contact Sender;
    }}

