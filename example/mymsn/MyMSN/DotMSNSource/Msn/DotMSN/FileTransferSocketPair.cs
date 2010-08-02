namespace DotMSN
{
    using System;
    using System.Net.Sockets;

    internal class FileTransferSocketPair
    {
        // Methods
        public FileTransferSocketPair(Socket primary, Socket secondary)
        {
            this.Handled = false;
            this.PrimarySocket = primary;
            this.SecondarySocket = secondary;
        }


        // Fields
        public bool Handled;
        public Socket PrimarySocket;
        public Socket SecondarySocket;
    }}

