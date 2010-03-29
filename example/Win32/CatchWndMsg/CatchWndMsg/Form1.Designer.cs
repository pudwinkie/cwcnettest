namespace CatchWndMsg {
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
            this.btnSendStruct = new System.Windows.Forms.Button();
            this.btnSendString = new System.Windows.Forms.Button();
            this.btnSendInt32 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSendStruct
            // 
            this.btnSendStruct.Location = new System.Drawing.Point(12, 27);
            this.btnSendStruct.Name = "btnSendStruct";
            this.btnSendStruct.Size = new System.Drawing.Size(75, 23);
            this.btnSendStruct.TabIndex = 0;
            this.btnSendStruct.Text = "Struct";
            this.btnSendStruct.UseVisualStyleBackColor = true;
            this.btnSendStruct.Click += new System.EventHandler(this.btnSendStruct_Click);
            // 
            // btnSendString
            // 
            this.btnSendString.Location = new System.Drawing.Point(121, 27);
            this.btnSendString.Name = "btnSendString";
            this.btnSendString.Size = new System.Drawing.Size(75, 23);
            this.btnSendString.TabIndex = 1;
            this.btnSendString.Text = "String";
            this.btnSendString.UseVisualStyleBackColor = true;
            this.btnSendString.Click += new System.EventHandler(this.btnSendString_Click);
            // 
            // btnSendInt32
            // 
            this.btnSendInt32.Location = new System.Drawing.Point(228, 27);
            this.btnSendInt32.Name = "btnSendInt32";
            this.btnSendInt32.Size = new System.Drawing.Size(75, 23);
            this.btnSendInt32.TabIndex = 2;
            this.btnSendInt32.Text = "Int32";
            this.btnSendInt32.UseVisualStyleBackColor = true;
            this.btnSendInt32.Click += new System.EventHandler(this.btnSendInt32_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 74);
            this.Controls.Add(this.btnSendInt32);
            this.Controls.Add(this.btnSendString);
            this.Controls.Add(this.btnSendStruct);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSendStruct;
        private System.Windows.Forms.Button btnSendString;
        private System.Windows.Forms.Button btnSendInt32;
    }
}

