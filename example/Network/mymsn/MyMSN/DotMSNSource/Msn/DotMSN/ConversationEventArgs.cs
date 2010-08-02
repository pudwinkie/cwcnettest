namespace DotMSN
{
    using System;

    public class ConversationEventArgs : EventArgs
    {
        // Methods
        public ConversationEventArgs(Conversation conversation)
        {
            this.Conversation = conversation;
        }


        // Fields
        public Conversation Conversation;
    }}

