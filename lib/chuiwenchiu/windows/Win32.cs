
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

		private const long HWND_Broadcast = 0xFFFF;// �s��
		private const long WM_FontChange  = 0x1D; // �r������

		// �����L�ƹ�
		public static void LockKeyboardAndMouse(int wait_secs){
			if( wait_secs <= 0){
				return; 
			}

			BlockInput(true);
			System.Threading.Thread.Sleep(wait_secs);
			BlockInput(false);
		}

		// �C�|�t�Φw�˪� ODBC Driver
		public static IEnumerable<String> GetODBCDrivers(){
			return Registry.LocalMachine.OpenSubKey(@"Software\ODBC\ODBCINST.INI").GetSubKeyNames();
		}

		public static String GetDefaulPPPOEName(){
			return Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\RAS AutoDial\Default").GetValue("DefaultInternet");
		}

		//ref: 
		//http://www.dotblogs.com.tw/PowerHammer/archive/2008/03/24/2263.aspx
		public static String GetPPPOEInfo(string pppoe_name){
			// ���o�s�u��T
			// RasGetEntryDialParams
		}

		// �w�˨t�Φr��
		//
		// �C�|�w�w�˪��r��
		// (new InstalledFontCollection()).Families
		//
		// ref:
		// http://www.dotblogs.com.tw/PowerHammer/archive/2008/03/24/2247.aspx
		public static bool AddSystemFont(string fnt){
			// GetEnvironmentVariable ��k: ���o�����ܼ�
			// WinDir : Windows �Ҧb�ؿ�, Windows\Fonts ��: �r���Ҧb�ؿ�
			// Path.GetFileName ��k: �n�q�䤤���o�ɮצW�٩M���ɦW�����|�r��C
            var font_folder = Path.Combine(Environment.GetEnvironmentVariable("WinDir"), "Fonts");
            var font_target_file = Path.Combine(font_folder, Path.GetFileName(fnt));            
			try{
				File.Copy(fnt, font_target_file);

				// PrivateFontCollection ���O
				// ���ѱq�r���ɮ׫إߪ��r���t�C���X�A�o�Ǧr���ɮ׬O�ѥΤ�����ε{���Ҵ��ѡC
				// �R�W�Ŷ��GSystem.Drawing.Text
				// �ե�GSystem.Drawing (�bsystem.drawing.dll ��)
				// ��l��PrivateFontCollection ���O���s�������

				var PFC = new System.Drawing.Text.PrivateFontCollection();
				// PrivateFontCollection.AddFontFile ��k: �q���w���ɮױN�r���[�J�o��PrivateFontCollection�C

				PFC.AddFontFile(fnt);
				SendMessage(HWND_Broadcast, WM_FontChange, 0, 0) // �s���T���r������
				return true;
			}catch(FileNotFoundException){
			}catch(IOException){

			}
			return false;
		}

		// �����t�Φr��
		public static bool RemoveSystemFont(string fnt){
			// GetEnvironmentVariable ��k: ���o�����ܼ�
			// WinDir : Windows �Ҧb�ؿ�, Windows\Fonts ��: �r���Ҧb�ؿ�
			// Path.GetFileName ��k: �n�q�䤤���o�ɮצW�٩M���ɦW�����|�r��C

            var font_folder = Path.Combine(Environment.GetEnvironmentVariable("WinDir"), "Fonts");
            var font_target_file = Path.Combine(font_folder, Path.GetFileName(fnt));        

			if (RemoveFontResource(strFontFile) ){// Call API RemoveFontResource �����r��
				try{
					File.Delete(strFontFile);//  �R���r����
					SendMessage(HWND_Broadcast, WM_FontChange, 0, 0) ;// �s���T���r������

					return true;
				}catch(IOException){

				}
			}

			return false;
		}

		// ref:
		// http://www.dotblogs.com.tw/PowerHammer/archive/2008/03/24/2251.aspx
		public static bool AddODBCSource(){
			// Activator ����: �]�t�����λ��ݫإߪ��󫬧O����k�A�Ϊ̨��o��{�����ݪ��󪺰ѦҡC

			// Activator.CreateInstance ��k(Type) : �ϥγ̲ŦX���w�Ѽƪ��غc�禡�A�إ߫��w���O���������C

			object dbe = Activator.CreateInstance(Type.GetTypeFromProgID("DAO.DBEngine.36"));

			// Type.GetTypeFromProgID ��k: ���o�P���w���{���ѧO��(ProgID) ���p�����O�F

			// �p�G�b���JType �ɵo�Ϳ��~�A�h�Ǧ^null�C

			// DAO.DBEngine.36 ��Microsoft DAO 3.6 Object Library ( dao360.dll )

			// �ŧi�ܼ�

			string[] strPara = new string[13];

			string strDSN;

			string strDriver;

			// �]�wODBC DSN �ѼƤ��e

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

			// �ϥΪ̸�ƨӷ��W��

			strDSN = "MyDataSourceName";

			// �X�ʵ{��

			strDriver = "Microsoft Access Driver (*.mdb)";

			object[] p = new object[] { strDSN, strDriver, true, string.Join('\0'.ToString(), strPara, 1, 12) };

			// Type.InvokeMember ��k

			// Type.InvokeMember (String, BindingFlags, Binder, Object, Object[])

			dbe.GetType().InvokeMember("RegisterDatabase", BindingFlags.InvokeMethod, null, dbe, p);

			// �z�LDAO.DBEngine ����RegisterDatabase ��k�s�W�@��ODBC �]�w
		}
	}
}


