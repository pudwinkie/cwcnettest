namespace StartIt
{
    using IWshRuntimeLibrary;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;

    public class Shortcut
    {
        private string _appFileName;
        private string _appPath;
        private string _arguments;
        private bool _exist;
        private Icon _icon;
        private string _shortcutPath;
        private string _shortcutText;
        private static Icon DEFAULT_ICON = Resource.App;

        public Shortcut()
        {
            this._icon = DEFAULT_ICON;
        }

        public Shortcut(string shortcutPath)
        {
            this._shortcutPath = shortcutPath;
            this._shortcutText = Path.GetFileNameWithoutExtension(this._shortcutPath);
            WshShellClass class2 = new WshShellClass();
            IWshShortcut shortcut = (IWshShortcut) class2.CreateShortcut(shortcutPath);
            this._appPath = shortcut.get_TargetPath();
            this._exist = File.Exists(shortcut.get_TargetPath());
            this._arguments = shortcut.get_Arguments();
            this._appFileName = Path.GetFileNameWithoutExtension(shortcut.get_TargetPath());
            this._icon = this.GetIcon(this._appPath);
        }

        private Icon GetIcon(string appPath)
        {
            NativeMethod.SHFILEINFO psfi = new NativeMethod.SHFILEINFO();
            NativeMethod.SHGetFileInfo(appPath, 0, ref psfi, (uint) Marshal.SizeOf(psfi), 0x101);
            if (psfi.hIcon.ToInt32() != 0)
            {
                return Icon.FromHandle(psfi.hIcon);
            }
            return DEFAULT_ICON;
        }

        public override string ToString()
        {
            return this._shortcutText;
        }

        public string AppFileName
        {
            get
            {
                return this._appFileName;
            }
        }

        public Icon AppIcon
        {
            get
            {
                return this._icon;
            }
        }

        public string AppPath
        {
            get
            {
                return this._appPath;
            }
            set
            {
                this._appPath = value;
            }
        }

        public string Arguments
        {
            get
            {
                return this._arguments;
            }
        }

        public bool Exist
        {
            get
            {
                return this._exist;
            }
        }

        public string ShortcutPath
        {
            get
            {
                return this._shortcutPath;
            }
        }

        public string ShortcutText
        {
            get
            {
                return this._shortcutText;
            }
            set
            {
                this._shortcutPath = value;
            }
        }
    }
}

