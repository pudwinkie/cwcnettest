namespace DotMSN
{
    using System;

    public class Owner : Contact
    {
        // Methods
        public Owner(string pMail, string pPassword)
        {
            this.privacy = MSNPrivacy.Unknown;
            this.notifyPrivacy = MSNNotifyPrivacy.Unknown;
            this.mail = pMail;
            this.password = pPassword;
        }


        // Properties
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
                    this.messenger.ChangeScreenName(value);
                }
            }
        }

        public MSNNotifyPrivacy NotifyPrivacy
        {
            get
            {
                return this.notifyPrivacy;
            }
            set
            {
                if (this.messenger != null)
                {
                    this.messenger.SetNotifyPrivacy(value);
                }
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        public MSNPrivacy Privacy
        {
            get
            {
                return this.privacy;
            }
            set
            {
                if (this.messenger != null)
                {
                    this.messenger.SetPrivacy(value);
                }
            }
        }

        public MSNStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (this.messenger != null)
                {
                    this.messenger.SetStatus(value);
                }
            }
        }


        // Fields
        internal MSNNotifyPrivacy notifyPrivacy;
        private string password;
        internal MSNPrivacy privacy;
    }}

