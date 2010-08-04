
namespace chuiwenchiu.windows
{
	public sealed class Helper
	{
		[DllImport("user32.dll")]
		private static extern bool BlockInput(bool fBlockIt);

		[DllImport("rasapi32.dll", SetLastError=true)]
		private static extern uint RasGetEntryDialParams(
		   string lpszPhonebook,
		   [In, Out] ref RASDIALPARAMS lprasdialparams,
		   out bool lpfPassword);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("gdi32.dll")]
		private static extern int AddFontResource(string lpszFilename);

		[DllImport("gdi32.dll")]
		private static extern bool RemoveFontResource(string lpFileName);

		private const long HWND_Broadcast = 0xFFFF;// 廣播
		private const long WM_FontChange  = 0x1D; // 字型異動

		// 鎖住鍵盤滑鼠
		public static void LockKeyboardAndMouse(int wait_secs){
			if( wait_secs <= 0){
				return; 
			}

			BlockInput(true);
			System.Threading.Thread.Sleep(wait_secs);
			BlockInput(false);
		}

		// 列舉系統安裝的 ODBC Driver
		public static IEnumerable<String> GetODBCDrivers(){
			return Registry.LocalMachine.OpenSubKey(@"Software\ODBC\ODBCINST.INI").GetSubKeyNames();
		}

		public static String GetDefaulPPPOEName(){
			return Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\RAS AutoDial\Default").GetValue("DefaultInternet");
		}

		//ref: 
		//http://www.dotblogs.com.tw/PowerHammer/archive/2008/03/24/2263.aspx
		public static String GetPPPOEInfo(string pppoe_name){
			// 取得連線資訊
			// RasGetEntryDialParams
		}

		// 安裝系統字型
		//
		// 列舉已安裝的字型
		// (new InstalledFontCollection()).Families
		//
		// ref:
		// http://www.dotblogs.com.tw/PowerHammer/archive/2008/03/24/2247.aspx
		public static bool AddSystemFont(string fnt){
			// GetEnvironmentVariable 方法: 取得環境變數
			// WinDir : Windows 所在目錄, Windows\Fonts 為: 字型所在目錄
			// Path.GetFileName 方法: 要從其中取得檔案名稱和副檔名的路徑字串。
            var font_folder = Path.Combine(Environment.GetEnvironmentVariable("WinDir"), "Fonts");
            var font_target_file = Path.Combine(font_folder, Path.GetFileName(fnt));            
			try{
				File.Copy(fnt, font_target_file);

				// PrivateFontCollection 類別
				// 提供從字型檔案建立的字型系列集合，這些字型檔案是由用戶端應用程式所提供。
				// 命名空間：System.Drawing.Text
				// 組件：System.Drawing (在system.drawing.dll 中)
				// 初始化PrivateFontCollection 類別的新執行個體

				var PFC = new System.Drawing.Text.PrivateFontCollection();
				// PrivateFontCollection.AddFontFile 方法: 從指定的檔案將字型加入這個PrivateFontCollection。

				PFC.AddFontFile(fnt);
				SendMessage(HWND_Broadcast, WM_FontChange, 0, 0) // 廣播訊息字型異動
				return true;
			}catch(FileNotFoundException){
			}catch(IOException){

			}
			return false;
		}

		// 移除系統字型
		public static bool RemoveSystemFont(string fnt){
			// GetEnvironmentVariable 方法: 取得環境變數
			// WinDir : Windows 所在目錄, Windows\Fonts 為: 字型所在目錄
			// Path.GetFileName 方法: 要從其中取得檔案名稱和副檔名的路徑字串。

            var font_folder = Path.Combine(Environment.GetEnvironmentVariable("WinDir"), "Fonts");
            var font_target_file = Path.Combine(font_folder, Path.GetFileName(fnt));        

			if (RemoveFontResource(strFontFile) ){// Call API RemoveFontResource 移除字型
				try{
					File.Delete(strFontFile);//  刪除字型檔
					SendMessage(HWND_Broadcast, WM_FontChange, 0, 0) ;// 廣播訊息字型異動

					return true;
				}catch(IOException){

				}
			}

			return false;
		}

		// ref:
		// http://www.dotblogs.com.tw/PowerHammer/archive/2008/03/24/2251.aspx
		public static bool AddODBCSource(){
			// Activator 成員: 包含本機或遠端建立物件型別的方法，或者取得對現有遠端物件的參考。

			// Activator.CreateInstance 方法(Type) : 使用最符合指定參數的建構函式，建立指定型別的執行個體。

			object dbe = Activator.CreateInstance(Type.GetTypeFromProgID("DAO.DBEngine.36"));

			// Type.GetTypeFromProgID 方法: 取得與指定的程式識別項(ProgID) 關聯的型別；

			// 如果在載入Type 時發生錯誤，則傳回null。

			// DAO.DBEngine.36 為Microsoft DAO 3.6 Object Library ( dao360.dll )

			// 宣告變數

			string[] strPara = new string[13];

			string strDSN;

			string strDriver;

			// 設定ODBC DSN 參數內容

			strPara[1] = "UID=admin";

			strPara[2] = "Database=C:\\db1.mdb";

			strPara[3] = "UserCommitSync=Yes";

			strPara[4] = "Threads=3";

			strPara[5] = "SafeTransactions=0";

			strPara[6] = "PageTimeout=5";

			strPara[7] = "MaxScanRows=8";

			strPara[8] = "MaxBufferSize=2048";

			strPara[9] = "FIL=MS Access";

			strPara[10] = "DriverId=25";

			strPara[11] = "DefaultDir=C:\\";

			strPara[12] = "DBQ=C:\\db1.mdb";

			// 使用者資料來源名稱

			strDSN = "MyDataSourceName";

			// 驅動程式

			strDriver = "Microsoft Access Driver (*.mdb)";

			object[] p = new object[] { strDSN, strDriver, true, string.Join('\0'.ToString(), strPara, 1, 12) };

			// Type.InvokeMember 方法

			// Type.InvokeMember (String, BindingFlags, Binder, Object, Object[])

			dbe.GetType().InvokeMember("RegisterDatabase", BindingFlags.InvokeMethod, null, dbe, p);

			// 透過DAO.DBEngine 物件的RegisterDatabase 方法新增一個ODBC 設定
		}
	}
}


