namespace DotMSN
{
    using System;
    using System.Net.Sockets;

    public class ConnectionErrorEventArgs : EventArgs
    {
        // Methods
        public ConnectionErrorEventArgs(SocketException error)
        {
            this.Error = error;
        }


        // Fields
        public SocketException Error;
    }}

