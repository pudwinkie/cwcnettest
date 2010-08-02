namespace DotMSN
{
    using System;

    public class MSNException : Exception
    {
        // Methods
        public MSNException(string message) : base(message)
        {
            this.errorID = -1;
        }

        public MSNException(string message, Exception innerException) : base(message, innerException)
        {
            this.errorID = -1;
        }

        public MSNException(string message, int ID) : base(message)
        {
            this.errorID = -1;
            this.errorID = ID;
        }


        // Fields
        private int errorID;
    }}

