namespace cwc
{

    public class OpenWith
    {
		private const string MenuName = "Folder\\shell\\NewMenuOption";
		private const string Command = "Folder\\shell\\NewMenuOption\\command";

		public void AddToFolder(String AppName, String AppPath)
		{
			RegistryKey regmenu = null;
			RegistryKey regcmd = null;
			try
			{
				regmenu = Registry.ClassesRoot.CreateSubKey(MenuName);
				if(regmenu != null)
					regmenu.SetValue("", AppName);
				regcmd = Registry.ClassesRoot.CreateSubKey(Command);
				if(regcmd != null)
						regcmd.SetValue("", AppPath);
			}
			finally       
			{
				if(regmenu != null)
					regmenu.Close();
				if(regcmd != null)
					regcmd.Close();
			}        
		}

		public void RemoveToFolder()
		{
				RegistryKey reg = Registry.ClassesRoot.OpenSubKey(Command);
				if(reg != null)
				{
					reg.Close();
					Registry.ClassesRoot.DeleteSubKey(Command);
				}
				reg = Registry.ClassesRoot.OpenSubKey(MenuName);
				if(reg != null)
				{
					reg.Close();
					Registry.ClassesRoot.DeleteSubKey(MenuName);
				}

		}

		public void AddToAllFile(String AppName, String AppPath){
			RegistryKey shell = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
			RegistryKey custom = shell.CreateSubKey(AppName);
			RegistryKey cmd = custom.CreateSubKey("command");
			cmd.SetValue("", AppPath + " %1");
			//Application.ExecutablePath �O���{�Ǧۨ������|
			//%1 �O�ǤJ���}�������|
			cmd.Close();
			custom.Close();
			shell.Close();
		}


    }
}

