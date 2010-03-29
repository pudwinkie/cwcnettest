using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace ChuiWenChiu {
	class CExtractor{		
		private bool _IsHaveValue = false;
		private string _item = "";
		private string _OrgItem = "";

		const string PATTERN_EXTRACT = @"\[[M|A]{1}].*[\r|\n]{0,1}$";
		const string PATTERN_TAG = @"\[[M|A]{1}]";

		// 項目萃取
		private void ExtractItem(){			
			Regex obj = new Regex( PATTERN_EXTRACT );
			Match mt = null; 			
			mt = obj.Match( _OrgItem );							
			if ( mt.Groups[0].ToString() != "" ){
				_IsHaveValue = true;
				_item = mt.Groups[0].ToString();
			}else{
				_IsHaveValue = false;
			}
		}

		// 移除修改標籤
		private void RemoveTag(){
			if (_IsHaveValue == false){
				return ;
			}
			
			Regex obj = new Regex( PATTERN_TAG );
			_item = obj.Replace(_item, "");			
		}

		public void Extract(string content){
			_OrgItem = content;
			ExtractItem(  );
			RemoveTag( );
		}

		public string Item{
			get{
				return _item;
			}
		}

		public bool IsHaveValue{
			get{
				return _IsHaveValue;
			}
		}
		
	};

    class Program {
        static void Main(string[] args) {				
			StreamReader sr = new StreamReader(@"c:\bugfix.txt", System.Text.Encoding.Default);			
			string content = "";
			CExtractor ext = new CExtractor();
			while( (content = sr.ReadLine()) != null ){			
				ext.Extract( content );
				if (ext.IsHaveValue == true){
					Console.WriteLine(ext.Item);
				}				
			}			
			
        }
    }
}
