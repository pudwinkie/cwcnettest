namespace DotMSN
{
    using System;

    public class FileTransferEventArgs : EventArgs
    {
        // Methods
        public FileTransferEventArgs(FileTransfer fileTransfer)
        {
            this.FileTransfer = fileTransfer;
        }


        // Fields
        public FileTransfer FileTransfer;
    }}

