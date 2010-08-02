namespace DotMSN
{
    using System;

    public class ListMutateEventArgs : EventArgs
    {
        // Methods
        public ListMutateEventArgs(Contact contact, MSNList affectedList)
        {
            this.Subject = contact;
            this.AffectedList = affectedList;
        }


        // Fields
        public MSNList AffectedList;
        public Contact Subject;
    }}

