namespace ChuiWenChiu.LiveUpdate {
    partial class FDonwload {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.pbFileProgress = new System.Windows.Forms.ProgressBar();
            this.btnDownload = new System.Windows.Forms.Button();
            this.pbFileCount = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbFileProgress
            // 
            this.pbFileProgress.Location = new System.Drawing.Point(12, 100);
            this.pbFileProgress.Name = "pbFileProgress";
            this.pbFileProgress.Size = new System.Drawing.Size(247, 23);
            this.pbFileProgress.TabIndex = 2;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(12, 12);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 6;
            this.btnDownload.Text = "下載";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // pbFileCount
            // 
            this.pbFileCount.Location = new System.Drawing.Point(12, 71);
            this.pbFileCount.Name = "pbFileCount";
            this.pbFileCount.Size = new System.Drawing.Size(247, 23);
            this.pbFileCount.TabIndex = 7;
            this.pbFileCount.Click += new System.EventHandler(this.progressBar2_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(93, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(18, 53);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(11, 12);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "#";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 138);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pbFileCount);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.pbFileProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "線上更新";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbFileProgress;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ProgressBar pbFileCount;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
    }
}

