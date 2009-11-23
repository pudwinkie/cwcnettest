using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace dog
{
    public class StringInput : Form
    {
        private ComboBox m_txtFontSize;
        private FontFamily m_selectedFont;
        private TextBox m_txtPreviewText;  
        private Container components = null; 
        private FontListBox fontBox;
        private Label label1;
        private Label label2;
        private Label txtPreviewResult;
        private Button buttonOK;
        private Button buttonCancel;
        private Panel panel2;

        public ComboBox txtFontSize
        {
            get { return m_txtFontSize; }
            set { m_txtFontSize = value; }
        }

        public FontFamily selectedFont
        {
            get { return m_selectedFont; }
            set { m_selectedFont = value; }
        }

        public TextBox txtPreviewText
        {
            get { return m_txtPreviewText; }
            set { m_txtPreviewText = value; }
        }

        public StringInput()
        {
            InitializeComponent();

            m_selectedFont = new FontFamily("Arial");
            m_txtFontSize.SelectedIndex = 1;

            fontBox.SelectedIndex = 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
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
            this.fontBox = new dog.FontListBox();
            this.txtPreviewResult = new System.Windows.Forms.Label();
            this.m_txtFontSize = new ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_txtPreviewText = new TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.SuspendLayout();
            // 
            // fontBox
            // 
            this.fontBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fontBox.Location = new System.Drawing.Point(8, 12);
            this.fontBox.Name = "fontBox";
            this.fontBox.Size = new System.Drawing.Size(172, 164);
            this.fontBox.TabIndex = 2;
            this.fontBox.TabStop = false;
            this.fontBox.SelectedIndexChanged += new System.EventHandler(this.fontBox_SelectedIndexChanged);
            // 
            // txtPreviewResult
            // 
            this.txtPreviewResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtPreviewResult.Location = new System.Drawing.Point(188, 88);
            this.txtPreviewResult.Name = "txtPreviewResult";
            this.txtPreviewResult.Size = new System.Drawing.Size(312, 88);
            this.txtPreviewResult.TabIndex = 6;
            // 
            // m_txtFontSize
            // 
            this.m_txtFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_txtFontSize.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.m_txtFontSize.Items.AddRange(new object[]
                                                {
                                                    "6",
                                                    "8",
                                                    "9",
                                                    "10",
                                                    "11",
                                                    "12",
                                                    "14",
                                                    "16",
                                                    "18",
                                                    "20",
                                                    "24",
                                                    "28",
                                                    "32",
                                                    "36",
                                                    "42",
                                                    "48",
                                                    "56",
                                                    "62",
                                                    "72",
                                                    "84"
                                                });
            this.m_txtFontSize.Location = new System.Drawing.Point(260, 48);
            this.m_txtFontSize.Name = "m_txtFontSize";
            this.m_txtFontSize.Size = new System.Drawing.Size(88, 22);
            this.m_txtFontSize.TabIndex = 5;
            this.m_txtFontSize.TabStop = false;
            this.m_txtFontSize.SelectedIndexChanged += new System.EventHandler(this.txtFontSize_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(188, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 27);
            this.label2.TabIndex = 4;
            this.label2.Text = "Font Size:";
            // 
            // m_txtPreviewText
            // 
            this.m_txtPreviewText.AcceptsReturn = true;
            this.m_txtPreviewText.AcceptsTab = true;
            this.m_txtPreviewText.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                                | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtPreviewText.Location = new System.Drawing.Point(260, 12);
            this.m_txtPreviewText.Name = "m_txtPreviewText";
            this.m_txtPreviewText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_txtPreviewText.Size = new System.Drawing.Size(240, 22);
            this.m_txtPreviewText.TabIndex = 2;
            this.m_txtPreviewText.Text = "";
            this.m_txtPreviewText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPreviewText_KeyUp);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(188, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "Text:";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Location = new System.Drawing.Point(8, 184);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(492, 4);
            this.panel2.TabIndex = 5;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonOK.Location = new System.Drawing.Point(348, 196);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(72, 28);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(428, 196);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(72, 28);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // StringInput
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(510, 232);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.fontBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_txtPreviewText);
            this.Controls.Add(this.txtPreviewResult);
            this.Controls.Add(this.m_txtFontSize);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StringInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input Text";
            this.ResumeLayout(false);
        }

        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                const int dropShadow = 0x00020000;
                CreateParams param = base.CreateParams;
                param.ClassStyle |= dropShadow;

                return param;
            }
        }

        private void fontBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FontFamily _family = fontBox.SelectedItem as FontFamily;
            
            if (_family != null)
            {
                m_selectedFont = _family;
                txtPreviewResult.Font = new Font(m_selectedFont, Convert.ToInt32(m_txtFontSize.SelectedItem));
            }
        }

        private void txtFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPreviewResult.Font = new Font(m_selectedFont, Convert.ToInt32(m_txtFontSize.SelectedItem));
        }

        private void txtPreviewText_KeyUp(object sender, KeyEventArgs e)
        {
            txtPreviewResult.Text = m_txtPreviewText.Text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}