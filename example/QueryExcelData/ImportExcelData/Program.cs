using System;
using System.Data.OleDb;

namespace ImportExcelData {
    class Program {
        static void Main(string[] args) {
            using (CExcel excel = new CExcel(@"c:\student.xls")) {
                OleDbDataReader reader = excel.DoQuery(@"SELECT [E-mail] FROM [student$] WHERE [E-mail] = 'a@hinet.net'");// Table 名稱對應格式為 [Sheet名稱$]
                while (reader.Read()) {
                    Console.WriteLine(reader["E-mail"].ToString());
                }
            }            
        }
    }

    public class CExcel :IDisposable  {
        public CExcel(String excelFile) {
            _excelConn = new OleDbConnection(String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", excelFile) );
            _excelConn.Open();
            _command = new OleDbCommand();
            _command.Connection = _excelConn;  
        }

        public OleDbDataReader DoQuery(String sql) {         
            _command.CommandText = sql ; 

            return _command.ExecuteReader();                         
        }

        public void Close() {
            if (_excelConn.State != System.Data.ConnectionState.Closed) {
                _excelConn.Close();
            }
        }

        public void Dispose() {
            Close();
        }

        private OleDbConnection _excelConn;
        private OleDbCommand _command;
    }
}
