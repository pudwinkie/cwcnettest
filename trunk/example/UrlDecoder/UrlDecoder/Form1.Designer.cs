namespace UrlDecoder {
    partial class Form1 {
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
            this.txtDecode = new System.Windows.Forms.TextBox();
            this.txtEncode = new System.Windows.Forms.TextBox();
            this.btnDecode = new System.Windows.Forms.Button();
            this.btnEncode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDecode
            // 
            this.txtDecode.Location = new System.Drawing.Point(12, 30);
            this.txtDecode.Multiline = true;
            this.txtDecode.Name = "txtDecode";
            this.txtDecode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDecode.Size = new System.Drawing.Size(428, 65);
            this.txtDecode.TabIndex = 0;
            // 
            // txtEncode
            // 
            this.txtEncode.Location = new System.Drawing.Point(12, 155);
            this.txtEncode.Multiline = true;
            this.txtEncode.Name = "txtEncode";
            this.txtEncode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEncode.Size = new System.Drawing.Size(428, 64);
            this.txtEncode.TabIndex = 1;
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(12, 114);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(99, 24);
            this.btnDecode.TabIndex = 2;
            this.btnDecode.Text = "Decode";
            this.btnDecode.UseVisualStyleBackColor = true;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(131, 114);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(94, 23);
            this.btnEncode.TabIndex = 3;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 253);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.btnDecode);
            this.Controls.Add(this.txtEncode);
            this.Controls.Add(this.txtDecode);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDecode;
        private System.Windows.Forms.TextBox txtEncode;
        private System.Windows.Forms.Button btnDecode;
        private System.Windows.Forms.Button btnEncode;
    }
}

