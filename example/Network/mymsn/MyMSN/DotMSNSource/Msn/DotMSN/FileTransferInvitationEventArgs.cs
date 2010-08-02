namespace DotMSN
{
    using System;

    public class FileTransferInvitationEventArgs : EventArgs
    {
        // Methods
        public FileTransferInvitationEventArgs(FileTransfer fileTransfer)
        {
            this.Accept = true;
            this.FileTransfer = fileTransfer;
        }


        // Fields
        public bool Accept;
        public FileTransfer FileTransfer;
    }}

