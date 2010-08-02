using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace NetInterfacePerfMonitor
{

	public class NetIntPerfForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button Stop;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button Start;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label4;
		private System.ComponentModel.IContainer components;
		private System.Diagnostics.PerformanceCounter performanceCounter1;

		public NetIntPerfForm()
		{
			InitializeComponent();
		}

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

		#region Windows Form 設計工具產生的程式碼

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.Stop = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.Start = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 80);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(336, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 0;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// Stop
			// 
			this.Stop.Location = new System.Drawing.Point(280, 120);
			this.Stop.Name = "Stop";
			this.Stop.TabIndex = 1;
			this.Stop.Text = "Stop";
			this.Stop.Click += new System.EventHandler(this.Stop_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 120);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 23);
			this.label1.TabIndex = 2;
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "0";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(288, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "1000";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Start
			// 
			this.Start.Location = new System.Drawing.Point(208, 120);
			this.Start.Name = "Start";
			this.Start.TabIndex = 5;
			this.Start.Text = "Start";
			this.Start.Click += new System.EventHandler(this.Start_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(88, 16);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(264, 23);
			this.comboBox1.TabIndex = 6;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 23);
			this.label4.TabIndex = 7;
			this.label4.Text = "Interface:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// NetIntPerfForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 18);
			this.ClientSize = new System.Drawing.Size(368, 160);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.Start);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Stop);
			this.Controls.Add(this.progressBar1);
			this.MaximizeBox = false;
			this.Name = "NetIntPerfForm";
			this.Text = "Network Interface Monitor";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new NetIntPerfForm());
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			try 
			{
				int val = (int)performanceCounter1.NextValue();                                
				while (val > 0)
				{
					if ( val >= progressBar1.Maximum)
					{
						progressBar1.Maximum *= 10;
						label3.Text = progressBar1.Maximum.ToString();
					}
					else
						break;
				}

				progressBar1.Value = val;
				label1.Text = val.ToString() + " Bytes Total/Sec";

			}
			catch (Exception ex)
			{
				MessageBox.Show("Error! " + ex.Message);
			}		
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			performanceCounter1 = new System.Diagnostics.PerformanceCounter();
			performanceCounter1.MachineName = Dns.GetHostName();
			performanceCounter1.CategoryName = "Network Interface";
			performanceCounter1.CounterName = "Bytes Total/sec";

			PerformanceCounterCategory categoryInstance = new PerformanceCounterCategory("Network Interface"); 
			foreach(string strInstanceName in categoryInstance.GetInstanceNames())
			{
				comboBox1.Items.Add(strInstanceName);			
			}
			comboBox1.SelectedIndex = 0;
		}

		private void Start_Click(object sender, System.EventArgs e)
		{
			timer1.Enabled = true;
		}

		private void Stop_Click(object sender, System.EventArgs e)
		{
			timer1.Enabled = false;
		}

		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			performanceCounter1.InstanceName = comboBox1.SelectedItem.ToString();
		}
	}
}
