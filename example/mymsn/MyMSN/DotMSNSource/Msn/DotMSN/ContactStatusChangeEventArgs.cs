namespace DotMSN
{
    using System;

    public class ContactStatusChangeEventArgs : EventArgs
    {
        // Methods
        public ContactStatusChangeEventArgs(Contact contact, MSNStatus oldStatus)
        {
            this.Contact = contact;
            this.OldStatus = oldStatus;
        }


        // Fields
        public Contact Contact;
        public MSNStatus OldStatus;
    }}

