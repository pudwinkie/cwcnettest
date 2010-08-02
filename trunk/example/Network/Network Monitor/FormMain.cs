using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Echevil;

namespace Network_Monitor_Sample
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label LabelDownload;
		private System.Windows.Forms.Label LabelUpload;
		private System.Windows.Forms.Label LableDownloadValue;
		private System.Windows.Forms.Label LabelUploadValue;
		private System.Windows.Forms.ListBox ListAdapters;
		private System.Windows.Forms.Timer TimerCounter;
		private System.ComponentModel.IContainer components;

		public FormMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.ListAdapters = new System.Windows.Forms.ListBox();
			this.LabelDownload = new System.Windows.Forms.Label();
			this.LabelUpload = new System.Windows.Forms.Label();
			this.LableDownloadValue = new System.Windows.Forms.Label();
			this.LabelUploadValue = new System.Windows.Forms.Label();
			this.TimerCounter = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// ListAdapters
			// 
			this.ListAdapters.Location = new System.Drawing.Point(16, 24);
			this.ListAdapters.Name = "ListAdapters";
			this.ListAdapters.Size = new System.Drawing.Size(208, 82);
			this.ListAdapters.TabIndex = 0;
			this.ListAdapters.SelectedIndexChanged += new System.EventHandler(this.ListAdapters_SelectedIndexChanged);
			// 
			// LabelDownload
			// 
			this.LabelDownload.Location = new System.Drawing.Point(256, 32);
			this.LabelDownload.Name = "LabelDownload";
			this.LabelDownload.TabIndex = 1;
			this.LabelDownload.Text = "Download Speed:";
			this.LabelDownload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LabelUpload
			// 
			this.LabelUpload.Location = new System.Drawing.Point(256, 80);
			this.LabelUpload.Name = "LabelUpload";
			this.LabelUpload.TabIndex = 2;
			this.LabelUpload.Text = "Upload Speed:";
			this.LabelUpload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LableDownloadValue
			// 
			this.LableDownloadValue.Location = new System.Drawing.Point(392, 32);
			this.LableDownloadValue.Name = "LableDownloadValue";
			this.LableDownloadValue.TabIndex = 3;
			this.LableDownloadValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelUploadValue
			// 
			this.LabelUploadValue.Location = new System.Drawing.Point(392, 80);
			this.LabelUploadValue.Name = "LabelUploadValue";
			this.LabelUploadValue.TabIndex = 4;
			this.LabelUploadValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TimerCounter
			// 
			this.TimerCounter.Interval = 1000;
			this.TimerCounter.Tick += new System.EventHandler(this.TimerCounter_Tick);
			// 
			// FormMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(520, 134);
			this.Controls.Add(this.LabelUploadValue);
			this.Controls.Add(this.LableDownloadValue);
			this.Controls.Add(this.LabelUpload);
			this.Controls.Add(this.LabelDownload);
			this.Controls.Add(this.ListAdapters);
			this.Name = "FormMain";
			this.Text = "Network Monitor Demo";
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.Run(new FormMain());
		}

		private NetworkAdapter[] adapters;
		private NetworkMonitor monitor;

		private void FormMain_Load(object sender, System.EventArgs e)
		{
			monitor	=	new NetworkMonitor();
			this.adapters	=	monitor.Adapters;

			if(adapters.Length == 0)
			{
				this.ListAdapters.Enabled	=	false;
				MessageBox.Show("No network adapters found on this computer.");
				return;
			}

			this.ListAdapters.Items.AddRange(this.adapters);
		}

		private void ListAdapters_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			monitor.StopMonitoring();
			monitor.StartMonitoring(adapters[this.ListAdapters.SelectedIndex]);
			this.TimerCounter.Start();
		}

		private void TimerCounter_Tick(object sender, System.EventArgs e)
		{
			NetworkAdapter adapter	=	this.adapters[this.ListAdapters.SelectedIndex];
			this.LableDownloadValue.Text	=	String.Format("{0:n} kbps",adapter.DownloadSpeedKbps);
			this.LabelUploadValue.Text		=	String.Format("{0:n} kbps",adapter.UploadSpeedKbps);
		}

	}
}
