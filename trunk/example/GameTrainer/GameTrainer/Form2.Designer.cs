/* GameTrainer 1.0
 Copyright (C) 2007 Luca Tagliaferri

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

 Copyright (C) 2007 Luca Tagliaferri
 e-mai luca.tagliaferri@gmail.com
*/




namespace GameTrainer
{
    partial class Form2
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProgramName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Percentage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.VarVal = new System.Windows.Forms.TextBox();
            this.NewVarGroupbox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.NewVarVal = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.NewVarGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProgramName
            // 
            this.ProgramName.AutoSize = true;
            this.ProgramName.Location = new System.Drawing.Point(28, 26);
            this.ProgramName.Name = "ProgramName";
            this.ProgramName.Size = new System.Drawing.Size(77, 13);
            this.ProgramName.TabIndex = 0;
            this.ProgramName.Text = "Program Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Percentage);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.VarVal);
            this.groupBox1.Location = new System.Drawing.Point(31, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(198, 248);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variable";
            // 
            // Percentage
            // 
            this.Percentage.Enabled = false;
            this.Percentage.Location = new System.Drawing.Point(26, 175);
            this.Percentage.Name = "Percentage";
            this.Percentage.Size = new System.Drawing.Size(152, 20);
            this.Percentage.TabIndex = 6;
            this.Percentage.Text = "0%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Progress";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(26, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 26);
            this.button1.TabIndex = 4;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "4"});
            this.comboBox1.Location = new System.Drawing.Point(26, 110);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(152, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Length";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Value";
            // 
            // VarVal
            // 
            this.VarVal.Location = new System.Drawing.Point(26, 46);
            this.VarVal.Name = "VarVal";
            this.VarVal.Size = new System.Drawing.Size(152, 20);
            this.VarVal.TabIndex = 0;
            this.VarVal.Text = "0";
            // 
            // NewVarGroupbox
            // 
            this.NewVarGroupbox.Controls.Add(this.label3);
            this.NewVarGroupbox.Controls.Add(this.textBox2);
            this.NewVarGroupbox.Controls.Add(this.button2);
            this.NewVarGroupbox.Controls.Add(this.label4);
            this.NewVarGroupbox.Controls.Add(this.NewVarVal);
            this.NewVarGroupbox.Location = new System.Drawing.Point(266, 58);
            this.NewVarGroupbox.Name = "NewVarGroupbox";
            this.NewVarGroupbox.Size = new System.Drawing.Size(198, 248);
            this.NewVarGroupbox.TabIndex = 5;
            this.NewVarGroupbox.TabStop = false;
            this.NewVarGroupbox.Text = "Variable Found";
            this.NewVarGroupbox.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Address";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(26, 46);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(152, 20);
            this.textBox2.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(26, 201);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(149, 26);
            this.button2.TabIndex = 4;
            this.button2.Text = "Set value";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "New value";
            // 
            // NewVarVal
            // 
            this.NewVarVal.Location = new System.Drawing.Point(26, 103);
            this.NewVarVal.Name = "NewVarVal";
            this.NewVarVal.Size = new System.Drawing.Size(152, 20);
            this.NewVarVal.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 318);
            this.Controls.Add(this.NewVarGroupbox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ProgramName);
            this.Name = "Form2";
            this.Text = "Search a variable";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.NewVarGroupbox.ResumeLayout(false);
            this.NewVarGroupbox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label ProgramName;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox VarVal;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox NewVarGroupbox;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox NewVarVal;
        public System.Windows.Forms.TextBox Percentage;
        private System.Windows.Forms.Label label5;
    }
}
