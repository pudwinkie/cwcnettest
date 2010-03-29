using System;
using System.Collections;
using System.Xml;

namespace ChuiWenChiu.Utility {
    public class CWMConfig : XmlDocument {
        #region �غc�l
        /// <summary>
        /// 
        /// </summary>
        public CWMConfig()
            : base() {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imp"></param>
        public CWMConfig(XmlImplementation imp)
            : base(imp) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tbl"></param>
        public CWMConfig(XmlNameTable tbl)
            : base(tbl) {
        }
        #endregion
        /// <summary>
        /// ���o WM �]�w
        /// </summary>
        /// <returns></returns>
        public CWMVersions GetWMs() {
            XmlNodeList nl = this.SelectNodes(@"/wm/version");
            CWMVersions wv = new CWMVersions();
            CWMProjects wmp = null;

            for (int i = 0; i < nl.Count; ++i) {
                XmlNodeList n2 = nl[i].SelectNodes("projects/project");
                wmp = new CWMProjects();
                for (int j = 0; j < n2.Count; ++j) {
                    wmp.Add(new CWMProject(
                        n2[j].SelectSingleNode("name").InnerText,
                        n2[j].SelectSingleNode("tag").InnerText)
                    );
                }

                wv.Add(new CWMVersion(nl.Item(i).SelectSingleNode("id").InnerText,
                    nl.Item(i).SelectSingleNode("ip").InnerText,
                    nl.Item(i).SelectSingleNode("work_dir").InnerText,
                    wmp
                    ));
            }

            return wv;
        }

        #region �p����k
        /// <summary>
        /// ���o�`�I��r
        /// </summary>
        /// <param name="tagName">�`�I�W��</param>
        /// <returns>�`�I��r</returns>
        protected string getText(string tagName) {
            return this.DocumentElement.SelectSingleNode(tagName).InnerText;
        }

        /// <summary>
        /// �]�w�`�I����r
        /// </summary>
        /// <param name="tagName">�`�I�W��</param>
        /// <param name="value">�`�I��r</param>
        protected void setText(string tagName, string value) {
            this.DocumentElement.SelectSingleNode(tagName).InnerText = value;
        }
        #endregion
    }
}
