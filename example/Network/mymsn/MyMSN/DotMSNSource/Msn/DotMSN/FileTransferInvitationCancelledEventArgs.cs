namespace DotMSN
{
    using System;

    public class FileTransferInvitationCancelledEventArgs : EventArgs
    {
        // Methods
        public FileTransferInvitationCancelledEventArgs(MSNInvitationCancelCode cancelCode)
        {
            this.CancelCode = cancelCode;
        }


        // Fields
        public MSNInvitationCancelCode CancelCode;
    }}

