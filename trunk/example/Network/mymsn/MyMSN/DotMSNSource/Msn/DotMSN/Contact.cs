namespace DotMSN
{
    using System;
    using System.Runtime.CompilerServices;

    public class Contact
    {
        // Events
        public event ContactBlockedHandler ContactBlocked;
        public event DotMSN.Contact.ContactGroupChangedHandler ContactGroupChanged;
        public event DotMSN.Contact.ContactOfflineHandler ContactOffline;
        public event DotMSN.Contact.ContactOnlineHandler ContactOnline;
        public event ContactUnBlockedHandler ContactUnBlocked;
        public event ScreenNameChangedHandler ScreenNameChanged;
        public event StatusChangedHandler StatusChanged;

        // Methods
        public Contact()
        {
            this.status = MSNStatus.Offline;
            this.updateVersion = 0;
            this.inList = false;
        }

        internal void AddToList(MSNList list)
        {
            if ((list == MSNList.BlockedList) && !this.Blocked)
            {
                this.lists |= MSNList.BlockedList;
                this.ContactBlocked.Invoke(this, new EventArgs());
                return;
            }
            this.lists |= list;
        }

        public override int GetHashCode()
        {
            return this.mail.GetHashCode();
        }

        public void RemoveFromList()
        {
            if (this.messenger != null)
            {
                this.messenger.RemoveContact(this);
            }
        }

        internal void RemoveFromList(MSNList list)
        {
            if ((list == MSNList.BlockedList) && this.Blocked)
            {
                this.lists ^= MSNList.BlockedList;
                this.ContactUnBlocked.Invoke(this, new EventArgs());
                return;
            }
            this.lists ^= list;
        }

        internal void SetContactGroup(int newGroup)
        {
            if (this.contactGroup != newGroup)
            {
                this.contactGroup = newGroup;
                if (this.ContactGroupChanged != null)
                {
                    this.ContactGroupChanged.Invoke(this, new EventArgs());
                }
            }
        }

        internal void SetName(string newName)
        {
            if (this.name != newName)
            {
                this.name = HttpUtility.UrlDecode(newName);
                if (this.ScreenNameChanged != null)
                {
                    this.ScreenNameChanged.Invoke(this, new EventArgs());
                }
            }
        }

        internal void SetStatus(MSNStatus newStatus)
        {
            if (this.status == newStatus)
            {
                return;
            }
            MSNStatus status1 = this.status;
            this.status = newStatus;
            if (this.StatusChanged != null)
            {
                this.StatusChanged.Invoke(this, new StatusChangeEventArgs(status1));
            }
            if ((status1 == MSNStatus.Offline) && (this.ContactOnline != null))
            {
                this.ContactOnline.Invoke(this, new EventArgs());
            }
            if ((newStatus == MSNStatus.Offline) && (this.ContactOffline != null))
            {
                this.ContactOffline.Invoke(this, new StatusChangeEventArgs(status1));
            }
        }

        public void UpdateScreenName()
        {
            if (this.messenger != null)
            {
                this.messenger.RequestScreenName(this);
                return;
            }
            throw new MSNException("No valid messenger object");
        }


        // Properties
        public bool Blocked
        {
            get
            {
                return ((this.lists & MSNList.BlockedList) > ((MSNList) 0));
            }
            set
            {
                if (this.messenger == null)
                {
                    return;
                }
                if (value)
                {
                    this.messenger.BlockContact(this);
                    return;
                }
                this.messenger.UnBlockContact(this);
            }
        }

        public ContactGroup ContactGroup
        {
            get
            {
                if (this.messenger != null)
                {
                    return ((ContactGroup) this.messenger.ContactGroups[this.contactGroup]);
                }
                return null;
            }
            set
            {
                if (this.messenger != null)
                {
                    this.messenger.ChangeGroup(this, value);
                }
            }
        }

        public string HomePhone
        {
            get
            {
                return this.homePhone;
            }
        }

        public string Mail
        {
            get
            {
                return this.mail;
            }
        }

        public string MobilePager
        {
            get
            {
                return this.mobilePager;
            }
        }

        public string MobilePhone
        {
            get
            {
                return this.mobilePhone;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public bool OnAllowedList
        {
            get
            {
                return ((this.lists & MSNList.AllowedList) > ((MSNList) 0));
            }
            set
            {
                if (value == this.OnAllowedList)
                {
                    return;
                }
                if (value)
                {
                    this.messenger.AddToList(this, MSNList.AllowedList);
                    return;
                }
                this.messenger.RemoveFromList(this, MSNList.AllowedList);
            }
        }

        public bool OnBlockedList
        {
            get
            {
                return ((this.lists & MSNList.BlockedList) > ((MSNList) 0));
            }
            set
            {
                if (value == this.OnBlockedList)
                {
                    return;
                }
                if (value)
                {
                    this.messenger.AddToList(this, MSNList.BlockedList);
                    return;
                }
                this.messenger.RemoveFromList(this, MSNList.BlockedList);
            }
        }

        public bool OnForwardList
        {
            get
            {
                return ((this.lists & MSNList.ForwardList) > ((MSNList) 0));
            }
            set
            {
                if (value == this.OnForwardList)
                {
                    return;
                }
                if (value)
                {
                    this.messenger.AddToList(this, MSNList.ForwardList);
                    return;
                }
                this.messenger.RemoveFromList(this, MSNList.ForwardList);
            }
        }

        public bool OnReverseList
        {
            get
            {
                return ((this.lists & MSNList.ReverseList) > ((MSNList) 0));
            }
        }

        public MSNStatus Status
        {
            get
            {
                return this.status;
            }
        }

        public string WorkPhone
        {
            get
            {
                return this.workPhone;
            }
        }


        // Fields
        public object ClientData;
        internal int contactGroup;
        internal string homePhone;
        public bool inList;
        internal MSNList lists;
        internal string mail;
        internal Messenger messenger;
        internal string mobilePager;
        internal string mobilePhone;
        internal string name;
        internal MSNStatus status;
        internal int updateVersion;
        internal string workPhone;

        // Nested Types
        public delegate void ContactBlockedHandler(Contact sender, EventArgs e);


        public delegate void ContactGroupChangedHandler(Contact sender, EventArgs e);


        public delegate void ContactOfflineHandler(Contact sender, StatusChangeEventArgs e);


        public delegate void ContactOnlineHandler(Contact sender, EventArgs e);


        public delegate void ContactUnBlockedHandler(Contact sender, EventArgs e);


        public delegate void ScreenNameChangedHandler(Contact sender, EventArgs e);


        public delegate void StatusChangedHandler(Contact sender, StatusChangeEventArgs e);

    }}

