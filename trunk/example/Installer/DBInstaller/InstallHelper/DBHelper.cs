using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace InstallHelper {
    [RunInstaller(true)]
    public partial class DBHelper : Installer {
        public DBHelper() {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver) {
            base.Install(stateSaver);
            bool bInstall = true;
            while (bInstall) {
                try {                    
                    bInstall = !InstallDB();
                } catch (InvalidOperationException ex) {
                    if ( System.Windows.Forms.MessageBox.Show("Try Again?\n\n Error Message: \n" + ex.Message, "Install DB Fail", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) {
                        bInstall = false;                        
                        break;
                    }
                } catch (SqlException ex) {
                    if (System.Windows.Forms.MessageBox.Show("Try Again?\n\n Error Message: \n" + ex.Message, "Install DB Fail", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) {
                        bInstall = false;
                        break;
                    }

                }
            }
        }

        private bool InstallDB() {
            frmDbInfo dlg = new frmDbInfo();
            if (Context.Parameters.ContainsKey("dbSource")) {
                dlg.Ip = Context.Parameters["dbsource"];
            }

            if (Context.Parameters.ContainsKey("id")) {
                dlg.Id = Context.Parameters["id"];
            }

            if (Context.Parameters.ContainsKey("pwd")) {
                dlg.Password = Context.Parameters["pwd"];
            }            

            if (dlg.ShowDialog( new ScreenWindow() ) == System.Windows.Forms.DialogResult.OK) {
                Context.Parameters["dbsource"] = dlg.Ip;
                Context.Parameters["id"] = dlg.Id;
                Context.Parameters["pwd"] = dlg.Password;

                ExecuteSql("master", string.Format("CREATE DATABASE[{0}]", dlg.DbName));
            }

            return true;            
        }

        public override void Uninstall(IDictionary savedState) {
            base.Uninstall(savedState);
        }

        public override void Rollback(IDictionary savedState) {            
            base.Rollback(savedState);

        }

        private string LoadSqlFromAssembly(string Name) {

            //得到當前程序集對象
            Assembly Asm = Assembly.GetExecutingAssembly();

            //創建sql腳本的流對象
            Stream strm = Asm.GetManifestResourceStream(Asm.GetName().Name + "." + Name);

            //讀sql流對象
            StreamReader reader = new StreamReader(strm);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DatabaseName"></param>
        /// <param name="Sql"></param>
        private void ExecuteSql(string DatabaseName, string Sql) {
            //創建一個數據庫連接對象
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "user id=" + this.Context.Parameters["id"]
                + ";data source=" + this.Context.Parameters["dbsource"]
                + ";initial catalog=master;password=" + this.Context.Parameters["pwd"] + ";";
            //open數據庫的連接
            conn.Open();

            //創建一個sql command對象,去運行sql腳本的內容
            SqlCommand command = new SqlCommand(Sql, conn);

            
            //command.Connection.Open();

            //因為調用這個方法前,在install的方法裡面command��運行在master的庫上面的,所以需要change到當前數據庫中.
            command.Connection.ChangeDatabase(DatabaseName);
            try {
                //執行sql腳本,生成數據表和初始數據.
                command.ExecuteNonQuery();
            } finally {
                //Finally, blocks are a great way to ensure that the connection
                //is always closed.
                command.Connection.Close();
            }
        }
    }

    class ScreenWindow : IWin32Window {
        public IntPtr Handle {
            get {
                return IntPtr.Zero;
            }
        }
    }
}
