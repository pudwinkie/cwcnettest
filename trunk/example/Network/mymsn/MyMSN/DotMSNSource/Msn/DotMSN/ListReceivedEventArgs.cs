namespace DotMSN
{
    using System;

    public class ListReceivedEventArgs : EventArgs
    {
        // Methods
        public ListReceivedEventArgs(MSNList affectedList)
        {
            this.AffectedList = affectedList;
        }


        // Fields
        public MSNList AffectedList;
    }}

