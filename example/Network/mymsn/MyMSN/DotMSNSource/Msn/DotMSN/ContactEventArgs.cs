namespace DotMSN
{
    using System;

    public class ContactEventArgs : EventArgs
    {
        // Methods
        public ContactEventArgs(Contact contact)
        {
            this.Contact = contact;
        }


        // Fields
        public Contact Contact;
    }}

