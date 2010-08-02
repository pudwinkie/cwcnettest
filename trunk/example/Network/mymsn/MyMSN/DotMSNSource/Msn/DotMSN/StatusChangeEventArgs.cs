namespace DotMSN
{
    using System;

    public class StatusChangeEventArgs : EventArgs
    {
        // Methods
        public StatusChangeEventArgs(MSNStatus oldStatus)
        {
            this.OldStatus = oldStatus;
        }


        // Fields
        public MSNStatus OldStatus;
    }}

