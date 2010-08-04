using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using Microsoft.Win32;

namespace ChuiWenChiu.Win32 {
	/// <summary>
	/// ApplicationData is a generic utility library class for managing an application's data.
	/// </summary>
	/// <remarks>
	/// According to the "Designed for Windows XP spec v2.3" you should 
	/// "Classify application data into the following categories:
	///   ?Per user, roaming
	///   ?Per user, non-roaming
	///   ?Per computer (non-user specific and non-roaming)"
	/// NOTE: The three categories are named here as: User, LocalUser and Common respectively.
	/// 
	/// Knowing where to store files and registry settings for each of this categories can be
	/// complicated and require repeating code. This utility class tries to simplify this
	/// process.
	/// 
	/// Furthermore, the spec requires that application data should be stored under:
	/// [company name]\[product name]\[version]
	/// The required information is extract from the assembly attributes 
	/// (e.g. AssemblyCompanyAttribute), therefore it is <b>highly recommended</b> to fill the 
	/// approriate attributes (typically in AssemblyInfo.cs) when using this utility.
	/// Example locations:
	/// C:\Documents and Settings\All Users\Application Data\My Company\My Product\1.0.840.34747\Sub1\Sub2\My File.txt
	/// "HKEY_CURRENT_USER\Software\My Company\My Product\1.0.840.34747\Local\Sub1\Sub1"
	/// </remarks>
	/// <example>
	/// Working with files:
	/// 
	/// FileInfo file = ApplicationData.LocalUser["Sub1"]["Sub2"].GetFile("mimi.txt");
	/// StreamWriter writer = file.CreateText();
	/// writer.WriteLine("This is a test of ApplicationData!");
	/// writer.Close();
	/// 
	/// Working with the registry:
	/// 
	/// RegistryKey key = ApplicationData.LocalUser["Sub1"]["Sub2"].GetRegistryKey();
	/// key.SetValue("Test", "This is a test!");
	/// </example>
	/// <author>Michael Mumcuoglu (xmichaelm@hotmail.com)</author>
	public class ApplicationData {
#region Constructors
		/// <summary>
		/// Private ctor. Use one of: Common, LocalUser or User to get an instance
		/// </summary>
		/// <param name="type">One of AppDataType</param>
		/// <param name="sub">The sub section</param>
		private ApplicationData(AppDataType type, string sub) {
			this.type = type;
			this.sub = sub;
		}

		private ApplicationData(AppDataType type) : this(type, null) {}

		private AppDataType type;
		private string sub;
#endregion Constructors

		private enum AppDataType {
			Common, LocalUser, User
		}

#region Static Accessors
		/// <summary>
		/// Returns an instance of ApplicationData for common application data.
		/// </summary>
		public static ApplicationData Common {
			get {
				return GetAppDataByType(AppDataType.Common);
			}
		}

		/// <summary>
		/// Returns an instance of ApplicationData for user-specific and computer-specific
		/// application data.
		/// </summary>
		public static ApplicationData LocalUser {
			get {
				return GetAppDataByType(AppDataType.LocalUser);
			}
		}

		/// <summary>
		/// Returns an instance of ApplicationData for user-specific roaming applicaiton data.
		/// </summary>
		public static ApplicationData User {
			get {
				return GetAppDataByType(AppDataType.User);
			}
		}

		/// <summary>
		/// Private method to retrieve and, if necessary, create the instance of the root
		/// ApplicationData for each type.
		/// The reference is saved in a WeakReference which allows for the best of both worlds:
		/// 1. If the reference is requested frequently in subsequent calls it will stay alive
		/// and be returned immidiately.
		/// 2. If the reference is no longer needed it will be freed by the GC.
		/// </summary>
		private static ApplicationData GetAppDataByType(AppDataType type) {
			if (type2ref.Contains(type)) {
				WeakReference appDataRef = (WeakReference) type2ref[type];
				ApplicationData appData = (ApplicationData) appDataRef.Target;
				
				if (appData == null) {
					// Recreate if dead
					appData = new ApplicationData(type);
					appDataRef.Target = appData;
				}

				return appData;
			}
	
			else {
				// First time
				ApplicationData appData = new ApplicationData(type);
				type2ref[type] = new WeakReference(appData);
				return appData;
			}
		}

		private static IDictionary type2ref = new Hashtable();
#endregion

#region Hierarchy
		/// <summary>
		/// Returns an ApplicationData for managing application data in the given sub section.
		/// </summary>
		public ApplicationData this[string sub] {
			get {
				if(subAppDataCollection.Contains(sub.ToLower())) {
					return (ApplicationData) subAppDataCollection[sub.ToLower()];
				}

				else {
					ApplicationData subAppData;
					if (this.sub != null) {
						subAppData = new ApplicationData(this.type, 
							string.Concat(this.sub, Path.DirectorySeparatorChar, sub));
					}

					else {
						subAppData = new ApplicationData(this.type, sub);
					}
					
					subAppDataCollection[sub.ToLower()] = subAppData;
					return subAppData;
				}
			}
		}

		IDictionary subAppDataCollection = new Hashtable();
#endregion Hierarchy

#region Files
		/// <summary>
		/// Returns the FileInfo of the file requested in the appropriate location.
		/// </summary>
		/// <param name="fileName">The name of the file</param>
		/// <returns>FileInfo for the requested file</returns>
		public FileInfo GetFile(string fileName) {
			VerifyDirectory();

			if (filesCollection.Contains(fileName.ToLower())) {
				FileInfo file = (FileInfo) filesCollection[fileName.ToLower()];
				file.Refresh();
				return file;
			}

			else {
				FileInfo file = new FileInfo(Path.Combine(directory.FullName, fileName));
				filesCollection[fileName.ToLower()] = file;
				return file;
			}
		}

		IDictionary filesCollection = new Hashtable();

		private void VerifyDirectory() {
			if (directory != null) {
				directory.Refresh();
				return;
			}

			else {
				string directoryPath = "";
				switch (type) {
					case AppDataType.Common:
						directoryPath = Application.CommonAppDataPath;
						break;

					case AppDataType.LocalUser:
						directoryPath = Application.LocalUserAppDataPath;
						break;

					case AppDataType.User:
						directoryPath = Application.UserAppDataPath;
						break;
				}

				if (sub != null) {
					directoryPath = Path.Combine(directoryPath, sub);
				}

				directory = new DirectoryInfo(directoryPath);
				VerifyDirectoryExists(directory);
			}
		}

		private DirectoryInfo directory;

		/// <summary>
		/// Recursive method to verify a directory exists.
		/// </summary>
		/// <param name="verifyDir">The directory to verify</param>
		private static void VerifyDirectoryExists(DirectoryInfo verifyDir) {
			if (verifyDir.Exists) {
				return;
			}

			else {
				VerifyDirectoryExists(verifyDir.Parent);
				verifyDir.Create();
			}
		}
#endregion Files

#region Registry
		/// <summary>
		/// Returns the RegistryKey for the appropriate location in the registry.
		/// </summary>
		/// <returns>The RegistryKey</returns>
		public RegistryKey GetRegistryKey() {
			VerifyRegistryKey();
            return registryKey;
		}

		private void VerifyRegistryKey() {
			if (registryKey != null) {
				return;
			}

			else {
				RegistryKey rootRegKey = null;
				switch (type) {
					case AppDataType.Common:
						rootRegKey = Application.CommonAppDataRegistry;
						break;

					case AppDataType.LocalUser:
						rootRegKey = Application.UserAppDataRegistry.CreateSubKey("Local");
						break;

					case AppDataType.User:
						rootRegKey = Application.UserAppDataRegistry;
						break;
				}

				if (sub != null) {
					registryKey = rootRegKey.CreateSubKey(sub);
				}

				else {
					registryKey = rootRegKey;
				}
			}
		}

		private RegistryKey registryKey;

#endregion Registry
	}
}
