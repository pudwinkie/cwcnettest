// MainForm.cs
//
// Copyright (C) Ranjeet Chakraborty,  July 2002, ranjeetc@hotmail.com
//
// Contains code for the main ui form 

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;


namespace WhiteBoard
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FrmMain : System.Windows.Forms.Form
	{

		private System.Windows.Forms.ToolBarButton BtnLine;
		private System.Windows.Forms.ToolBarButton BtnRectangle;
		private System.Windows.Forms.ToolBarButton BtnEllipse;
		private System.Windows.Forms.ToolBar ToolBar1;
		private System.Windows.Forms.ImageList ImageList1;
		private System.ComponentModel.IContainer components;

		//This is the most important class that handles all the networking infrastucture code
		private NetworkManager			m_NetMgr;
		public System.Windows.Forms.Label LblStatus;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button BtnListen;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TxtListeningPort;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TxtServerIP;
		private System.Windows.Forms.Button BtnPeersConnect;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button BtnPeersDisconnect;
		private System.Windows.Forms.RadioButton ChkListenMode;
		private System.Windows.Forms.RadioButton ChkClientMode;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Timer FrmTimer1;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.TreeView TreeViewPeers;
		private System.Windows.Forms.ToolBarButton BtnAbout;
		private System.Windows.Forms.ToolBarButton BtnSave;
		private System.Windows.Forms.ToolBarButton BtnClearScreen;
		private System.Windows.Forms.TextBox TxtServerPort;

		[DllImport("winmm.dll", EntryPoint="PlaySound")] 
		public static extern long PlaySound(String lpszName, long hModule, long dwFlags);
		private int		m_iStatusChgCtr;	
		private Color	m_colorStatusBack, m_colorStatusFore;

		public WhiteBoard.DrawAreaCtrl m_DrawAreaCtrl1;
		public DrawAreaCtrl DrawAreaCtrlMain
		{
			get
			{
				return m_DrawAreaCtrl1;
			}
		}
		private int	m_iListeningPort;

		public FrmMain()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmMain));
			this.ToolBar1 = new System.Windows.Forms.ToolBar();
			this.BtnLine = new System.Windows.Forms.ToolBarButton();
			this.BtnRectangle = new System.Windows.Forms.ToolBarButton();
			this.BtnEllipse = new System.Windows.Forms.ToolBarButton();
			this.BtnSave = new System.Windows.Forms.ToolBarButton();
			this.BtnAbout = new System.Windows.Forms.ToolBarButton();
			this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
			this.LblStatus = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ChkListenMode = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.TxtListeningPort = new System.Windows.Forms.TextBox();
			this.BtnListen = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.TxtServerPort = new System.Windows.Forms.TextBox();
			this.ChkClientMode = new System.Windows.Forms.RadioButton();
			this.BtnPeersConnect = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.TxtServerIP = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.BtnPeersDisconnect = new System.Windows.Forms.Button();
			this.FrmTimer1 = new System.Windows.Forms.Timer(this.components);
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.m_DrawAreaCtrl1 = new WhiteBoard.DrawAreaCtrl();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.TreeViewPeers = new System.Windows.Forms.TreeView();
			this.BtnClearScreen = new System.Windows.Forms.ToolBarButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// ToolBar1
			// 
			this.ToolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.BtnLine,
																						this.BtnRectangle,
																						this.BtnEllipse,
																						this.BtnClearScreen,
																						this.BtnSave,
																						this.BtnAbout});
			this.ToolBar1.DropDownArrows = true;
			this.ToolBar1.ImageList = this.ImageList1;
			this.ToolBar1.Name = "ToolBar1";
			this.ToolBar1.ShowToolTips = true;
			this.ToolBar1.Size = new System.Drawing.Size(624, 25);
			this.ToolBar1.TabIndex = 0;
			// 
			// BtnLine
			// 
			this.BtnLine.ImageIndex = 0;
			this.BtnLine.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.BtnLine.ToolTipText = "Scribble lines on the Whiteboard";
			// 
			// BtnRectangle
			// 
			this.BtnRectangle.ImageIndex = 1;
			this.BtnRectangle.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.BtnRectangle.ToolTipText = "Draw rectangles on the Whiteboard";
			// 
			// BtnEllipse
			// 
			this.BtnEllipse.ImageIndex = 2;
			this.BtnEllipse.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.BtnEllipse.ToolTipText = "Draw Ellipses on the Whiteboard";
			// 
			// BtnSave
			// 
			this.BtnSave.ImageIndex = 4;
			this.BtnSave.ToolTipText = "Save the whiteboard contents to a file";
			// 
			// BtnAbout
			// 
			this.BtnAbout.ImageIndex = 3;
			// 
			// ImageList1
			// 
			this.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.ImageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
			this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// LblStatus
			// 
			this.LblStatus.BackColor = System.Drawing.SystemColors.ControlText;
			this.LblStatus.Location = new System.Drawing.Point(8, 376);
			this.LblStatus.Name = "LblStatus";
			this.LblStatus.Size = new System.Drawing.Size(464, 40);
			this.LblStatus.TabIndex = 6;
			this.LblStatus.Text = "Status";
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.ControlText;
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.ChkListenMode,
																					this.label1,
																					this.TxtListeningPort,
																					this.BtnListen});
			this.groupBox1.Location = new System.Drawing.Point(480, 111);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(136, 104);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// ChkListenMode
			// 
			this.ChkListenMode.Checked = true;
			this.ChkListenMode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.ChkListenMode.Location = new System.Drawing.Point(8, 16);
			this.ChkListenMode.Name = "ChkListenMode";
			this.ChkListenMode.Size = new System.Drawing.Size(120, 16);
			this.ChkListenMode.TabIndex = 12;
			this.ChkListenMode.TabStop = true;
			this.ChkListenMode.Text = "Listening server ?";
			this.ChkListenMode.Click += new System.EventHandler(this.ChkListenMode_Click);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 11;
			this.label1.Text = "Port:";
			// 
			// TxtListeningPort
			// 
			this.TxtListeningPort.Location = new System.Drawing.Point(72, 36);
			this.TxtListeningPort.Name = "TxtListeningPort";
			this.TxtListeningPort.Size = new System.Drawing.Size(48, 20);
			this.TxtListeningPort.TabIndex = 10;
			this.TxtListeningPort.Text = "8888";
			// 
			// BtnListen
			// 
			this.BtnListen.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.BtnListen.Location = new System.Drawing.Point(8, 64);
			this.BtnListen.Name = "BtnListen";
			this.BtnListen.Size = new System.Drawing.Size(120, 24);
			this.BtnListen.TabIndex = 9;
			this.BtnListen.Text = "Start Listening";
			this.BtnListen.Click += new System.EventHandler(this.BtnListen_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.ControlText;
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.label3,
																					this.TxtServerPort,
																					this.ChkClientMode,
																					this.BtnPeersConnect,
																					this.label2,
																					this.TxtServerIP});
			this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.groupBox2.Location = new System.Drawing.Point(480, 224);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(136, 144);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 17;
			this.label3.Text = "Port:";
			// 
			// TxtServerPort
			// 
			this.TxtServerPort.Enabled = false;
			this.TxtServerPort.Location = new System.Drawing.Point(64, 72);
			this.TxtServerPort.Name = "TxtServerPort";
			this.TxtServerPort.Size = new System.Drawing.Size(64, 20);
			this.TxtServerPort.TabIndex = 16;
			this.TxtServerPort.Text = "8888";
			// 
			// ChkClientMode
			// 
			this.ChkClientMode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.ChkClientMode.Location = new System.Drawing.Point(8, 16);
			this.ChkClientMode.Name = "ChkClientMode";
			this.ChkClientMode.Size = new System.Drawing.Size(120, 16);
			this.ChkClientMode.TabIndex = 15;
			this.ChkClientMode.Text = "Connect to :";
			this.ChkClientMode.Click += new System.EventHandler(this.ChkClientMode_Click);
			// 
			// BtnPeersConnect
			// 
			this.BtnPeersConnect.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.BtnPeersConnect.Enabled = false;
			this.BtnPeersConnect.ForeColor = System.Drawing.Color.White;
			this.BtnPeersConnect.Location = new System.Drawing.Point(8, 104);
			this.BtnPeersConnect.Name = "BtnPeersConnect";
			this.BtnPeersConnect.Size = new System.Drawing.Size(120, 24);
			this.BtnPeersConnect.TabIndex = 14;
			this.BtnPeersConnect.Text = "Connect";
			this.BtnPeersConnect.Click += new System.EventHandler(this.BtnPeersConnect_Click);
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(8, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 13;
			this.label2.Text = "ServerIP:";
			// 
			// TxtServerIP
			// 
			this.TxtServerIP.Enabled = false;
			this.TxtServerIP.Location = new System.Drawing.Point(64, 38);
			this.TxtServerIP.Name = "TxtServerIP";
			this.TxtServerIP.Size = new System.Drawing.Size(64, 20);
			this.TxtServerIP.TabIndex = 12;
			this.TxtServerIP.Text = "10.11.12.14";
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.SystemColors.ControlText;
			this.groupBox3.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.BtnPeersDisconnect});
			this.groupBox3.Location = new System.Drawing.Point(480, 376);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(136, 40);
			this.groupBox3.TabIndex = 9;
			this.groupBox3.TabStop = false;
			// 
			// BtnPeersDisconnect
			// 
			this.BtnPeersDisconnect.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.BtnPeersDisconnect.Location = new System.Drawing.Point(8, 9);
			this.BtnPeersDisconnect.Name = "BtnPeersDisconnect";
			this.BtnPeersDisconnect.Size = new System.Drawing.Size(120, 24);
			this.BtnPeersDisconnect.TabIndex = 5;
			this.BtnPeersDisconnect.Text = "Disconnect";
			this.BtnPeersDisconnect.Click += new System.EventHandler(this.BtnPeersDisconnect_Click);
			// 
			// FrmTimer1
			// 
			this.FrmTimer1.Tick += new System.EventHandler(this.FrmTimer1_Tick);
			// 
			// groupBox4
			// 
			this.groupBox4.BackColor = System.Drawing.SystemColors.ControlText;
			this.groupBox4.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.m_DrawAreaCtrl1});
			this.groupBox4.Location = new System.Drawing.Point(8, 32);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(464, 336);
			this.groupBox4.TabIndex = 10;
			this.groupBox4.TabStop = false;
			// 
			// m_DrawAreaCtrl1
			// 
			this.m_DrawAreaCtrl1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.m_DrawAreaCtrl1.Location = new System.Drawing.Point(8, 8);
			this.m_DrawAreaCtrl1.Name = "m_DrawAreaCtrl1";
			this.m_DrawAreaCtrl1.Size = new System.Drawing.Size(448, 320);
			this.m_DrawAreaCtrl1.TabIndex = 2;
			// 
			// groupBox5
			// 
			this.groupBox5.BackColor = System.Drawing.SystemColors.ControlText;
			this.groupBox5.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.TreeViewPeers});
			this.groupBox5.Location = new System.Drawing.Point(480, 32);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(136, 72);
			this.groupBox5.TabIndex = 11;
			this.groupBox5.TabStop = false;
			// 
			// TreeViewPeers
			// 
			this.TreeViewPeers.BackColor = System.Drawing.SystemColors.ControlText;
			this.TreeViewPeers.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.TreeViewPeers.ImageIndex = -1;
			this.TreeViewPeers.Location = new System.Drawing.Point(6, 8);
			this.TreeViewPeers.Name = "TreeViewPeers";
			this.TreeViewPeers.SelectedImageIndex = -1;
			this.TreeViewPeers.Size = new System.Drawing.Size(125, 56);
			this.TreeViewPeers.TabIndex = 3;
			// 
			// BtnClearScreen
			// 
			this.BtnClearScreen.ImageIndex = 5;
			this.BtnClearScreen.ToolTipText = "Clear the contents of the screen";
			// 
			// FrmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(624, 430);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox5,
																		  this.groupBox4,
																		  this.groupBox3,
																		  this.groupBox2,
																		  this.groupBox1,
																		  this.LblStatus,
																		  this.ToolBar1});
			this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FrmMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Whiteboard";
			this.Load += new System.EventHandler(this.FrmMain_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			FrmMain frm = new FrmMain();
			Application.Run(frm);
			if	(frm.m_NetMgr != null)
			{
				/*if	(frm.m_NetMgr.enNetMgrMode == NetworkManager.NETWORK_MANAGER_MODE.enServerMode)
				{
					//frm.m_NetMgr.StopListening();
					//frm.m_NetMgr = null;
				}
				else
				{
				}*/
				frm.m_NetMgr.Disconnect();
				frm.m_NetMgr = null;
			}
			else
			{
			}
			GC.Collect();
			GC.WaitForPendingFinalizers();		
		}

		private void FrmMain_Load(object sender, System.EventArgs e)
		{			
			ToolBar1.ButtonClick += new ToolBarButtonClickEventHandler(this.ToolBar1_ButtonClick);
			m_RootTVNode = null;
			AddRootPeerTreeViewNode();
			SetStatus("Disconnected");
		}


		protected void ToolBar1_ButtonClick (Object sender, ToolBarButtonClickEventArgs e)
		{
			//This ensures that the each button is pushed once
			foreach(ToolBarButton btn in ToolBar1.Buttons)
			{
				if	(!(btn.GetHashCode() == e.Button.GetHashCode()))
					btn.Pushed = false;
			}
			switch(ToolBar1.Buttons.IndexOf(e.Button))
			{
				case 0:
					m_DrawAreaCtrl1.m_enDrawMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enModeLine; 
					break;
				case 1:
					m_DrawAreaCtrl1.m_enDrawMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enModeRect;
					break;
				case 2:
					m_DrawAreaCtrl1.m_enDrawMode = DrawAreaCtrl.WHITEBOARD_DRAW_MODE.enModeEllipse;
					break;
				case 3:
					m_DrawAreaCtrl1.OnClearScreen();
					break;
				case 4:
					SaveFileDialog SaveFileDlg = new SaveFileDialog();
					SaveFileDlg.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*" ;
					SaveFileDlg.FilterIndex = 1;
					SaveFileDlg.RestoreDirectory = true ;
					string StrFileName = "";
					if(SaveFileDlg.ShowDialog() == DialogResult.OK)
					{
						StrFileName = SaveFileDlg.FileName;
						m_DrawAreaCtrl1.BitmapCanvas.Save (StrFileName);
					}
					break;
				case 5:
					AboutForm AbtDlg = new AboutForm();
					AbtDlg.ShowDialog(this);
					AbtDlg.Dispose();
					break;
			}
		}

		
		public void ClearPeersTreeView()
		{
			TreeViewPeers.BeginUpdate();
			//TreeViewPeers.Nodes
			m_RootTVNode.Nodes.Clear();
			TreeViewPeers.EndUpdate();
			SetStatus("User has disconnected...");
			//m_RootTVNode = null;
		}
		
		private TreeNode m_RootTVNode;

		private void AddRootPeerTreeViewNode()
		{
			m_RootTVNode = new TreeNode("Connected Peers");
			TreeViewPeers.BeginUpdate();
			TreeViewPeers.Nodes.Add(m_RootTVNode);
			TreeViewPeers.EndUpdate();
		}

		public void AddPeersTreeViewNode(string StrNode)
		{
			if(m_RootTVNode == null)
				AddRootPeerTreeViewNode();

			TreeViewPeers.BeginUpdate();
			//TreeViewPeers.Nodes.Add(node);
			m_RootTVNode.Nodes.Add(new TreeNode(StrNode));
			m_RootTVNode.Collapse();
			TreeViewPeers.EndUpdate();
			
		}

		public void SetStatus(string StrText)
		{
			LblStatus.Text = StrText;
		}

		public void SetMusicalStatus(string StrText, string StrWaveFileName)
		{
			m_iStatusChgCtr = 0;
			m_colorStatusFore = LblStatus.ForeColor;
			m_colorStatusBack = LblStatus.BackColor;

			LblStatus.Text = StrText;
			if(!StrWaveFileName.Equals(""))
				PlaySound(StrWaveFileName , 0, 0);
			FrmTimer1.Interval = 200;
			FrmTimer1.Enabled = true;
		}
			

		private void FrmTimer1_Tick(object sender, System.EventArgs e)
		{
			const int iTimerInterval = 500;
			
			switch (m_iStatusChgCtr)
				{
				case 0:
					LblStatus.BackColor = m_colorStatusBack;
					LblStatus.ForeColor = m_colorStatusFore;
					LblStatus.BorderStyle = BorderStyle.None;
					FrmTimer1.Enabled = false;
					FrmTimer1.Interval = iTimerInterval;
					FrmTimer1.Enabled = true;
					break;
				case 1:
					LblStatus.ForeColor = Color.YellowGreen;
					LblStatus.BorderStyle = BorderStyle.Fixed3D;

					break;
				case 2:
					LblStatus.BackColor = m_colorStatusBack;
					LblStatus.ForeColor = m_colorStatusFore;
					LblStatus.BorderStyle = BorderStyle.None;
					break;
				case 3:
					LblStatus.ForeColor = Color.YellowGreen;
					LblStatus.BorderStyle = BorderStyle.Fixed3D;
					LblStatus.BorderStyle = BorderStyle.Fixed3D;
					break;
				case 4:
					LblStatus.BackColor = m_colorStatusBack;
					LblStatus.ForeColor = m_colorStatusFore;
					LblStatus.BorderStyle = BorderStyle.None;
					m_iStatusChgCtr = 0;
					FrmTimer1.Enabled = false;
					return;
				default:
					m_iStatusChgCtr = 0;
					FrmTimer1.Enabled = false;
					break;
				}
			m_iStatusChgCtr++;
		}

		public void AppendStatus(string StrText)
		{
			LblStatus.Text += StrText;
		}

		private void BtnPeersConnect_Click(object sender, System.EventArgs e)
		{
			m_iListeningPort = Convert.ToInt32(TxtServerPort.Text); 
			if(m_NetMgr == null)
				{
					m_NetMgr = new NetworkManager
						(
						this, 
						m_iListeningPort, 
						NetworkManager.NETWORK_MANAGER_MODE.enClientMode 
						);
				}
				m_NetMgr.ConnectToListeningWBServer(TxtServerIP.Text, TxtServerPort.Text); 
				m_DrawAreaCtrl1.m_NetMgr = m_NetMgr;
		}

		private void BtnListen_Click(object sender, System.EventArgs e)
		{
			m_iListeningPort = Convert.ToInt32(TxtServerPort.Text); 
			if(m_NetMgr == null)
			{
				//Kick off the network manager
				m_NetMgr = new NetworkManager
					(
					this, 
					m_iListeningPort, 
					NetworkManager.NETWORK_MANAGER_MODE.enServerMode
					);
			}

			m_NetMgr.StartListening();
			m_DrawAreaCtrl1.m_NetMgr = m_NetMgr;
		}

		public void EnableDisableListenModeControls(bool bDisable)
		{
			if(bDisable)
			{
				BtnListen.Enabled = false;
				ChkListenMode.Enabled = false;
				ChkClientMode.Enabled = false;
				//TxtListeningPort.Enabled = false;
				this.Text += "  : Running in the listening mode";
			}
			else
			{
				BtnListen.Enabled = true;
				ChkListenMode.Enabled = true;
				ChkClientMode.Enabled = true;
				//TxtListeningPort.Enabled = true;
				this.Text = "Whiteboard";
				SetStatus("Application has stopped listening");
			}
		}

		public void EnableDisableConnectModeControls(bool bDisable)
		{
			if(bDisable)
			{
				ChkClientMode.Enabled = false;
				BtnListen.Enabled = false;
				ChkListenMode.Enabled = false;
				BtnPeersConnect.Enabled = false;
				//TxtListeningPort.Enabled = false;
			}
			else
			{
				BtnPeersConnect.Enabled = true;
				BtnListen.Enabled = true;
				ChkListenMode.Enabled = true;
				ChkClientMode.Enabled = true;
				//TxtListeningPort.Enabled = true;
				SetStatus("Application has disconnected from the listener");
			}
		}

		private void ChkListenMode_Click(object sender, System.EventArgs e)
		{
			if(sender.GetType() == ChkListenMode.GetType())
			{
				ChkClientMode.Checked = false;
				TxtServerIP.Enabled = false;
				TxtServerPort.Enabled = false;
				BtnPeersConnect.Enabled = false;

				ChkListenMode.Enabled = true;
				TxtListeningPort.Enabled = true;
				BtnListen.Enabled = true;
			}
		}

		private void ChkClientMode_Click(object sender, System.EventArgs e)
		{
			if(sender.GetType() == ChkClientMode.GetType())
			{
				ChkClientMode.Enabled = true;
				TxtServerIP.Enabled = true;
				TxtServerPort.Enabled = true;
				BtnPeersConnect.Enabled = true;

				ChkListenMode.Checked = false;
				TxtListeningPort.Enabled = false;
				BtnListen.Enabled = false;
			}
		}

		private void BtnPeersDisconnect_Click(object sender, System.EventArgs e)
		{
			if(m_NetMgr != null)
			{
				m_NetMgr.Disconnect();
			}
		}






	}
}
