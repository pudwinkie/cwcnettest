using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// 狀態物件
namespace ChuiWenChiu.Utility {
	[Serializable]
	public class CState {
        #region 靜態成員
        public static void Save(string filename, CState obj){			
			Stream s = File.Open(filename, FileMode.Create);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(s, obj);
			s.Close();						
		}
		
		public static CState Load(string filename){
			Stream s = File.Open(filename, FileMode.Open);
			BinaryFormatter bf = new BinaryFormatter();
			CState b = bf.Deserialize(s) as CState;

			s.Close();
			return b;
        }
        #endregion 

        #region 私有成員
        private string _version;				// 版本代碼
		private string _modify_detail;			// 修改細節
		private string _install_guide;			// 安裝注意事項
		private string _src_path;				// 來源路徑
		private string _dst_path;				// 目的路徑
		private string _modify_list;			// 修改清單
		private string _unencode_list;			// 不編碼清單
		private string _compress_name;			// 壓縮檔名
		private string _map_path;				// 對應路徑
		private int _wm_version;				// WM 版本
		private int _wm_project;				// 專案代碼
		private bool _pack_all;					// 打包整個站台
        #endregion		

        #region 建構子
        /// <summary>
        /// 
        /// </summary>
        public CState(){
			_version = "";			
			_modify_detail = "";		
			_install_guide = "";	
			_src_path = "";			
			_dst_path = "";			
			_modify_list = "";		
			_unencode_list = "";		
			_compress_name = "";		
			_map_path = "";
            _wm_version = -1;
            _wm_project = -1;
            _pack_all = false;
        }
        #endregion

        #region 屬性
        /// <summary>
        /// 
        /// </summary>
        public string Version{
			get{
				return _version;
			}

			set{
				_version = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string ModifyDetail{
			get{
				return _modify_detail;
			}

			set{
				_modify_detail = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string InstallGuide{
			get{
				return _install_guide;
			}

			set{
				_install_guide = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string SourcePath{
			get{
				return _src_path;
			}

			set{
				_src_path = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string DestPath{
			get{
				return _dst_path;
			}

			set{
				_dst_path = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string ModifyList{
			get{
				return _modify_list;
			}

			set{
				_modify_list = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string UnencodeList{
			get{
				return _unencode_list;
			}

			set{
				_unencode_list = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string CompressName{
			get{
				return _compress_name;
			}

			set{
				_compress_name = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string MapPath{
			get{
				return _map_path;
			}

			set{
				_map_path = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public int WMVersion{
			get{
				return _wm_version;
			}

			set{
				_wm_version = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public int WMProject{
			get{
				return _wm_project;
			}

			set{
				_wm_project = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public bool PackAll{
			get{
				return _pack_all;
			}

			set{
				_pack_all = value;
			}
        }
        #endregion 
    }
}