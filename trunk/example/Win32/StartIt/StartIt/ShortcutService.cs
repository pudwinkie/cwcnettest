namespace StartIt
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class ShortcutService : IDisposable
    {
        private List<Shortcut> _apps;
        private VoidVoidDelegate _beginGetApps;
        private DirectoryInfo _myAppDir;
        private DirectoryInfo _sharedAppDir;

        public event EventHandler GetAppsFinish;

        public ShortcutService()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            this._myAppDir = new DirectoryInfo(folderPath);
            if (Directory.Exists(folderPath.Replace(Environment.UserName, "All Users.WINDOWS")))
            {
                this._sharedAppDir = new DirectoryInfo(folderPath.Replace(Environment.UserName, "All Users.WINDOWS"));
            }
            else
            {
                this._sharedAppDir = new DirectoryInfo(folderPath.Replace(Environment.UserName, "All Users"));
            }
            this._apps = new List<Shortcut>();
        }

        public void BeginGetApps()
        {
            if (this._beginGetApps == null)
            {
                this._beginGetApps = new VoidVoidDelegate(this.GetApps);
            }
            this._beginGetApps.BeginInvoke(null, null);
        }

        public void Dispose()
        {
        }

        public void GetApps()
        {
            this._apps.Clear();
            this.GetApps(this._myAppDir);
            this.GetApps(this._sharedAppDir);
            if (this.GetAppsFinish != null)
            {
                this.GetAppsFinish(this, null);
            }
        }

        private void GetApps(DirectoryInfo folder)
        {
            Shortcut item = null;
            foreach (FileInfo info in folder.GetFiles("*.lnk"))
            {
                item = new Shortcut(info.FullName);
                if (item.Exist)
                {
                    this._apps.Add(item);
                }
            }
            foreach (DirectoryInfo info2 in folder.GetDirectories())
            {
                this.GetApps(info2);
            }
        }

        public List<Shortcut> AppLinks
        {
            get
            {
                return this._apps;
            }
        }

        private delegate void VoidVoidDelegate();
    }
}

