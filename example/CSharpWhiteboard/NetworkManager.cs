// NetManager.cs
//
// Copyright (C) Ranjeet Chakraborty,  July 2002, ranjeetc@hotmail.com
//
// Contains the Networking infrastructure code. 

using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

//-- The Network Protocol consists of the following

namespace WhiteBoard
{
	/// <summary>
	/// Summary description for NetworkManager.
	/// </summary>
	public class NetworkManager
	{
		//Server Mode
		private Socket				m_SockListener;
		private int					m_iPort;
		private Thread				m_MainThread;
		private FrmMain				m_Creator;
		//private Hashtable			m_htConnectedClients;
		private NETWORK_MANAGER_MODE m_enNetMgrMode;
		private bool				m_bStoppedListening;
		private ClientHandler		m_HndlrForConnectingWB;

		//Client mode
		private Socket				m_SockServer;
		//private NetworkStream		m_NS;
		//private StreamReader		m_SR;
		private string				m_StrHostIp;
		private string				m_StrHostPort;
		private ClientHandler		m_HndlrForListeningWB;

		private AsyncCallback		m_AsyncCallbackAccept;

		public enum NETWORK_MANAGER_MODE
		{
			enServerMode,
			enClientMode
		};
		
		public NETWORK_MANAGER_MODE enNetMgrMode
		{
			get
			{
				return m_enNetMgrMode;
			}
		}

		public int Port
		{
			get
			{
				return m_iPort;
			}
			set
			{
				m_iPort = value;
			}
		}
		public Thread MainThread
		{
			get 
			{
				return m_MainThread;
			}
		}

		public NetworkManager(FrmMain frm, int iPort, NETWORK_MANAGER_MODE enAppMode)
		{
			m_bStoppedListening = true;

			Port = iPort;
			m_Creator = frm;
			m_MainThread = null;
			m_enNetMgrMode = enAppMode;
			m_AsyncCallbackAccept = new AsyncCallback(this.OnAcceptRequest);
		}

		//Server related methods (App behaves as a Server by opening up a listening port)
		public void StartListening()
		{
			m_enNetMgrMode = NETWORK_MANAGER_MODE.enServerMode;

			string StrHostName		= Dns.GetHostName();
			m_SockListener			= new Socket(0, SocketType.Stream, ProtocolType.Tcp);
			IPHostEntry IpEntry		= Dns.GetHostByName (StrHostName);
			IPAddress [] IpAddress	= IpEntry.AddressList; 
			IPEndPoint LocEndpoint	= new IPEndPoint(IpAddress[0], m_iPort);
			m_SockListener.Bind(LocEndpoint);
			m_SockListener.Blocking = true;
			m_SockListener.Listen(-1);
			m_SockListener.BeginAccept(m_AsyncCallbackAccept, m_SockListener);
			m_bStoppedListening = false;
			m_Creator.EnableDisableListenModeControls(true);
			m_Creator.SetMusicalStatus ("Application with IP Address: " + IpAddress[0].ToString() +  " listening on port: " + Convert.ToString(m_iPort), "ding.wav");
			//m_MainThread = new Thread(new ThreadStart(MainListenerProc));
			//m_MainThread.Start();
		}

		public void StopListening()
		{
			/*if	(m_MainThread.ThreadState == ThreadState.Running)
			{
				//m_SockListener.Shutdown(SocketShutdown.Both);
				//m_SockListener = null;
				m_MainThread.Abort();
			}
			m_MainThread.Join();*/
			m_SockListener.Close();
			m_SockListener = null;
			m_bStoppedListening = true;
		}
		public void SendWBMsgConnectedHdlr(WBMessage msg)
		{
			switch(m_enNetMgrMode)
			{
				case NETWORK_MANAGER_MODE.enServerMode:
					if(m_HndlrForConnectingWB != null)
					{
						if(m_HndlrForConnectingWB.Sock != null)
							m_HndlrForConnectingWB.SendWBMessage(msg);
					}
					break;
				case NETWORK_MANAGER_MODE.enClientMode:
					if(m_HndlrForListeningWB != null)
					{
						if(m_HndlrForListeningWB.Sock != null)
							m_HndlrForListeningWB.SendWBMessage(msg); 
					}
					break;				
			}
		}

		public void OnAcceptRequest( IAsyncResult ar )
		{
			try
			{
				Socket SockListener = (Socket)ar.AsyncState;
				if(!m_bStoppedListening)
				{
					Socket SockClient = SockListener.EndAccept(ar);
					//IPAddress.Parse(((IPEndPoint)SockClient.RemoteEndPoint).Address.ToString
					//SockClient.RemoteEndPoint.ToString(); 
					//Control.
					//IPEndPoint ipendpt = (IPEndPoint)SockClient.RemoteEndPoint;
					//m_Creator.AddPeersTreeViewNode(ipendpt.Address.ToString());

					m_HndlrForConnectingWB = new ClientHandler(SockClient, m_Creator);
					m_HndlrForConnectingWB.ProcessClientRequest();

					SockListener.BeginAccept(m_AsyncCallbackAccept, SockListener);
				}
			}
			catch(SocketException sockexcp)
			{
				m_Creator.SetStatus("Socket Accept failed. " + sockexcp.Message);
			}
			catch(Exception exception)
			{
				m_Creator.SetStatus("Accept failed. " + exception.Message);
			}

		}

		/*private void MainListenerProc()
		{
			string StrHostName		= Dns.GetHostName();
			m_SockListener			= new Socket(0, SocketType.Stream, ProtocolType.Tcp);
			IPHostEntry IpEntry		= Dns.GetHostByName (StrHostName);
			IPAddress [] IpAddress	= IpEntry.AddressList; 
			IPEndPoint LocEndpoint	= new IPEndPoint(IpAddress[0], m_iPort);
			m_SockListener.Bind(LocEndpoint);
			m_SockListener.Blocking = true;
			m_SockListener.Listen(-1);
			//The main listener loop
			while(true)
			{
				try
				{
					Socket SockClient	= m_SockListener.Accept();
					m_HndlrForConnectingWB = new ClientHandler(SockClient, m_Creator);
					m_HndlrForConnectingWB.ProcessClientRequest();
				}
				catch(Exception exception)
				{
					//Console.WriteLine("Accept failed. " + exception.Message);
				}

			}
		}*/

		//Client mode methods
		public void ConnectToListeningWBServer(string StrHostIp, string StrHostPort)
		{
			IPAddress	hostadd	= Dns.Resolve(StrHostIp).AddressList[0];
			int			iHostPort = Convert.ToInt32(StrHostPort);
			IPEndPoint	EPhost	= new IPEndPoint(hostadd,iHostPort);

			m_StrHostIp = StrHostIp;
			m_StrHostPort = StrHostPort; 
			m_SockServer = new Socket(
				AddressFamily.InterNetwork, 
				SocketType.Stream,
				ProtocolType.Tcp 
				);
			try
			{
				m_SockServer.Connect(EPhost);
				m_Creator.EnableDisableConnectModeControls(true);
				string msg = "Connected to remote Whiteboard on: " + StrHostIp ;
				msg += " listening on port: " + StrHostPort;
				m_Creator.SetMusicalStatus(msg, "ding.wav");
				m_HndlrForListeningWB = new ClientHandler(m_SockServer, m_Creator);
				m_HndlrForListeningWB.ProcessClientRequest();
					
			}
			catch(NullReferenceException exception)
			{
				m_Creator.SetStatus ("Connect failed. " + exception.Message);
				m_SockServer = null;
			}
			
		}

		public void Disconnect()
		{
			switch(m_enNetMgrMode)
			{
				case NETWORK_MANAGER_MODE.enServerMode:
					if(m_HndlrForConnectingWB != null)
					{
						if(m_HndlrForConnectingWB.Sock != null)
						{
							if(m_HndlrForConnectingWB.Sock.Connected)
							{
								m_HndlrForConnectingWB.DisconnectClient();
								m_HndlrForConnectingWB = null; 
							}
						}
						else
						{
							m_HndlrForConnectingWB = null;
						}
							
					}
					if(m_SockListener != null)
					{
						m_SockListener.Close();
						m_SockListener = null;
						m_bStoppedListening = true;
					}

					m_Creator.EnableDisableListenModeControls(false);
					break;
				case NETWORK_MANAGER_MODE.enClientMode:
					if(m_HndlrForListeningWB != null)
					{
						if(m_HndlrForListeningWB.Sock == null)
						{
							m_HndlrForListeningWB = null;
						}
						else
							if(m_HndlrForListeningWB.Sock.Connected)
							{
								m_HndlrForListeningWB.DisconnectClient(); 
								m_HndlrForListeningWB = null;
							}
					}
					m_Creator.EnableDisableConnectModeControls(false);
					break;				
			}
			m_Creator.ClearPeersTreeView();
		}
	}

	public class ClientHandler
	{
		private Socket	m_SockClient;
		public Socket Sock
		{
			get
			{
				return m_SockClient;
			}
		}
		//Deals with the Async socket IO
		private AsyncCallback	m_AsyncCallbackRead;
		private AsyncCallback	m_AsyncCallbackWrite;
		private NetworkStream	m_NS;
		private byte[]			m_ByteBuf;
		
		private byte[]			m_ByteCache;
		
		private FrmMain			m_Creator;
		//Since this object cant directly access UI controls on the UI thread methods on the 
		// UI controls are invoked via the Control.invoke mechanism which requires 
		// delegates
		private delegate void DelAddPeersTreeViewNode(string StrNode);
		private delegate void DelClearPeersTreeView();

		private DelAddPeersTreeViewNode AddPeersTreeViewNode;
		private DelClearPeersTreeView ClearPeersTreeView;

		public ClientHandler(Socket SockClient, FrmMain ParentForm)
		{
			m_Creator		= ParentForm;
			m_SockClient	= SockClient;
			m_ByteBuf		= new Byte[1024];
			
			m_NS			= new NetworkStream(m_SockClient, FileAccess.ReadWrite);

			//Initialize the AsyncCallback delegate
			m_AsyncCallbackRead	= new AsyncCallback(this.OnReadComplete);
			m_AsyncCallbackWrite= new AsyncCallback(this.OnWriteComplete);
			
			AddPeersTreeViewNode = new DelAddPeersTreeViewNode(m_Creator.AddPeersTreeViewNode);
			ClearPeersTreeView = new DelClearPeersTreeView(m_Creator.ClearPeersTreeView);
			//Control.
			IPEndPoint ipendpt = (IPEndPoint)SockClient.RemoteEndPoint;
			m_Creator.Invoke(AddPeersTreeViewNode, new object [] {ipendpt.Address.ToString()});

			m_ByteCache     = new Byte[2048];
			m_iByteCacheLen = 0;
			m_bOnHeader = true;
			m_iDataPackLen = 0;
		}
		
		public void ProcessClientRequest()
		{
			try
			{
				//IPEndPoint ipendpt = (IPEndPoint)m_SockClient.RemoteEndPoint;
				//m_Creator.AddPeersTreeViewNode(ipendpt.Address.ToString());
				m_NS.BeginRead(m_ByteBuf, 0, m_ByteBuf.Length, m_AsyncCallbackRead, null);
			}
			catch
			{
			}
			finally
			{
			}
			//return m_iUserID;
		}

		public void SendWBMessage(WBMessage msg)
		{
			if(m_NS != null)
			{
				MemoryStream ms = WBMessageHelper.Serialize(msg);
				MemoryStream msLength = WBMessageHelper.Serialize(ms.Length);								
				m_NS.BeginWrite(msLength.GetBuffer(), 0, (int)msLength.Length, m_AsyncCallbackWrite, null);
				m_NS.BeginWrite(ms.GetBuffer(), 0, (int)ms.Length, m_AsyncCallbackWrite, null);
				ms.Close();
				msLength.Close();
			}		
		}

		private void OnReadComplete( IAsyncResult ar )
		{
			try
			{
				int CbRead = m_NS.EndRead(ar);
				//This section clearly assumes data from client is NEVER more than 1024
				if(CbRead > 0)
				{
					//This means there is some data from previous OnReadComplete, that needs to be
					//appended to the beginning of the data buffer in this call.
					//m_Creator.AppendStatus(Convert.ToString(CbRead));
					//m_Creator.AppendStatus("  ");
					if(m_iByteCacheLen > 0)
					{
						//Create a temporary buffer to pass on to ParseData, 
						//bcos buffers are passed by reference and it might get overwritten
						byte [] ByteBuffTemp = new byte[CbRead + m_iByteCacheLen];
						Array.Copy(m_ByteBuf, 0, m_ByteCache, m_iByteCacheLen, CbRead);
						CbRead += m_iByteCacheLen;
						Array.Copy(m_ByteCache, 0, ByteBuffTemp, 0, CbRead);
						ParseData(ByteBuffTemp, CbRead);
					}
					else
					{
						ParseData(m_ByteBuf, CbRead);
					}

					m_NS.BeginRead(m_ByteBuf, 0, m_ByteBuf.Length, m_AsyncCallbackRead, null);

					
				}
				else
				{
					DisconnectClient();
				}
			}
			catch(SocketException SockExcep)
			{
				m_Creator.SetStatus("SocketException, Error: " + SockExcep.Message);
				DisconnectClient();
			}
				//Client disconnects abruptly, remove him from the collection of clients
			catch(IOException IOExcep)
			{
				m_Creator.SetStatus("IOException, Error: " + IOExcep.Message);
				DisconnectClient();
			}
			catch(Exception excp)
			{
				m_Creator.SetStatus("Exception, Error: " + excp.Message);
				DisconnectClient();
			}
		}

		//This is where we should ideally call RouteRequest
		public static void WBMsgProcessThreadPoolProc(object State)
		{
			
		}

		private int m_iByteCacheLen;
		private bool m_bOnHeader;
		private int m_iDataPackLen;
		private const int m_iHdrSize = 56;
		
		// ------------------------------------------------------------------
		// | Data Length | Data ... | Data Length | Data ... | ...
		// ------------------------------------------------------------------
		public void ParseData(byte [] ByteBuff, int iCbRead)
		{
			int offset = 0;
			//long lLenData = 0;
			m_iByteCacheLen = 0;

			while(iCbRead >= m_iHdrSize)
			{
				if(m_bOnHeader)
				{
					m_iDataPackLen = (int)WBMessageHelper.DeserializeL(ByteBuff, offset, m_iHdrSize);
					offset += m_iHdrSize;
					m_bOnHeader = false;
					iCbRead -= m_iHdrSize;
				}
				//Parse the data 
				else
				{
					if(m_iDataPackLen == 0) return; 
					if(m_iDataPackLen > iCbRead)
					{
						Array.Clear(m_ByteCache, 0, 2048);
						Array.Copy(ByteBuff, offset, m_ByteCache, 0, iCbRead);
						m_iByteCacheLen = iCbRead;
						iCbRead -= iCbRead;
						//break;
					}
					else
					{
						//Complete Data packet, deserialize and RouteRequest
						WBMessage msg = WBMessageHelper.Deserialize(ByteBuff, offset, m_iDataPackLen);
						RouteRequest(msg);
						offset += m_iDataPackLen;
						m_iByteCacheLen = 0;
						iCbRead -= m_iDataPackLen;
						m_iDataPackLen = 0;
						m_bOnHeader = true;
					}
				}
			}
			//This is where we have surplus/incomplete data packets
			if(iCbRead > 0)
			{
				Array.Clear(m_ByteCache, 0, 2048);
				Array.Copy(ByteBuff, offset, m_ByteCache, 0, iCbRead);
				m_iByteCacheLen = iCbRead;
			}

		}

		//Asynchronous write operations terminate here
		private void OnWriteComplete( IAsyncResult ar )
		{
			m_NS.EndWrite(ar);
		}

		//This method is practically invoked from a seperate thread than the UI thread
		//(asycn functions)
		public void RouteRequest(WBMessage msg)
		{
			DrawAreaCtrl.WHITEBOARD_DRAW_MODE enOrigMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enWBNone;
			switch(msg.MessageType)
			{
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBBegin:
					m_Creator.SetStatus("User starting drawing on the other end, Whiteboard locked...");
					WBMsgDrawBegin MsgBegin = (WBMsgDrawBegin)msg;
					//This is done to prevent any user input while connected peer draws on his
					//Whiteboard
					m_Creator.DrawAreaCtrlMain.LockWhiteBoardMouseEvents();
						m_Creator.DrawAreaCtrlMain.m_iNoOfUsrs++;
						enOrigMode = m_Creator.DrawAreaCtrlMain.m_enDrawMode; 
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enModeLine;// .enWBNone;
						m_Creator.DrawAreaCtrlMain.WhenMouseDown(MsgBegin.m_PtBegin);
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = enOrigMode;
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBEnd:
					m_Creator.SetStatus("User finished drawing on the other end, Use whiteboard now.");
					WBMsgDrawEnd MsgEnd = (WBMsgDrawEnd)msg;
						enOrigMode = m_Creator.DrawAreaCtrlMain.m_enDrawMode; 
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enModeLine;// .enWBNone;
						m_Creator.DrawAreaCtrlMain.WhenMouseUp(MsgEnd.m_PtEnd);
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = enOrigMode;
						m_Creator.DrawAreaCtrlMain.m_iNoOfUsrs--;
					m_Creator.DrawAreaCtrlMain.UnlockWhiteBoardMouseEvents();
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBLine:
					m_Creator.SetStatus("User using the pencil tool on the other end, Whiteboard locked...");
					WBMsgDrawLine MsgLine = (WBMsgDrawLine)msg;
						enOrigMode = m_Creator.DrawAreaCtrlMain.m_enDrawMode; 
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enModeLine;// .enWBNone;
						m_Creator.DrawAreaCtrlMain.WhenMouseMove(MsgLine.m_Pt);
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = enOrigMode;
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBRectangle:
					m_Creator.SetStatus("User using the Rectangle tool on the other end, Whiteboard locked...");
					WBMsgDrawRectangle MsgRect = (WBMsgDrawRectangle)msg;
						enOrigMode = m_Creator.DrawAreaCtrlMain.m_enDrawMode; 
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enModeRect;// .enWBNone;
						m_Creator.DrawAreaCtrlMain.WhenMouseMove(MsgRect.m_Pt);
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = enOrigMode;
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBEllipse:
					m_Creator.SetStatus("User using the Ellipse tool on the other end, Whiteboard locked...");
					WBMsgDrawEllipse MsgEllipse = (WBMsgDrawEllipse)msg;
						enOrigMode = m_Creator.DrawAreaCtrlMain.m_enDrawMode; 
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enModeEllipse;// .enWBNone;
						m_Creator.DrawAreaCtrlMain.WhenMouseMove(MsgEllipse.m_Pt);
						m_Creator.DrawAreaCtrlMain.m_enDrawMode = enOrigMode;
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBClearScreen:
					m_Creator.SetStatus("User cleared the screen...");
					WBMsgClearScreen MsgClrScreen = (WBMsgClearScreen)msg;
						//enOrigMode = m_Creator.DrawAreaCtrlMain.m_enDrawMode; 
						//m_Creator.DrawAreaCtrlMain.m_enDrawMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enModeEllipse;// .enWBNone;
						m_Creator.DrawAreaCtrlMain.m_iNoOfUsrs++;
						m_Creator.DrawAreaCtrlMain.LockWhiteBoardMouseEvents();
							m_Creator.DrawAreaCtrlMain.ClearScreen();
						m_Creator.DrawAreaCtrlMain.UnlockWhiteBoardMouseEvents();
						m_Creator.DrawAreaCtrlMain.m_iNoOfUsrs--;
						//m_Creator.DrawAreaCtrlMain.m_enDrawMode = enOrigMode;

					break;
			}
		}

		public void DisconnectClient()
		{
			//Problems..Delegate to creator to GC instance and disconnect client
			m_NS.Close();
			m_SockClient.Close();
			m_NS = null;
			m_SockClient= null;
			ClearPeersTreeView();
		}
	}

}
