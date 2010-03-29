/**
 * �ɮײM��Ѩ�
 * 
 * ���Ī����Ү榡��
 * [A] xxxxx
 * [M] xxxxx
 * 
 **/ 
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace ChuiWenChiu.Utility{
	public class CExtractor{		
		private bool _IsHaveValue = false;
		private string _item = "";
		private string _OrgItem = "";
        private Regex rgxExtract;
        private Regex rgxRemoveTag;

		const string PATTERN_EXTRACT = @"\[[M|A]{1}].*[\r|\n]{0,1}$";
		const string PATTERN_TAG = @"\[[M|A]{1}]";
        /// <summary>
        /// �غc�l
        /// </summary>
        public CExtractor() {
            rgxExtract = new Regex(PATTERN_EXTRACT);
            rgxRemoveTag = new Regex(PATTERN_TAG);
        }	

        /// <summary>
        /// ���صѨ� 
        /// </summary>
		private void ExtractItem(){              
			Match mt = null; 			
			mt = rgxExtract.Match( _OrgItem );							
			if ( mt.Groups[0].ToString() != "" ){
				_IsHaveValue = true;
				_item = mt.Groups[0].ToString();
			}else{
				_IsHaveValue = false;
			}
		}		
        /// <summary>
        /// �����ק���� 
        /// </summary>
		private void RemoveTag(){
			if (_IsHaveValue == false){
				return ;
			}		
			
			_item = rgxRemoveTag.Replace(_item, "");
            _item = _item.Trim(); 
		}
        
        /// <summary>
        /// ���e�Ѩ�
        /// </summary>
        /// <param name="content">�ӷ����</param>
		public void Extract(string content){
			_OrgItem = content;
			ExtractItem(  );
			RemoveTag( );
            
		}
        /// <summary>
        /// ���o�Ѩ�����
        /// </summary>
		public string Item{
			get{
				return _item;
			}
		}
        /// <summary>
        /// �O�_����
        /// </summary>
		public bool IsHaveValue{
			get{
				return _IsHaveValue;
			}
		}
		
	};
/*
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
*/
}
