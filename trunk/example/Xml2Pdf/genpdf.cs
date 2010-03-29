/**
 * XML �� PDF
 *
 * ���ҡG
 *	[1] Windows 2000 Professional
 *	[2] EditPlus v2.12
 *	[3] .Net Framework 1.1 + .Net Framework 2.0
 *	[4] XSL Formatter v3.4 
 *  [5] MSXML Parser 3.0
 *
 * �ɦW�Ggenpdf.cs
 *
 * �sĶ�G
 *	csc genpdf.cs /r:XfoDotNetCtl11.dll
 *
 * �ѦҸ�ơG
 *  [1] Professional XML 2nd, wrox
 *
 * �]�p�̡GChui-Wen Chiu(Arick)
 *
 * ��x�G
 *	2006/02/08 �إ�
 **/
using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl ;
using XfoDotNetCtl;

namespace ChuiWenChiu{
	public class program{
		const string fo_filename = "tmp.fo";
		static public void Main(string[] args){
			if (args.Length < 3){
				Console.WriteLine("usage: genpdf xml-file xsl-file pdf-file");
				return;
			}
			
			XfoObj.Initialize();
			XfoObj obj = null;
			try	{	
				// 1. ���J XML ���
				XPathDocument xmlDoc = new XPathDocument( args[0] ); 

				// 2. ���J XSL ���
				XslCompiledTransform xslDoc = new XslCompiledTransform();
				xslDoc.Load(args[1]); 

				// 3. XML �� XSL-FO
				XmlTextWriter xmlfo = new XmlTextWriter(fo_filename, null); 
				xslDoc.Transform(xmlDoc, null, xmlfo);

				// 4. XSL-FO �� PDF
				obj					= new XfoObj();
				obj.DocumentURI		= fo_filename;
				obj.OutputFilePath	= args[2];
				obj.ExitLevel		= 4;
				obj.Execute();

				// 5. ����ഫ�᪺���~�T��
				ArrayList errList = new ArrayList();
				obj.GetFormattingError(errList);
				for (int i = 0; i < errList.Count; i++)			{
					XfoErrorInformation ei = (XfoErrorInformation)errList[i];
					Console.WriteLine("ErrorLevel : " + ei.ErrorLevel + "\nErrorCode  : " + 
					ei.ErrorCode + "\n" + ei.ErrorMessage);
				}
				
				Console.WriteLine("\nFormatting finished: '" + args[2] + "' created.");
			}
			catch(XfoException e){
				Console.WriteLine("ErrorLevel : " + e.ErrorLevel + "\nErrorCode : " + e.ErrorCode + "\n" + e.Message);
			}catch(Exception e)	{
				Console.WriteLine(e.Message);
			}finally{
				if (obj != null)
					obj.Dispose();
				XfoObj.Terminate();
			}
		}
	}
}
