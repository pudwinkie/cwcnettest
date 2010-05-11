namespace UploadToGSite
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbLogin = new System.Windows.Forms.ToolStripButton();
            this.tsbStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUrl = new System.Windows.Forms.ToolStripTextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tsbExportHtml = new System.Windows.Forms.ToolStripButton();
            this.tsbExportCsv = new System.Windows.Forms.ToolStripButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dataSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSourceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLogin,
            this.tsbStart,
            this.tsbExportHtml,
            this.tsbExportCsv,
            this.toolStripSeparator1,
            this.tsbUrl});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(579, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbLogin
            // 
            this.tsbLogin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLogin.Image = ((System.Drawing.Image)(resources.GetObject("tsbLogin.Image")));
            this.tsbLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLogin.Name = "tsbLogin";
            this.tsbLogin.Size = new System.Drawing.Size(23, 22);
            this.tsbLogin.Text = "登入";
            this.tsbLogin.Click += new System.EventHandler(this.tsbLogin_Click);
            // 
            // tsbStart
            // 
            this.tsbStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStart.Image = ((System.Drawing.Image)(resources.GetObject("tsbStart.Image")));
            this.tsbStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStart.Name = "tsbStart";
            this.tsbStart.Size = new System.Drawing.Size(23, 22);
            this.tsbStart.Text = "開始";
            this.tsbStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tsbStart.Click += new System.EventHandler(this.tsbStart_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbUrl
            // 
            this.tsbUrl.Name = "tsbUrl";
            this.tsbUrl.Size = new System.Drawing.Size(400, 25);
            this.tsbUrl.Click += new System.EventHandler(this.toolStripTextBox1_Click);
            // 
            // listView1
            // 
            this.listView1.AutoArrange = false;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(579, 230);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "狀態";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "檔名";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "網址";
            // 
            // tsbExportHtml
            // 
            this.tsbExportHtml.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExportHtml.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportHtml.Image")));
            this.tsbExportHtml.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportHtml.Name = "tsbExportHtml";
            this.tsbExportHtml.Size = new System.Drawing.Size(23, 22);
            this.tsbExportHtml.Text = "toolStripButton3";
            this.tsbExportHtml.Click += new System.EventHandler(this.tsbExportHtml_Click);
            // 
            // tsbExportCsv
            // 
            this.tsbExportCsv.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExportCsv.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportCsv.Image")));
            this.tsbExportCsv.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportCsv.Name = "tsbExportCsv";
            this.tsbExportCsv.Size = new System.Drawing.Size(23, 22);
            this.tsbExportCsv.Text = "toolStripButton4";
            this.tsbExportCsv.Click += new System.EventHandler(this.tsbExportCsv_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1273490881_DoctorMale.png");
            this.imageList1.Images.SetKeyName(1, "1273490956_New.png");
            this.imageList1.Images.SetKeyName(2, "1273490930_Export.png");
            this.imageList1.Images.SetKeyName(3, "1273491126_Stop1NormalRed.png");
            // 
            // dataSourceBindingSource
            // 
            this.dataSourceBindingSource.DataSource = typeof(UploadToGSite.DataSource);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 255);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "UploadToGSite";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSourceBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbLogin;
        private System.Windows.Forms.ToolStripButton tsbStart;
        private System.Windows.Forms.BindingSource dataSourceBindingSource;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripTextBox tsbUrl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbExportHtml;
        private System.Windows.Forms.ToolStripButton tsbExportCsv;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

