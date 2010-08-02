namespace DotMSN
{
    using System;

    public class FileTransferCancelledEventArgs : EventArgs
    {
        // Methods
        public FileTransferCancelledEventArgs(MSNFileTransferCancelCode cancelCode)
        {
            this.CancelCode = cancelCode;
        }


        // Fields
        public MSNFileTransferCancelCode CancelCode;
    }}

