namespace DotMSN
{
    using System;

    public class NewMailReceivedEventArgs : EventArgs
    {
        // Methods
        public NewMailReceivedEventArgs(string from, string messageURL, string subject, string destinationFolder, string fromEmail, int id)
        {
            this.From = from;
            this.MessageURL = messageURL;
            this.Subject = subject;
            this.DestinationFolder = destinationFolder;
            this.FromEmail = fromEmail;
            this.ID = id;
        }


        // Fields
        public string DestinationFolder;
        public string From;
        public string FromEmail;
        public int ID;
        public string MessageURL;
        public string Subject;
    }}

