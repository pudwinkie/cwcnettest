using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace MyNetstat
{
	/// <summary>
	/// Class1 的摘要描述。
	/// </summary>
	/// Reference:
	/// http://www.codeproject.com/csharp/iphlpapi.asp
	/// http://www.pcquest.com/content/coding/102101103.asp
	/// http://www.stevex.org/dottext/articles/158.aspx

	class Class1 : WIN32API
	{
		/// <summary>
		/// 應用程式的主進入點。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: 在此加入啟動應用程式的程式碼
			//
			// For XP Only!!
			MIB_EXTCPTABLE TcpExConnexions;		
			MIB_EXUDPTABLE UdpExConnexion;

			Class1 myclass1 = new Class1();

			int BufferSize, error, CurrentIndex, NumEntries;
			IntPtr current;
			String strLocalAddr, strRemoteAddr, strPIDName, strStat;

			BufferSize = 100000;
			IntPtr lpTcpTable = Marshal.AllocHGlobal(BufferSize);
			error = AllocateAndGetTcpExTableFromStack(ref lpTcpTable, true, GetProcessHeap(), 2, 2 );
			if(error != NO_ERROR)
			{
				Console.WriteLine("Failed to snapshot TCP endpoints.");				
			}

			current = lpTcpTable;
			CurrentIndex = 0;
			// get the (again) the number of entries
			NumEntries = (int)Marshal.ReadIntPtr(current);
			// Make the array of entries
			TcpExConnexions.table = new MIB_EXTCPROW[NumEntries];
			// iterate the pointer of 4 (the size of the DWORD dwNumEntries)
			CurrentIndex+=4;
			current = (IntPtr)((int)current+CurrentIndex);
		
			for(int i = 0; i < NumEntries; i++) 
			{
                				
				// The state of the connexion (in string)
				TcpExConnexions.table[i].StrgState = myclass1.convert_state((int)Marshal.ReadIntPtr(current));
				// The state of the connexion (in ID)
				TcpExConnexions.table[i].iState = (int)Marshal.ReadIntPtr(current);
				// iterate the pointer of 4
				current = (IntPtr)((int)current+4);
				// get the local address of the connexion
				UInt32 localAddr = (UInt32)Marshal.ReadIntPtr(current);
				// iterate the pointer of 4
				current = (IntPtr)((int)current+4);
				// get the local port of the connexion
				UInt32 localPort = (UInt32)Marshal.ReadIntPtr(current);				
				// iterate the pointer of 4
				current = (IntPtr)((int)current+4);
				// Store the local endpoint in the struct and convertthe port in decimal (ie convert_Port())
				TcpExConnexions.table[i].Local = new IPEndPoint(localAddr,(int)myclass1.convert_Port(localPort));
				// get the remote address of the connexion
				UInt32 RemoteAddr = (UInt32)Marshal.ReadIntPtr(current);
				// iterate the pointer of 4
				current = (IntPtr)((int)current+4);
				UInt32 RemotePort=0;				
				// if the remote address = 0 (0.0.0.0) the remote port is always 0
				// else get the remote port
				if(RemoteAddr!=0)
				{
					RemotePort = (UInt32)Marshal.ReadIntPtr(current);
					RemotePort=myclass1.convert_Port(RemotePort);
				}
				current = (IntPtr)((int)current+4);
				// store the remote endpoint in the struct  and convertthe port in decimal (ie convert_Port())
				TcpExConnexions.table[i].Remote = new IPEndPoint(RemoteAddr,(int)RemotePort);
				// store the process ID
				TcpExConnexions.table[i].dwProcessId = (int)Marshal.ReadIntPtr(current);
				// Store and get the process name in the struct
				TcpExConnexions.table[i].ProcessName = myclass1.get_process_name(TcpExConnexions.table[i].dwProcessId);
				current = (IntPtr)((int)current+4);
				
				strLocalAddr = String.Format("{0,-21}",TcpExConnexions.table[i].Local.ToString());
				strRemoteAddr =  String.Format("{0,-21}",TcpExConnexions.table[i].Remote.ToString());
				strPIDName =  String.Format("{0,-20}",TcpExConnexions.table[i].ProcessName.ToString());
				strStat = TcpExConnexions.table[i].StrgState.ToString();
				
				Console.WriteLine("[TCP]   [Local] " + strLocalAddr + " [Remote] " + strRemoteAddr + " [Proc] " + strPIDName + "  [" + strStat + "]");

			}
			
			Console.Write("\n");

			Marshal.FreeHGlobal(lpTcpTable);

			BufferSize = 100000;
			IntPtr lpUdpTable = Marshal.AllocHGlobal(BufferSize);
			error = AllocateAndGetUdpExTableFromStack(ref lpUdpTable, true, GetProcessHeap(), 2, 2 );
			if(error != NO_ERROR)
			{
				Console.WriteLine("Failed to snapshot UDP endpoints.");				
			}

			current = lpUdpTable;
			CurrentIndex = 0;
			// get the (again) the number of entries
			NumEntries = (int)Marshal.ReadIntPtr(current);
			// Make the array of entries
			UdpExConnexion.table = new MIB_EXUDPROW[NumEntries];
			// iterate the pointer of 4 (the size of the DWORD dwNumEntries)
			CurrentIndex+=4;
			current = (IntPtr)((int)current+CurrentIndex);
			
			for(int i = 0; i < NumEntries; i++) 
			{
                				
				UInt32 localAddr = (UInt32)Marshal.ReadIntPtr(current);
				// iterate the pointer of 4
				current = (IntPtr)((int)current+4);
				// get the local port of the connexion
				UInt32 localPort = (UInt32)Marshal.ReadIntPtr(current);
				// iterate the pointer of 4
				current = (IntPtr)((int)current+4);
				// Store the local endpoint in the struct and convertthe port in decimal (ie convert_Port())
				UdpExConnexion.table[i].Local = new IPEndPoint(localAddr,myclass1.convert_Port(localPort));
				// store the process ID
				UdpExConnexion.table[i].dwProcessId = (int)Marshal.ReadIntPtr(current);
				// Store and get the process name in the struct
				UdpExConnexion.table[i].ProcessName = myclass1.get_process_name(UdpExConnexion.table[i].dwProcessId);
				current = (IntPtr)((int)current+4);

				strLocalAddr = String.Format("{0,-21}",UdpExConnexion.table[i].Local.ToString());
				strPIDName =  String.Format("{0,-20}",UdpExConnexion.table[i].ProcessName.ToString());
				
				Console.WriteLine("[UDP]   [Local] " + strLocalAddr + " [Proc] " + strPIDName);

			}

			Marshal.FreeHGlobal(lpUdpTable);
			current = IntPtr.Zero;

			Console.ReadLine();
		}


		private UInt16 convert_Port(UInt32 dwPort)
		{
			byte[] b = new Byte[2];
			// high weight byte
			b[0] = byte.Parse((dwPort>>8).ToString());
			// low weight byte
			b[1] = byte.Parse((dwPort & 0xFF).ToString());
			return BitConverter.ToUInt16(b,0);
		}


		private string convert_state(int state)
		{
			string strg_state="";
			switch(state)
			{
				case MIB_TCP_STATE_CLOSED: strg_state = "CLOSED" ;break;
				case MIB_TCP_STATE_LISTEN: strg_state = "LISTEN" ;break;
				case MIB_TCP_STATE_SYN_SENT: strg_state = "SYN_SENT" ;break;
				case MIB_TCP_STATE_SYN_RCVD: strg_state = "SYN_RCVD" ;break;
				case MIB_TCP_STATE_ESTAB: strg_state = "ESTAB" ;break;
				case MIB_TCP_STATE_FIN_WAIT1: strg_state = "FIN_WAIT1" ;break;
				case MIB_TCP_STATE_FIN_WAIT2: strg_state = "FIN_WAIT2" ;break;
				case MIB_TCP_STATE_CLOSE_WAIT: strg_state = "CLOSE_WAIT" ;break;
				case MIB_TCP_STATE_CLOSING: strg_state = "CLOSING" ;break;
				case MIB_TCP_STATE_LAST_ACK: strg_state = "LAST_ACK" ;break;
				case MIB_TCP_STATE_TIME_WAIT: strg_state = "TIME_WAIT" ;break;
				case MIB_TCP_STATE_DELETE_TCB: strg_state = "DELETE_TCB" ;break;
			}
			return strg_state;
		}


		private string get_process_name(int processID)
		{
			//could be an error here if the process die before we can get his name
			try
			{
				Process p = Process.GetProcessById((int)processID);
				return p.ProcessName;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "Unknown";
			}
				
		}
	}


	class WIN32API
	{
		#region Struct TCP
		[StructLayout(LayoutKind.Sequential)]
			public struct MIB_TCPSTATS
		{
			public int dwRtoAlgorithm ;
			public int dwRtoMin ;
			public int dwRtoMax ;
			public int dwMaxConn ;
			public int dwActiveOpens ;
			public int dwPassiveOpens ;
			public int dwAttemptFails ;
			public int dwEstabResets ;
			public int dwCurrEstab ;
			public int dwInSegs ;
			public int dwOutSegs ;
			public int dwRetransSegs ;
			public int dwInErrs ;
			public int dwOutRsts ;
			public int dwNumConns ;
		}


		public struct MIB_TCPTABLE 
		{
			public int dwNumEntries;  
			public MIB_TCPROW[] table;

		}

		public struct MIB_TCPROW 
		{
			public string StrgState; 
			public int iState;
			public IPEndPoint Local;  
			public IPEndPoint Remote;
		}

		public struct MIB_EXTCPTABLE
		{
			public int dwNumEntries;  
			public MIB_EXTCPROW[] table;

		}

		public struct MIB_EXTCPROW
		{
			public string StrgState;
			public int iState;
			public IPEndPoint Local;  
			public IPEndPoint Remote;
			public int dwProcessId;
			public string ProcessName;
		}
	
		#endregion

		#region Struct UDP
	
		[StructLayout(LayoutKind.Sequential)]
			public struct MIB_UDPSTATS
		{
			public int dwInDatagrams ;
			public int dwNoPorts ;
			public int dwInErrors ;
			public int dwOutDatagrams ;
			public int dwNumAddrs;
		}

		public struct MIB_UDPTABLE 
		{
			public int dwNumEntries;  
			public MIB_UDPROW[] table;

		}

		public struct MIB_UDPROW 
		{
			public IPEndPoint Local;
		}

		public struct MIB_EXUDPTABLE
		{
			public int dwNumEntries;  
			public MIB_EXUDPROW[] table;

		}

		public struct MIB_EXUDPROW
		{
			public IPEndPoint Local;  
			public int dwProcessId;
			public string ProcessName;
		}

		#endregion		
		
		#region Struct ProcessEntry32

		[StructLayout(LayoutKind.Sequential)]
			public struct ProcessEntry32 
		{ 
			public uint dwSize; 
			public uint cntUsage; 
			public uint th32ProcessID; 
			public IntPtr th32DefaultHeapID; 
			public uint th32ModuleID; 
			public uint cntThreads; 
			public uint th32ParentProcessID; 
			public int pcPriClassBase; 
			public uint dwFlags; 
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)] public string szExeFile;
		};

		#endregion

		#region Const Defined

		public const int  TH32CS_SNAPHEAPLIST		= 0x00000001;
		public const int  TH32CS_SNAPPROCESS		= 0x00000002;
		public const int  TH32CS_SNAPTHREAD			= 0x00000004;
		public const int  TH32CS_SNAPMODULE			= 0x00000008;
		public const int  TH32CS_SNAPMODULE32		= 0x00000010;
		public const int  TH32CS_SNAPALL			= (TH32CS_SNAPHEAPLIST | TH32CS_SNAPPROCESS | TH32CS_SNAPTHREAD | TH32CS_SNAPMODULE);
		public const uint TH32CS_INHERIT			= 0x80000000;

		public const int  NO_ERROR					= 0;
		public const int  MIB_TCP_STATE_CLOSED		= 1;
		public const int  MIB_TCP_STATE_LISTEN		= 2;
		public const int  MIB_TCP_STATE_SYN_SENT	= 3;
		public const int  MIB_TCP_STATE_SYN_RCVD	= 4;
		public const int  MIB_TCP_STATE_ESTAB		= 5;
		public const int  MIB_TCP_STATE_FIN_WAIT1	= 6;
		public const int  MIB_TCP_STATE_FIN_WAIT2	= 7;
		public const int  MIB_TCP_STATE_CLOSE_WAIT	= 8;
		public const int  MIB_TCP_STATE_CLOSING		= 9;
		public const int  MIB_TCP_STATE_LAST_ACK	= 10;
		public const int  MIB_TCP_STATE_TIME_WAIT	= 11;
		public const int  MIB_TCP_STATE_DELETE_TCB	= 12;

		#endregion

		#region DllImport

		[DllImport("iphlpapi.dll",SetLastError=true)]
		public extern static int AllocateAndGetTcpExTableFromStack(
			ref IntPtr pTable, 
			bool bOrder, 
			IntPtr heap,
			int zero,
			int flags);

		[DllImport("iphlpapi.dll",SetLastError=true)]
		public extern static int AllocateAndGetUdpExTableFromStack(
			ref IntPtr pTable,
			bool bOrder, 
			IntPtr heap,
			int zero,
			int flags);

		[DllImport("kernel32" ,SetLastError= true)] 
			// this function is 
			// used to get the pointer on the process 
			// heap required by AllocateAndGetTcpExTableFromStack
		public static extern IntPtr GetProcessHeap(); 
		
		#endregion
	}
}