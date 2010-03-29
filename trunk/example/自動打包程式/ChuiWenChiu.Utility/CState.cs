using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// ���A����
namespace ChuiWenChiu.Utility {
	[Serializable]
	public class CState {
        #region �R�A����
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

        #region �p������
        private string _version;				// �����N�X
		private string _modify_detail;			// �ק�Ӹ`
		private string _install_guide;			// �w�˪`�N�ƶ�
		private string _src_path;				// �ӷ����|
		private string _dst_path;				// �ت����|
		private string _modify_list;			// �ק�M��
		private string _unencode_list;			// ���s�X�M��
		private string _compress_name;			// ���Y�ɦW
		private string _map_path;				// �������|
		private int _wm_version;				// WM ����
		private int _wm_project;				// �M�ץN�X
		private bool _pack_all;					// ���]��ӯ��x
        #endregion		

        #region �غc�l
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

        #region �ݩ�
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