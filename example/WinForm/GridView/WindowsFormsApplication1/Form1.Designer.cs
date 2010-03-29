namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.wM_10001DataSet = new WindowsFormsApplication1.WM_10001DataSet();
            this.wMuseraccountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.wM_user_accountTableAdapter = new WindowsFormsApplication1.WM_10001DataSetTableAdapters.WM_user_accountTableAdapter();
            this.usernameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passwordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.firstnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.birthdayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.personalidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homepageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.skypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hometelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homefaxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.homeaddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.officetelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.officefaxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.officeaddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cellphoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.departmentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.languageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.themeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.msgreservedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logintimesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastloginDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastipDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.regtimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totaltimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wM10001DataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.form1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wM_10001DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wMuseraccountBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wM10001DataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.usernameDataGridViewTextBoxColumn,
            this.passwordDataGridViewTextBoxColumn,
            this.enableDataGridViewTextBoxColumn,
            this.firstnameDataGridViewTextBoxColumn,
            this.lastnameDataGridViewTextBoxColumn,
            this.genderDataGridViewTextBoxColumn,
            this.birthdayDataGridViewTextBoxColumn,
            this.personalidDataGridViewTextBoxColumn,
            this.emailDataGridViewTextBoxColumn,
            this.homepageDataGridViewTextBoxColumn,
            this.skypeDataGridViewTextBoxColumn,
            this.hometelDataGridViewTextBoxColumn,
            this.homefaxDataGridViewTextBoxColumn,
            this.homeaddressDataGridViewTextBoxColumn,
            this.officetelDataGridViewTextBoxColumn,
            this.officefaxDataGridViewTextBoxColumn,
            this.officeaddressDataGridViewTextBoxColumn,
            this.cellphoneDataGridViewTextBoxColumn,
            this.companyDataGridViewTextBoxColumn,
            this.departmentDataGridViewTextBoxColumn,
            this.titleDataGridViewTextBoxColumn,
            this.languageDataGridViewTextBoxColumn,
            this.themeDataGridViewTextBoxColumn,
            this.msgreservedDataGridViewTextBoxColumn,
            this.hidDataGridViewTextBoxColumn,
            this.logintimesDataGridViewTextBoxColumn,
            this.lastloginDataGridViewTextBoxColumn,
            this.lastipDataGridViewTextBoxColumn,
            this.regtimeDataGridViewTextBoxColumn,
            this.totaltimeDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.wMuseraccountBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(39, 30);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 0;
            // 
            // wM_10001DataSet
            // 
            this.wM_10001DataSet.DataSetName = "WM_10001DataSet";
            this.wM_10001DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // wMuseraccountBindingSource
            // 
            this.wMuseraccountBindingSource.DataMember = "WM_user_account";
            this.wMuseraccountBindingSource.DataSource = this.wM_10001DataSet;
            // 
            // wM_user_accountTableAdapter
            // 
            this.wM_user_accountTableAdapter.ClearBeforeFill = true;
            // 
            // usernameDataGridViewTextBoxColumn
            // 
            this.usernameDataGridViewTextBoxColumn.DataPropertyName = "username";
            this.usernameDataGridViewTextBoxColumn.HeaderText = "username";
            this.usernameDataGridViewTextBoxColumn.Name = "usernameDataGridViewTextBoxColumn";
            // 
            // passwordDataGridViewTextBoxColumn
            // 
            this.passwordDataGridViewTextBoxColumn.DataPropertyName = "password";
            this.passwordDataGridViewTextBoxColumn.HeaderText = "password";
            this.passwordDataGridViewTextBoxColumn.Name = "passwordDataGridViewTextBoxColumn";
            // 
            // enableDataGridViewTextBoxColumn
            // 
            this.enableDataGridViewTextBoxColumn.DataPropertyName = "enable";
            this.enableDataGridViewTextBoxColumn.HeaderText = "enable";
            this.enableDataGridViewTextBoxColumn.Name = "enableDataGridViewTextBoxColumn";
            // 
            // firstnameDataGridViewTextBoxColumn
            // 
            this.firstnameDataGridViewTextBoxColumn.DataPropertyName = "first_name";
            this.firstnameDataGridViewTextBoxColumn.HeaderText = "first_name";
            this.firstnameDataGridViewTextBoxColumn.Name = "firstnameDataGridViewTextBoxColumn";
            // 
            // lastnameDataGridViewTextBoxColumn
            // 
            this.lastnameDataGridViewTextBoxColumn.DataPropertyName = "last_name";
            this.lastnameDataGridViewTextBoxColumn.HeaderText = "last_name";
            this.lastnameDataGridViewTextBoxColumn.Name = "lastnameDataGridViewTextBoxColumn";
            // 
            // genderDataGridViewTextBoxColumn
            // 
            this.genderDataGridViewTextBoxColumn.DataPropertyName = "gender";
            this.genderDataGridViewTextBoxColumn.HeaderText = "gender";
            this.genderDataGridViewTextBoxColumn.Name = "genderDataGridViewTextBoxColumn";
            // 
            // birthdayDataGridViewTextBoxColumn
            // 
            this.birthdayDataGridViewTextBoxColumn.DataPropertyName = "birthday";
            this.birthdayDataGridViewTextBoxColumn.HeaderText = "birthday";
            this.birthdayDataGridViewTextBoxColumn.Name = "birthdayDataGridViewTextBoxColumn";
            // 
            // personalidDataGridViewTextBoxColumn
            // 
            this.personalidDataGridViewTextBoxColumn.DataPropertyName = "personal_id";
            this.personalidDataGridViewTextBoxColumn.HeaderText = "personal_id";
            this.personalidDataGridViewTextBoxColumn.Name = "personalidDataGridViewTextBoxColumn";
            // 
            // emailDataGridViewTextBoxColumn
            // 
            this.emailDataGridViewTextBoxColumn.DataPropertyName = "email";
            this.emailDataGridViewTextBoxColumn.HeaderText = "email";
            this.emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            // 
            // homepageDataGridViewTextBoxColumn
            // 
            this.homepageDataGridViewTextBoxColumn.DataPropertyName = "homepage";
            this.homepageDataGridViewTextBoxColumn.HeaderText = "homepage";
            this.homepageDataGridViewTextBoxColumn.Name = "homepageDataGridViewTextBoxColumn";
            // 
            // skypeDataGridViewTextBoxColumn
            // 
            this.skypeDataGridViewTextBoxColumn.DataPropertyName = "skype";
            this.skypeDataGridViewTextBoxColumn.HeaderText = "skype";
            this.skypeDataGridViewTextBoxColumn.Name = "skypeDataGridViewTextBoxColumn";
            // 
            // hometelDataGridViewTextBoxColumn
            // 
            this.hometelDataGridViewTextBoxColumn.DataPropertyName = "home_tel";
            this.hometelDataGridViewTextBoxColumn.HeaderText = "home_tel";
            this.hometelDataGridViewTextBoxColumn.Name = "hometelDataGridViewTextBoxColumn";
            // 
            // homefaxDataGridViewTextBoxColumn
            // 
            this.homefaxDataGridViewTextBoxColumn.DataPropertyName = "home_fax";
            this.homefaxDataGridViewTextBoxColumn.HeaderText = "home_fax";
            this.homefaxDataGridViewTextBoxColumn.Name = "homefaxDataGridViewTextBoxColumn";
            // 
            // homeaddressDataGridViewTextBoxColumn
            // 
            this.homeaddressDataGridViewTextBoxColumn.DataPropertyName = "home_address";
            this.homeaddressDataGridViewTextBoxColumn.HeaderText = "home_address";
            this.homeaddressDataGridViewTextBoxColumn.Name = "homeaddressDataGridViewTextBoxColumn";
            // 
            // officetelDataGridViewTextBoxColumn
            // 
            this.officetelDataGridViewTextBoxColumn.DataPropertyName = "office_tel";
            this.officetelDataGridViewTextBoxColumn.HeaderText = "office_tel";
            this.officetelDataGridViewTextBoxColumn.Name = "officetelDataGridViewTextBoxColumn";
            // 
            // officefaxDataGridViewTextBoxColumn
            // 
            this.officefaxDataGridViewTextBoxColumn.DataPropertyName = "office_fax";
            this.officefaxDataGridViewTextBoxColumn.HeaderText = "office_fax";
            this.officefaxDataGridViewTextBoxColumn.Name = "officefaxDataGridViewTextBoxColumn";
            // 
            // officeaddressDataGridViewTextBoxColumn
            // 
            this.officeaddressDataGridViewTextBoxColumn.DataPropertyName = "office_address";
            this.officeaddressDataGridViewTextBoxColumn.HeaderText = "office_address";
            this.officeaddressDataGridViewTextBoxColumn.Name = "officeaddressDataGridViewTextBoxColumn";
            // 
            // cellphoneDataGridViewTextBoxColumn
            // 
            this.cellphoneDataGridViewTextBoxColumn.DataPropertyName = "cell_phone";
            this.cellphoneDataGridViewTextBoxColumn.HeaderText = "cell_phone";
            this.cellphoneDataGridViewTextBoxColumn.Name = "cellphoneDataGridViewTextBoxColumn";
            // 
            // companyDataGridViewTextBoxColumn
            // 
            this.companyDataGridViewTextBoxColumn.DataPropertyName = "company";
            this.companyDataGridViewTextBoxColumn.HeaderText = "company";
            this.companyDataGridViewTextBoxColumn.Name = "companyDataGridViewTextBoxColumn";
            // 
            // departmentDataGridViewTextBoxColumn
            // 
            this.departmentDataGridViewTextBoxColumn.DataPropertyName = "department";
            this.departmentDataGridViewTextBoxColumn.HeaderText = "department";
            this.departmentDataGridViewTextBoxColumn.Name = "departmentDataGridViewTextBoxColumn";
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "title";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            // 
            // languageDataGridViewTextBoxColumn
            // 
            this.languageDataGridViewTextBoxColumn.DataPropertyName = "language";
            this.languageDataGridViewTextBoxColumn.HeaderText = "language";
            this.languageDataGridViewTextBoxColumn.Name = "languageDataGridViewTextBoxColumn";
            // 
            // themeDataGridViewTextBoxColumn
            // 
            this.themeDataGridViewTextBoxColumn.DataPropertyName = "theme";
            this.themeDataGridViewTextBoxColumn.HeaderText = "theme";
            this.themeDataGridViewTextBoxColumn.Name = "themeDataGridViewTextBoxColumn";
            // 
            // msgreservedDataGridViewTextBoxColumn
            // 
            this.msgreservedDataGridViewTextBoxColumn.DataPropertyName = "msg_reserved";
            this.msgreservedDataGridViewTextBoxColumn.HeaderText = "msg_reserved";
            this.msgreservedDataGridViewTextBoxColumn.Name = "msgreservedDataGridViewTextBoxColumn";
            // 
            // hidDataGridViewTextBoxColumn
            // 
            this.hidDataGridViewTextBoxColumn.DataPropertyName = "hid";
            this.hidDataGridViewTextBoxColumn.HeaderText = "hid";
            this.hidDataGridViewTextBoxColumn.Name = "hidDataGridViewTextBoxColumn";
            // 
            // logintimesDataGridViewTextBoxColumn
            // 
            this.logintimesDataGridViewTextBoxColumn.DataPropertyName = "login_times";
            this.logintimesDataGridViewTextBoxColumn.HeaderText = "login_times";
            this.logintimesDataGridViewTextBoxColumn.Name = "logintimesDataGridViewTextBoxColumn";
            // 
            // lastloginDataGridViewTextBoxColumn
            // 
            this.lastloginDataGridViewTextBoxColumn.DataPropertyName = "last_login";
            this.lastloginDataGridViewTextBoxColumn.HeaderText = "last_login";
            this.lastloginDataGridViewTextBoxColumn.Name = "lastloginDataGridViewTextBoxColumn";
            // 
            // lastipDataGridViewTextBoxColumn
            // 
            this.lastipDataGridViewTextBoxColumn.DataPropertyName = "last_ip";
            this.lastipDataGridViewTextBoxColumn.HeaderText = "last_ip";
            this.lastipDataGridViewTextBoxColumn.Name = "lastipDataGridViewTextBoxColumn";
            // 
            // regtimeDataGridViewTextBoxColumn
            // 
            this.regtimeDataGridViewTextBoxColumn.DataPropertyName = "reg_time";
            this.regtimeDataGridViewTextBoxColumn.HeaderText = "reg_time";
            this.regtimeDataGridViewTextBoxColumn.Name = "regtimeDataGridViewTextBoxColumn";
            // 
            // totaltimeDataGridViewTextBoxColumn
            // 
            this.totaltimeDataGridViewTextBoxColumn.DataPropertyName = "total_time";
            this.totaltimeDataGridViewTextBoxColumn.HeaderText = "total_time";
            this.totaltimeDataGridViewTextBoxColumn.Name = "totaltimeDataGridViewTextBoxColumn";
            // 
            // wM10001DataSetBindingSource
            // 
            this.wM10001DataSetBindingSource.DataSource = this.wM_10001DataSet;
            this.wM10001DataSetBindingSource.Position = 0;
            // 
            // form1BindingSource
            // 
            this.form1BindingSource.DataSource = typeof(WindowsFormsApplication1.Form1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 407);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wM_10001DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wMuseraccountBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wM10001DataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private WM_10001DataSet wM_10001DataSet;
        private System.Windows.Forms.BindingSource wMuseraccountBindingSource;
        private WindowsFormsApplication1.WM_10001DataSetTableAdapters.WM_user_accountTableAdapter wM_user_accountTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn usernameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn passwordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn enableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn genderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn birthdayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn personalidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn homepageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn skypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hometelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn homefaxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn homeaddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn officetelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn officefaxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn officeaddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cellphoneDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn departmentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn languageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn themeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn msgreservedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn logintimesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastloginDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastipDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn regtimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totaltimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource wM10001DataSetBindingSource;
        private System.Windows.Forms.BindingSource form1BindingSource;
    }
}

