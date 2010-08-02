namespace DotMSN
{
    using System;

    public class ContactGroup
    {
        // Methods
        internal ContactGroup(string name, int id, Messenger messenger)
        {
            this.name = name;
            this.id = id;
            this.messenger = messenger;
        }

        public override int GetHashCode()
        {
            return this.id;
        }


        // Properties
        public int ID
        {
            get
            {
                return this.id;
            }
        }

        public Messenger Messenger
        {
            get
            {
                return this.messenger;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.messenger != null)
                {
                    this.messenger.RenameGroup(this, value);
                }
            }
        }


        // Fields
        public object ClientData;
        private int id;
        internal Messenger messenger;
        private string name;
    }}

