namespace DotMSN
{
    using System;

    public class ContactChangeEventArgs : EventArgs
    {
        // Methods
        public ContactChangeEventArgs(Contact contact)
        {
            this.Contact = contact;
        }


        // Fields
        public Contact Contact;
    }}

