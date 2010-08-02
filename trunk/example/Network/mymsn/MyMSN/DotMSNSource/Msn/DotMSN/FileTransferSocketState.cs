namespace DotMSN
{
    using System;
    using System.Net.Sockets;

    internal class FileTransferSocketState
    {
        // Methods
        public FileTransferSocketState(Socket activeSocket, FileTransferSocketPair socketPair)
        {
            this.FileTransfer = null;
            this.FileTransferSocketPair = socketPair;
            this.ActiveSocket = activeSocket;
        }

        public FileTransferSocketState(FileTransfer fileTransfer, Socket activeSocket, FileTransferSocketPair socketPair)
        {
            this.FileTransfer = null;
            this.FileTransfer = fileTransfer;
            this.FileTransferSocketPair = socketPair;
            this.ActiveSocket = activeSocket;
        }


        // Fields
        public Socket ActiveSocket;
        public FileTransfer FileTransfer;
        public FileTransferSocketPair FileTransferSocketPair;
    }}

