namespace DotMSN
{
    using System;

    public class Connection
    {
        // Methods
        public Connection(string pHost, int pPort)
        {
            this.host = pHost;
            this.port = pPort;
        }


        // Properties
        public string Host
        {
            get
            {
                return this.host;
            }
            set
            {
                this.host = value;
            }
        }

        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }


        // Fields
        private string host;
        private int port;
    }}

