namespace DotMSN
{
    using System;

    public class ContactGroupEventArgs : EventArgs
    {
        // Methods
        public ContactGroupEventArgs(ContactGroup contactGroup)
        {
            this.ContactGroup = contactGroup;
        }


        // Fields
        public ContactGroup ContactGroup;
    }}

