
#region ExportMht
// �ѦҡGMicrosoft CDO for Windows 2000 Library
 public static void ExportMht(string url, string fn){
	 ExportMht(url, fn, String.Empty, String.Empty);
 }

 public static void ExportMht(string url, string fn, string id, string pwd){
	 CDO.Message msg = new CDO.MessageClass();  
	 CDO.Configuration cfg = new CDO.ConfigurationClass();  
	 msg.Configuration = cfg;
	 msg.CreateMHTMLBody(url, CDO.CdoMHTMLFlags.cdoSuppressAll, id, pwd);  
	 msg.GetStream().SaveToFile(fn, ADODB.SaveOptionsEnum.adSaveCreateOverWrite);  
 }
 #endregion