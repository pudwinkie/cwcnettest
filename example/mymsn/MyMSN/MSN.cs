/* 
 * dotMSN example
 * Last modified : 22 March 2004
 * Created by    : Bas Geertsema, Xih Productions <orphix@planet.nl>
 * DotMSN site	 : http://members.home.nl/b.geertsema/dotMSN
 *
 * **** A short note about sending messages ****
 * 
 * The 'workflow' of sending messages is:
 * - Request a conversation through Messenger.RequestConversation(..)
 *	 You can setup eventhandlers to the Conversation object you receive from this function. 
 * - Catch the Messenger.ContactJoined(..) event. After the first contact has joined you can start
 *	 sending messages. Sending messages earlier in this process will fail.
 * - Use Conversation.SendMessage(..) to send messages to all other participants in the conversation
 * 
 * When someone invites you:
 * - Catch the Messenger.ConversationCreated(..) event. In this event both parties have agreed
 *	 to create a conversation and therefore a DotMSN.Conversation object is created which you can initalize.
 *	 Note that this event is also called when you send the conversation request.
 *
 * **** About file transfers ****
 * 
 * You can only send file transfers after you have established a conversation. You can check Messenger.Conversations
 * to check whether you a conversation with the contactperson already exists or you have to create a new conversation.
 * 
 * I hope this example will provide a good basic understanding of how DotMSN works. If you come across bugs, drop me a mail.
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using DataAccess;
using Network.IPAddress;
using DotMSN;

namespace MSN
{
	public class MSN : System.Windows.Forms.Form
	{
		#region Variables
		private System.Windows.Forms.TextBox Log;
		private System.Windows.Forms.Button StartButton;
		private System.Windows.Forms.TextBox mailTextBox;
		private System.Windows.Forms.TextBox passTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button showlistButton;
		private System.Windows.Forms.ListView contactListView;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button SendFileButton;
		private System.Windows.Forms.ComboBox cboStatus;
		private System.Windows.Forms.CheckBox chkLog;

		private System.ComponentModel.Container components = null;
		#endregion
		
		#region Standard windows.forms code
		[STAThread]
		static void Main() 
		{
			Application.Run(new MSN());
		}

		public MSN()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();			

			cboStatus.Items.Add("在线");
			cboStatus.Items.Add("忙碌");
			cboStatus.Items.Add("离线");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion


		// this object will be the interface to the dotMSN library
		private DotMSN.Messenger messenger = null;
		private IPScanner ip = null;
		private string dbconnection="Password=\"\";Persist Security Info=True;User ID=sa;Initial Catalog=Messaging;Data Source=(local)";

		// Called when the button 'Connected' is clicked
		private void StartMSN()
		{						
			messenger = new Messenger();
			ip = new IPScanner();
			ip.DataPath=Application.StartupPath + @"\..\..\QQWry.Dat";

			try
			{				
				// make sure we don't use the default settings, since they're invalid
				if(mailTextBox.Text == "yourmail@hotmail.com")
					MessageBox.Show(this, "请输入您的MSN账号！");
				else
				{
					// setup the callbacks
					// we log when someone goes online
					messenger.ContactOnline += new Messenger.ContactOnlineHandler(ContactOnline);

					// we want to do something when we have a conversation
					messenger.ConversationCreated += new Messenger.ConversationCreatedHandler(ConversationCreated);

					// notify us when synchronization is completed
					messenger.SynchronizationCompleted += new Messenger.SynchronizationCompletedHandler(OnSynchronizationCompleted);

					messenger.ReverseAdded += new Messenger.ReverseAddedHandler(ReverseAdded);

					messenger.ReverseRemoved += new Messenger.ReverseRemovedHandler(ReverseRemoved);

					// everything is setup, now connect to the messenger service
					messenger.Connect(mailTextBox.Text, passTextBox.Text);					
					AddLog ("连接成功！\r\n");

					// synchronize the whole list.
					// remember you can only do this once per session!
					// after synchronizing the initial status will be set.
					messenger.SynchronizeList();
					
					/* uncomment this when you want to automatically add
					 * people who have added you to their contactlist on your own
					 * contactlist. (remember the pop-up dialog in MSN Messenger client when someone adds you, this is the 'automatic' method)					 */
					foreach(Contact contact in
						messenger.GetListEnumerator(MSNList.ReverseList))
					{						
						messenger.AddContact(contact.Mail);
					}
					
				}
			}
			catch(MSNException e)
			{
				// in case of an error, report this to the user (or developer)
				MessageBox.Show(this, "连接失败：" + e.ToString());
			}			

		}

		/// <summary>
		/// When the MSN server responds we can setup a conversation (the other party agreed)
		/// the Messenger.ConversationCreated event is called so we can initialize the
		/// Conversation object.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ConversationCreated(Messenger sender, ConversationEventArgs e)
		{
			// we request a conversation or were asked one. Now log this
			AddLog ("会话建立！\r\n");

			// remember there are not yet users in the conversation (except ourselves)
			// they will join _after_ this event. We create another callback to handle this.
			// When user(s) have joined we can start sending messages.
			e.Conversation.ContactJoin += new Conversation.ContactJoinHandler(ContactJoined);			

			// log the event when the two clients are connected
			e.Conversation.ConnectionEstablished += new Conversation.ConnectionEstablishedHandler(ConnectionEstablished);

			e.Conversation.MessageReceived += new Conversation.MessageReceivedHandler(ReadMsg);

			// notify us when the other contact is typing something
			e.Conversation.UserTyping  += new Conversation.UserTypingHandler(ContactTyping);			

			// we want to be accept filetransfer invitations
			e.Conversation.FileTransferHandler.InvitationReceived +=new DotMSN.FileTransferHandler.FileTransferInvitationHandler(FileTransferHandler_FileTransferInvitation);
		}


		private void AddLog(string strLog)
		{
			if(chkLog.Checked)
			{
				Log.Visible=false;
				Log.Text += strLog;
				Log.SelectionStart=Log.Text.Length;
				Log.ScrollToCaret();
				Log.Visible=true;
			}
		}

		/// <summary>
		/// Log when the connection is actually established between the two clients.
		/// You can not yet send messages, the other contact must join first (if you have initiated the conversation)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ConnectionEstablished(Conversation sender, EventArgs e)
		{
			AddLog ("连接建立。\r\n\r\n");
		}

		// this is actually just annoying but it proves the concept
		private void ContactTyping(Conversation sender, ContactEventArgs e)
		{
			AddLog (e.Contact.Name + " 正在输入...\r\n\r\n");
		}

		// log the event when a contact goed online
		private void ContactOnline(Messenger sender, ContactEventArgs e)
		{
			AddLog (e.Contact.Name + " 上线\r\n\r\n");
		}

		private void ReverseAdded(Messenger sender, ContactEventArgs e)
		{
			AddLog ("别人加了你： " + e.Contact.Mail);
			messenger.AddContact(e.Contact.Mail);
		}

		void ReverseRemoved(Messenger sender, ContactEventArgs e)
		{
			AddLog ("你别别人添加了: " + e.Contact.Mail);
			messenger.RemoveContact(e.Contact);
		}

		/// <summary>
		/// After the first contact has joined you can actually send messages to the
		/// other contact(s)!
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContactJoined(Conversation sender, ContactEventArgs e)
		{
			// someone joined our conversation! remember that this also occurs when you are
			// only talking to 1 other person. Log this event.
			AddLog (e.Contact.Name + " 加入会话中。\r\n\r\n");

			// now say something back. You can send messages using the Conversation object.
			SendHelp(sender);
		}
		
		private void SendHelp(Conversation sender)
		{
			sender.SendMessage("您好！我是SIM，机器人：）\r\n请输入：\r\n IP 查询您的IP地址查询IP数据，如：\r\nIP 202.96.128.68\r\nIP www.163.com\r\n\r\nHelp 帮助");
		}

		private void ReadMsg(Conversation sender, DotMSN.MessageEventArgs e)
		{
			AddLog(DateTime.Now + "\t" + e.Sender.Name + ":" +e.Message.Text);
			string input = e.Message.Text;
			string paras = "";

			if(input.ToUpper().StartsWith("IP"))
			{
				paras = input.Substring(input.ToUpper().IndexOf("IP")+2);
				if(paras!=null&&paras!="")
				{
					paras=Regex.Replace(paras,"\r\n","");
					paras=Regex.Replace(paras,"\r","");
					paras=paras.Trim();
					ip.IP=paras;
					paras=ip.IPLocation();
					sender.SendMessage(paras);
				}
				else
				{
					DataSet ds=SqlHelper.ExecuteDataset(dbconnection, CommandType.Text, "SELECT TOP 1 * FROM RobotResponses WHERE (CategoryID = 4) ORDER BY NEWID()");
					DataRow dataRow=ds.Tables[0].Rows[0];
					sender.SendMessage(dataRow["ResponseContent"].ToString());
				}
			}
			else if(input.ToUpper().StartsWith("HELP"))
			{
				SendHelp(sender);
			}
			else
			{
				string content=null;
				DataRow dataRow=null;
				DataSet ds=SqlHelper.ExecuteDataset(dbconnection, CommandType.Text, "SELECT CategoryID FROM RobotKeywords WHERE (CHARINDEX(KeywordContent, '" + Regex.Replace(input,"'","''") + "') <> 0)");

				if (ds.Tables[0].Rows.Count>0)
				{
					dataRow=ds.Tables[0].Rows[0];
					content=dataRow["CategoryID"].ToString();
				}

				if (content!=null&&content!="")
				{
					ds=SqlHelper.ExecuteDataset(dbconnection, CommandType.Text, "SELECT TOP 1 * FROM RobotResponses WHERE (CategoryID = " +content+ ") ORDER BY NEWID()");
					if (ds.Tables[0].Rows.Count>0)
					{
						dataRow=ds.Tables[0].Rows[0];
						content=dataRow["ResponseContent"].ToString();
					}
				}

				if(content==null||content=="")
				{
					ds=SqlHelper.ExecuteDataset(dbconnection, CommandType.Text, "SELECT TOP 1 * FROM RobotResponses WHERE (CategoryID = 5) ORDER BY NEWID()");
					if (ds.Tables[0].Rows.Count>0)
					{
						dataRow=ds.Tables[0].Rows[0];
						content=dataRow["ResponseContent"].ToString();
					}
				}

				sender.SendMessage(content);

				AddLog(content);
			}

			/*string result = SMSSender.SendSMS(paras[0],paras[1]);
			((Conversation)messenger.Conversations[0]).SendMessage(result);*/
		}
		
		private void CloseClick(object obj,EventArgs ea)
		{
			//
		}

		private void TitleClick(object obj,EventArgs ea)
		{
			//
		}

		private void ContentClick(object obj,EventArgs ea)
		{
			//
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Log = new System.Windows.Forms.TextBox();
			this.StartButton = new System.Windows.Forms.Button();
			this.mailTextBox = new System.Windows.Forms.TextBox();
			this.passTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.showlistButton = new System.Windows.Forms.Button();
			this.contactListView = new System.Windows.Forms.ListView();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.SendFileButton = new System.Windows.Forms.Button();
			this.cboStatus = new System.Windows.Forms.ComboBox();
			this.chkLog = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// Log
			// 
			this.Log.Location = new System.Drawing.Point(8, 232);
			this.Log.Multiline = true;
			this.Log.Name = "Log";
			this.Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.Log.Size = new System.Drawing.Size(384, 96);
			this.Log.TabIndex = 0;
			this.Log.Text = "";
			// 
			// StartButton
			// 
			this.StartButton.Location = new System.Drawing.Point(128, 400);
			this.StartButton.Name = "StartButton";
			this.StartButton.Size = new System.Drawing.Size(90, 25);
			this.StartButton.TabIndex = 1;
			this.StartButton.Text = "连接";
			this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
			// 
			// mailTextBox
			// 
			this.mailTextBox.Location = new System.Drawing.Point(96, 344);
			this.mailTextBox.Name = "mailTextBox";
			this.mailTextBox.Size = new System.Drawing.Size(297, 21);
			this.mailTextBox.TabIndex = 2;
			this.mailTextBox.Text = "yourmail@hotmail.com";
			// 
			// passTextBox
			// 
			this.passTextBox.Location = new System.Drawing.Point(96, 368);
			this.passTextBox.Name = "passTextBox";
			this.passTextBox.PasswordChar = '*';
			this.passTextBox.Size = new System.Drawing.Size(297, 21);
			this.passTextBox.TabIndex = 3;
			this.passTextBox.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 344);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "账号";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 368);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 18);
			this.label2.TabIndex = 5;
			this.label2.Text = "密码";
			// 
			// showlistButton
			// 
			this.showlistButton.Location = new System.Drawing.Point(240, 400);
			this.showlistButton.Name = "showlistButton";
			this.showlistButton.Size = new System.Drawing.Size(144, 25);
			this.showlistButton.TabIndex = 6;
			this.showlistButton.Text = "显示在线联系人";
			this.showlistButton.Click += new System.EventHandler(this.showlistButton_Click);
			// 
			// contactListView
			// 
			this.contactListView.Location = new System.Drawing.Point(8, 24);
			this.contactListView.MultiSelect = false;
			this.contactListView.Name = "contactListView";
			this.contactListView.Size = new System.Drawing.Size(384, 146);
			this.contactListView.TabIndex = 7;
			this.contactListView.View = System.Windows.Forms.View.List;
			this.contactListView.DoubleClick += new System.EventHandler(this.contactListView_DoubleClick);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(317, 17);
			this.label3.TabIndex = 8;
			this.label3.Text = "双击启动会话！";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 216);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 17);
			this.label4.TabIndex = 9;
			this.label4.Text = "日志";
			// 
			// SendFileButton
			// 
			this.SendFileButton.Location = new System.Drawing.Point(8, 176);
			this.SendFileButton.Name = "SendFileButton";
			this.SendFileButton.Size = new System.Drawing.Size(90, 25);
			this.SendFileButton.TabIndex = 10;
			this.SendFileButton.Text = "发送文件";
			this.SendFileButton.Click += new System.EventHandler(this.SendFileButton_Click);
			// 
			// cboStatus
			// 
			this.cboStatus.AllowDrop = true;
			this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStatus.Location = new System.Drawing.Point(272, 176);
			this.cboStatus.Name = "cboStatus";
			this.cboStatus.Size = new System.Drawing.Size(121, 20);
			this.cboStatus.TabIndex = 11;
			this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
			// 
			// chkLog
			// 
			this.chkLog.Location = new System.Drawing.Point(144, 176);
			this.chkLog.Name = "chkLog";
			this.chkLog.TabIndex = 12;
			this.chkLog.Text = "记录";
			// 
			// MSN
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(412, 429);
			this.Controls.Add(this.chkLog);
			this.Controls.Add(this.cboStatus);
			this.Controls.Add(this.SendFileButton);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.contactListView);
			this.Controls.Add(this.showlistButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.passTextBox);
			this.Controls.Add(this.mailTextBox);
			this.Controls.Add(this.Log);
			this.Controls.Add(this.StartButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MSN";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MSN服务";
			this.ResumeLayout(false);

		}
		#endregion

		private void StartButton_Click(object sender, System.EventArgs e)
		{
			// Start our journey
			StartMSN();
		}

		/// <summary>
		/// The user clicks on the button to show all online contacts in the contact list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void showlistButton_Click(object sender, System.EventArgs e)
		{
			FillListview();
		}

		/// <summary>
		/// Fill the listview with the users who are online. By double-clicking on a listview-item
		/// a conversation is created with the corresponding contact.
		/// </summary>
		private void FillListview()
		{
			contactListView.Clear();
			foreach(Contact contact in messenger.GetListEnumerator(MSNList.ForwardList))
			{
				// if the contact is not offline we can send messages and we want to show
				// it in the contactlistview
				if(contact.Status != MSNStatus.Offline)
				{
					// add this contact to the listview,
					// we 'tag' the listitem with the contact object
					ListViewItem item = new ListViewItem(contact.Name);
					item.Tag		  = contact;
					contactListView.Items.Add(item);
				}
			}
		}

		/// <summary>
		/// Called when the synchronization is completed. When this happens
		/// we want to fill the listbox on the form.
		/// </summary>
		/// <param name="sender">The messenger object</param>
		/// <param name="e">Contains nothing important</param>
		private void OnSynchronizationCompleted(Messenger sender, EventArgs e)
		{
			// first show all people who are on our forwardlist. This is the 'main' contactlist
			// a normal person would see when logging in.
			// if you want to get all 'online' people enumerate trough this list and extract
			// all contacts with the right DotMSN.MSNStatus  (eg online/away/busy)
			foreach(Contact contact in messenger.GetListEnumerator(MSNList.ForwardList))
			{					
				AddLog ("FL > " + contact.Name + " (" + contact.Status + ")\r\n");
				FillListview();
			}

			// now get the reverse list. This list shows all people who have you on their
			// contactlist.
			foreach(Contact contact in messenger.ReverseList)
			{
				AddLog ("RL > " + contact.Name + " (" + contact.Status + ")\r\n");
			}

			// we follow with the blocked list. this shows all the people who are blocked
			// by you
			foreach(Contact contact in messenger.BlockedList)
			{
				AddLog ("BL > " + contact.Name + " (" + contact.Status + ")\r\n");
			}

			// when the privacy of the client is set to MSNPrivacy.NoneButAllowed then only
			// the contacts on the allowedlist are able to see your status
			foreach(Contact contact in messenger.AllowedList)
			{
				AddLog ("AL > " + contact.Name + " (" + contact.Status + ")\r\n");
			}

			ChangeStatus(0);

			/* the alllist just enumerates all possible contacts. it is not very usefull in
				 * this sample so i've commented it out.
				foreach(Contact contact in messenger.AllList)
				{
					AddLog ("AL > " + contact.Name + " (" + contact.Status + ")");
				} */
		}

		private void ChangeStatus(int index)
		{
			MSNStatus status=MSNStatus.Online;
			switch(index)
			{
				case 0:
					status=MSNStatus.Online;
					break;
				case 1:
					status=MSNStatus.Busy;
					break;
				case 2:
					status=MSNStatus.Hidden;
					break;
			}
			messenger.SetStatus(status);
			AddLog ("状态设置为"+cboStatus.Items[index].ToString()+"！\r\n\r\n");
		}

		/// <summary>
		/// Called when the user doubleclicks on a contact in the contactlist.
		/// We will send a message to the contact which have been selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contactListView_DoubleClick(object sender, System.EventArgs e)
		{
			// when this contact we inserted in the listview we let the 'tag' property
			// point to the corresponsing Contact object of the dotMSN library.
			Contact contact = (DotMSN.Contact)contactListView.SelectedItems[0].Tag;

			// Do the conversation request. You can optionally define clientdata,
			// for example your own conversation object, such that the request can
			// be identified in events later on.
			messenger.RequestConversation(contact.Mail);			
		}

		private void FileTransferHandler_FileTransferInvitation(FileTransferHandler sender, FileTransferInvitationEventArgs e)
		{
			// We want to accept it. You can check the e.FileTransfer object for more info.			
			e.Accept = true;
			// uncomment this to save the file to your C-drive
			//e.FileTransfer.ReceiveStream = new FileStream("C:\\" + e.FileTransfer.FileName, FileMode.Create, FileAccess.ReadWrite);
			AddLog ("邀请文件传输 " + e.FileTransfer.FileName + "\r\n");
		}

		
		#region Filetransfer code

		/// <summary>
		/// When a contact is selected in the listview a file-dialog will be presented
		/// and a file will be send to the contact.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SendFileButton_Click(object sender, System.EventArgs e)
		{
			if(contactListView.SelectedItems.Count == 0) return;

			FileDialog dialog = new OpenFileDialog();
			if(dialog.ShowDialog() != DialogResult.OK)
				return;

			// when this contact we inserted in the listview we let the 'tag' property
			// point to the corresponsing Contact object of the dotMSN library.
			Contact contact = (DotMSN.Contact)contactListView.SelectedItems[0].Tag;

			// first check whether we already have a session we can capture to send the file
			foreach(Conversation conversation in messenger.Conversations)
			{
				if(conversation.Users.ContainsKey(contact.Mail))
				{
					SendFile(conversation, contact, dialog.FileName);
					return;
				}					
			}

			// we have to create a conversation (session) before we can send a file
			Conversation newConversation = messenger.RequestConversation(contact.Mail, dialog.FileName);
			
			// set this callback so we can start the file as soon as the other contact has joined the conversation
			newConversation.ContactJoin +=new DotMSN.Conversation.ContactJoinHandler(filetransferConversation_ContactJoin);
		}

		/// <summary>
		/// Called when a session/conversation has been created in order to send a file. After the contact has joined
		/// we are going to send the file which is stored in the Conversation.ClientData field.
		/// </summary>
		private void filetransferConversation_ContactJoin(Conversation sender, ContactEventArgs e)
		{
			SendFile(sender, e.Contact, (string)sender.ClientData);
		}

		/// <summary>
		/// Send the file to the specified conversation/contact combination
		/// </summary>		
		private void SendFile(Conversation conversation, Contact contact, string filename)
		{
			conversation.FileTransferHandler.TransferFile(contact.Mail, filename);
		}

		#endregion

		private void cboStatus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ChangeStatus(cboStatus.SelectedIndex);
		}

	}
}
