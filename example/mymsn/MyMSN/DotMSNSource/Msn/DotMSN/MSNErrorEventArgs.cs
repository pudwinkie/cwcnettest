namespace DotMSN
{
    using System;

    public class MSNErrorEventArgs : EventArgs
    {
        // Methods
        public MSNErrorEventArgs(MSNError error)
        {
            this.Error = error;
        }


        // Fields
        public MSNError Error;
    }}

