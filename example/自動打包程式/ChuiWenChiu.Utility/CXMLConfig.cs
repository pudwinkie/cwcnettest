using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ChuiWenChiu.Utility {
    public class CXMLConfig {
        #region 常數定義
        // 標籤定義
        private const string XML_SOURCE_SERVER      = "source-server";
        private const string XML_SOURCE_FOLDER      = "source-folder";
        private const string XML_PROJECT_TAG        = "project-tag";
        private const string XML_ENCODER_SERVER     = "encoder-server";
        private const string XML_ENCODER_FOLDER     = "encoder-folder";
        private const string XML_ENCODER_PORT       = "encoder-port";
　      private const string XML_ENCODER_USERNAME   = "encoder-username";
        private const string XML_ENCODER_PASSWORD   = "encoder-password";
        private const string XML_SHELL_SCRIPT       = "shell-script";
        private const string XML_PACK_FOLDER        = "pack-folder";
        private const string XML_SMTP_SERVER        = "smtp-server";
        #endregion
        #region 私有資料成員
        private XmlDocument _doc;
        private string _filename;
        #endregion
        #region 建構子
        /// <summary>
        /// 建構子
        /// </summary>
        public CXMLConfig() {
            init();
        }

        public CXMLConfig(string filename) {
            init();
            _filename = filename;
            load(_filename);
        }
        #endregion
        #region 公開方法
        /// <summary>
        /// 儎入設定檔
        /// </summary>
        /// <param name="config"></param>
        public void load(string config) {
            _filename = config;
            _doc.Load(config);
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        public void save() {
            _doc.Save(_filename); 
        }

        /// <summary>
        /// 另存新檔
        /// </summary>
        /// <param name="filename"></param>
        public void save(string filename) {
            _filename = filename;
            save();
        }
        #endregion
        #region 屬性
        public string PackFolder {
            get {
                return getText(XML_PACK_FOLDER);
            }

            set {
                setText(XML_PACK_FOLDER, value);
            }
        }
        public string SourceFolder {
            get {
                return getText(XML_SOURCE_FOLDER);
            }

            set {
                setText(XML_SOURCE_FOLDER, value);
            }
        }

        public string EncoderServer {
            get {
                return getText(XML_ENCODER_SERVER);
            }

            set {
                setText(XML_ENCODER_SERVER, value);
            }
        }

        public string EncoderFolder {
            get {
                return getText(XML_ENCODER_FOLDER);
            }

            set {
                setText(XML_ENCODER_FOLDER, value);
            }
        }

        public string ProjectTag {
            get {
                return getText(XML_PROJECT_TAG);
            }

            set {
                setText(XML_PROJECT_TAG, value);
            }
        }
        public string SMTPServer {
            get {
                return getText(XML_SMTP_SERVER);
            }

            set {
                setText(XML_SMTP_SERVER, value);
            }
        }
        public string SourceServer {
            get {
                return getText(XML_SOURCE_SERVER);
            }

            set {
                setText(XML_SOURCE_SERVER, value);  
            }
        }
        public string EncoderPort {
            get {
                return getText(XML_ENCODER_PORT);
            }

            set {
                setText(XML_ENCODER_PORT, value);
            }
        }
        public string EncoderUsername {
            get {
                return getText(XML_ENCODER_USERNAME);
            }

            set {
                setText(XML_ENCODER_USERNAME, value);
            }
        }
        public string EncoderPassword {
            get {
                return getText(XML_ENCODER_PASSWORD);
            }

            set {
                setText(XML_ENCODER_PASSWORD, value);
            }
        }
        public string EncoderShellScript {
            get {
                return getText(XML_SHELL_SCRIPT);
            }

            set {
                setText(XML_SHELL_SCRIPT, value);
            }
        }

        #endregion
        #region 私有方法
        /// <summary>
        /// 取得節點文字
        /// </summary>
        /// <param name="tagName">節點名稱</param>
        /// <returns>節點文字</returns>
        private string getText(string tagName) {
            if (_filename == null){
                throw new ApplicationException("尚未呼叫load");
            }
            return _doc.DocumentElement.SelectSingleNode(tagName).InnerText;
        }

        /// <summary>
        /// 設定節點的文字
        /// </summary>
        /// <param name="tagName">節點名稱</param>
        /// <param name="value">節點文字</param>
        private void setText(string tagName, string value) {
            _doc.DocumentElement.SelectSingleNode(tagName).InnerText = value;
        }
        private void init(){
            _doc = new XmlDocument();
            _filename = null;
        }
        #endregion
    }
}
