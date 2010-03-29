using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ChuiWenChiu;
using System.IO;
using ChuiWenChiu.Utility;
using AutoCollect.Properties;
using System.Text.RegularExpressions;
    
namespace AutoCollect {
    /// <summary>
    /// �D���
    /// </summary>
    public partial class FCollect : Form {        
        #region ��Ʀ���
        private CCollect col;
        private CXMLConfig _conf;
        private FConfig frmConfig;
        //private CMail mail;
        CState state = null;
        private System.Diagnostics.Process Proc;

        private int iFail ;
        private int iSuccess;

        private bool isRunning;
        private string _fileList;

        //private Settings state = null;
        private CWMConfig wm = null;
        private CWMVersions wmv = null;
        private CExtractor ext = null;  // �ɮײM��Ѩ�
        #endregion

        #region �`�Ʃw�q
        private string CONFIG_FILE          = @"\config\config.xml";        // �]�w��
        private string SSH_PROGRAM          = @"\plugin\plink.exe";         // �إ� SSH �s�u�u��
        private string TEMPLATE_MAIL_BODY   = @"\template\mail.template";   // �H�󥻤�˪O
        private string TEMPLATE_MAIL_TO     = @"\template\to.template";     // �H��W��˪�
        private string TEMPLATE_README      = @"\template\readme.template"; // readme
        private string TEMPLATE_SOURCE      = @"\template\sourceList.template"; // readme
        private string XML_PROJECT          = @"\config\project.xml";       // �M�׾��]�w��
        private string LIST_SOURCE          = @"\sourceList.txt";           // ���s�X�M��                                              
        private string FILE_VERSION_MEMO    = @"\versionMemo.txt";
        private string FILE_MODIFY_LIST     = @"\modifyList.txt";
        private string FILE_README          = @"\readme.txt";
        private string LIST_FILES           = @"\fileList.txt";
        #endregion

        #region UI �{��
        /// <summary>
        /// �غc�l
        /// </summary>
        public FCollect() {
            iFail       = 0;
            iSuccess    = 0;
            isRunning   = false;
            InitializeComponent();
            frmConfig   =  new FConfig();
            _conf       = new CXMLConfig();
            _conf.load(getAppPath() + CONFIG_FILE); 
            //mail        = null;
            createCollect(null);

            loadTemplate(txtMailBody , string.Format("{0}{1}", getAppPath(), TEMPLATE_MAIL_BODY));
            loadTemplate(txtMailTo, string.Format("{0}{1}", getAppPath(), TEMPLATE_MAIL_TO));
            loadTemplate(txtReadme, string.Format("{0}{1}", getAppPath(), TEMPLATE_README));
            loadTemplate(txtExcepList, string.Format("{0}{1}", getAppPath(), TEMPLATE_SOURCE));
        }

        /// <summary>
        /// ���J�˪O���
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="template"></param> 
        private void loadTemplate(Control obj, string template) {                        
            if (System.IO.File.Exists(template)) {
                obj.Text = file_get_contents(template);  
            }                        
        }
        private void mnuFileOpen_Click(object sender, EventArgs e) {
            stopCollect();
             
            spStatus.Text = tbOpenFile.ToolTipText;
 
            dlgOpen.Filter = "*.txt|*.txt";
            if (dlgOpen.ShowDialog() == DialogResult.OK  ) {
                //MessageBox.Show(dlgOpen.FileName);
                try {
                        
                    sbrFileList.Text = System.IO.Path.GetFileName( dlgOpen.FileName );
                    _fileList = dlgOpen.FileName;
                    createCollect(_fileList);
                    
                    updatePath();
                    txtFileList.Clear(); 
                    col.getList(); 
                } catch (Exception ep) {
                    MessageBox.Show (ep.Message );
                    col = null;
                }
            }
        }

        /// <summary>
        /// �{������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuFilExit_Click(object sender, EventArgs e) {
            stopCollect();
            this.Close(); 
        }
        /// <summary>
        /// ���v�ŧi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuHelpAbout_Click(object sender, EventArgs e) {
            FAboutBox frmAbout = new FAboutBox();
            frmAbout.ShowDialog();
            frmAbout = null;             
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FCollect_FormClosed(object sender, FormClosedEventArgs e) {
            if (Proc != null && Proc.HasExited == false) {
                //Console.WriteLine("�ѥD�{�Ǳj��פ�~���{�Ǫ��B��I");
                Proc.Kill();
            }
            trayicon.Visible = false;
        }
        /// <summary>
        /// �s�����]�n����Ƨ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPublish_click(object sender, EventArgs e) {
            spStatus.Text = tbExplorer.ToolTipText;
            Application.DoEvents();

            string folder = col.DestPath.Replace(@"\\", @"\");
            if (folder.Length > 0 && folder.Substring(folder.Length - 1, 1) == @"\") {
                folder = folder.Substring(0, folder.Length - 1);
            }
            col.DestPath = folder;

            if (!System.IO.Directory.Exists(col.DestPath )) {
                MessageBox.Show("�ت����|���s�b�Ω|�����w"); 
                return;
            }

            System.Diagnostics.Process.Start(@"explorer.exe ",  col.DestPath  + @"\remote\");                  
        }

        /// <summary>
        /// SSH �]�w
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOption_Click(object sender, EventArgs e) {            
            frmConfig.setConfig(_conf);
            frmConfig.ShowDialog(); 
        }
        
        private void FCollect_Click(object sender, EventArgs e) {
            stopCollect();
        }

        /// <summary>
        /// �ɮצ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e) {
            stopCollect();

            // �ˬd�ӷ��ؿ��O�_�s�b
            if (Directory.Exists( col.SourcePath  ) == false ){
                MessageBox.Show("�ӷ��ؿ����s�b", "�T��", MessageBoxButtons.OK);
                return;
            }

            if (col.DestPath.Trim() == "" ){
                MessageBox.Show("�����w�ت����|", "�T��", MessageBoxButtons.OK);
                return;
            }

            if (col != null) {
                spStatus.Text = tbFileCollect.ToolTipText;
                Application.DoEvents();

                this.clear();
                iFail = 0;
                iSuccess = 0;

                try {
                    if (chkAutoClear.Checked == true) {
                        if (System.IO.Directory.Exists(col.DestPath)) {
                            System.IO.Directory.Delete(col.DestPath, true);
                        }
                    }
                    isRunning = true;
                    col.CatchByList(txtFileList.Text);
                    //col.Go();
                    isRunning = false;
                } catch (System.Exception ex) {
                    isRunning = false;
                    col.abort();
                    createCollect(_fileList);
                    MessageBox.Show(ex.Message);
                } finally {

                }
                sbrMessage.Text = "�`�@�G" + (iSuccess + iFail) + " ���\�G" + iSuccess + " ���ѡG" + iFail;
            } else {
                MessageBox.Show("�|�����J�ɮײM��");
            }
        }
        /// <summary>
        /// �ɮץ��]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPack_Click(object sender, EventArgs e) {
            RegexPackName rgx = new RegexPackName();
            if (rgx.isValid(txtCompressName.Text) == false) {
                MessageBox.Show("�ɮפ��X�k");  
            }
            string prefix = txtCompressName.Text;
            if (prefix == "") {
                //prefix = "Untitle";
                MessageBox.Show("�|����J���]�W��", "�߰�", MessageBoxButtons.OK);
                txtCompressName.Focus();  
                return;
            }

            if (!Directory.Exists(col.DestPath)) {
                MessageBox.Show("�ت����|���s�b");
                return;
            }

            // �إ� versionMemo.txt
            file_put_contents(string.Format("{0}{1}", col.DestPath, FILE_VERSION_MEMO), txtVersion.Text);
            // �إ� modifyList.txt                
            file_put_contents(string.Format("{0}{1}", col.DestPath, FILE_MODIFY_LIST), txtModifyDetail.Text);
            // �إ� readme.txt
            file_put_contents(string.Format("{0}{1}", col.DestPath, FILE_README), txtReadme.Text);
            // �إ� sourceList.txt
            file_put_contents(string.Format("{0}{1}", col.DestPath, LIST_SOURCE), txtExcepList.Text);
            // �إ� fileList.txt
            file_put_contents(string.Format("{0}{1}", col.DestPath, LIST_FILES), txtFileList.Text);                        
            stopCollect();

            spStatus.Text = tbPack.ToolTipText;
            Application.DoEvents();
            try {
                if (Proc != null && Proc.HasExited == false) {
                    //Console.WriteLine("�ѥD�{�Ǳj��פ�~���{�Ǫ��B��I");
                    if (MessageBox.Show("�w�g���@��SSH���b����A�A�O�_�n���s�Ұ� SSH �{��", "Tell Me", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        Proc.Kill();
                    } else {
                        return;
                    }
                }

                //�n���@�ӵ{�ǫH����
                System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();

                //�]�m�~���{�ǦW
                //Info.FileName = Application.StartupPath + @"\plugin\putty.exe";
                Info.FileName = getAppPath() + SSH_PROGRAM;

                // 
                if (System.IO.Directory.Exists(col.DestPath + _conf.PackFolder)) {
                    System.IO.Directory.Delete(col.DestPath + _conf.PackFolder, true);
                }

                //�]�m�~���{�Ǫ��ҰʰѼơ]�R�O��Ѽơ^��test.txt
                //Info.Arguments = "-ssh wmenc@192.168.10.20";
                //string prefix = txtCompressName.Text;
                if (prefix == "") {
                    prefix = "Untitle";
                }
                //string commandLine = "-P 22 -l wmenc -pw wmenc.2004 192.168.10.20 \"cd arick;./arick_net3.sh " + prefix + " 192.168.10.120 WM25_ARICK package;\"";
                 
                StringBuilder sb = new StringBuilder();
/*
                sb.AppendFormat(
                    "-P {8} -l {6} -pw {7} {0} \"cd {1} ;./arick_net3.sh {2} {3} {4} {5} ;\"",
                    _conf.EncoderServer,
                    _conf.EncoderFolder,
                    prefix,
                    _conf.SourceServer,
                    _conf.SourceFolder,
                    _conf.ProjectTag,                    
                    _conf.EncoderUsername,
                    _conf.EncoderPassword,
                    _conf.EncoderPort
                );
*/
                sb.AppendFormat(
                    "-P {0} -l {1} -pw {2} {3} \"cd {4} ;./{5} {6} {7} /{8}/{9}/{10} ;\"",
                    _conf.EncoderPort,                  // 0
                    _conf.EncoderUsername,              // 1
                    _conf.EncoderPassword,              // 2
                    _conf.EncoderServer,                // 3
                    _conf.EncoderFolder,                // 4
                    _conf.EncoderShellScript,           // 5
                    prefix,                             // 6
                    wmv[cbWMVersion.SelectedIndex].IP,  // 7
                    wmv[cbWMVersion.SelectedIndex].WorkDir ,  // 8
                    wmv[cbWMVersion.SelectedIndex].Projects[cbWMProject.SelectedIndex].tag,                     // 9
                    txtFolder.Text                      // 10
                );
                string commandLine = sb.ToString();
                //MessageBox.Show(commandLine);
   
                //Console.WriteLine(commandLine);
                Info.Arguments = commandLine;
                Info.UseShellExecute = false;
                Info.RedirectStandardOutput = false;
                //Info.Arguments = "-P 22 -l wmenc -pw wmenc.2004 192.168.10.20 \"cd saly;scp -P 1500 -r wm2@192.168.10.120:/home1/WM25_ARICK/bugfix ./tmp ";
                //�]�m�~���{�Ǥu�@�ؿ�
                Info.WorkingDirectory = @"C:\";

                //
                //�Ұʥ~���{��
                //
                Proc = System.Diagnostics.Process.Start(Info);

                //MessageBox.Show(Proc.Handle.ToString() );

                //�C�L�X�~���{�Ǫ��}�l����ɶ�
                //Console.WriteLine("�~���{�Ǫ��}�l����ɶ��G{0}",  Proc.StartTime);
                //Proc.WaitForExit(10000);
                //txtBadPackList.Text = Proc.StandardOutput.ReadToEnd();
                Proc.WaitForExit();

                sb.Remove(0, sb.Length);
                sb.AppendFormat(@"{0}\{1}\{2}_Encode.md5", col.DestPath, _conf.PackFolder, prefix);
                string md5 = sb.ToString();
                sb.Remove(0, sb.Length);
                sb.AppendFormat(@"{0}\{1}\{2}_Encode.tgz", col.DestPath, _conf.PackFolder, prefix);
                string tgz = sb.ToString();
                sb.Remove(0, sb.Length);
                sb.AppendFormat(@"{0}\{1}\error.log", col.DestPath, _conf.PackFolder);
                string error_log = sb.ToString();
                if (!File.Exists(md5) || !File.Exists(tgz) || !File.Exists(error_log)) {
                    MessageBox.Show("���]����");
                    return;
                } else {
                    MessageBox.Show("���]���\");
                }
                //lstAttatch.Items.Clear();
                lstAttatch.Items.Add(md5);
                lstAttatch.Items.Add(tgz);

                txtBadPackList.Text = file_get_contents(error_log); 

                //lstAttatch.Items.Add();   
            } catch (System.ComponentModel.Win32Exception ep) {
                //Console.WriteLine("�t�Χ䤣����w���{�Ǥ��C\r{0}",  ep);
                MessageBox.Show("�t�Χ䤣����w���{�Ǥ��C\r" + ep);
            } catch (System.Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion
        #region �ۭq���
        /// <summary>
        /// ���o���ε{�����| 
        /// </summary>
        /// <returns></returns>
        private string getAppPath() {
            if (Application.StartupPath.Substring(Application.StartupPath.Length - 1, 1) == @"\") {
                return Application.StartupPath;
            } else {
                StringBuilder sb = new StringBuilder();
                sb.Append(Application.StartupPath);
                sb.Append(@"\");
                return sb.ToString();
            }
        }
        /// <summary>
        /// �M�ŰT���C��
        /// </summary>
        private void clear() {
            lstMessage.Items.Clear();
            lstFail.Items.Clear();
            Application.DoEvents(); 
        }
        /// <summary>
        /// ��ܰT��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        private void display(object sender, string msg) {
            ListBox lstObj = (ListBox)sender;
            lstObj.Items.Add(msg);            
            lstObj.SelectedIndex = lstObj.Items.Count - 1;
            Application.DoEvents();
        }
        /// <summary>
        /// �Ыطs�� CCollect ����
        /// </summary>
        /// <param name="filename"></param>
        private void createCollect(string filename) {
            if (filename == null) {
                col = new CCollect();
            } else {
                //col = new CCollect(_fileList);
                col = new CCollect(filename);
            }
            col.Success     += new SuccessEventHandler(this.onSuccess);
            col.Fail        += new FailEventHandler(this.onFail);
            col.Replace     += new ReplaceEventHandler(this.onReplace);
            col.NoExist     += new NoExistEventHandler(this.onNoExist);
            col.EAbort      += new AbortEventHandler(this.onAbort);
            col.EEnumFile   += new EnumFileEventHandler(this.onEnumFile);

            col.SourcePath  = txtSrcPath.Text;
            col.DestPath = txtDestPath.Text;            
        }
        /// <summary>
        /// �j����ɮצ���
        /// </summary>
        private void stopCollect() {
            if (isRunning == true) {
                if (MessageBox.Show("�A�T�w�n�פ��ɮצ����ܡH", "�߰�", MessageBoxButtons.YesNo) == DialogResult.No) {
                    return;
                }
                col.abort();
                createCollect(_fileList);
            }
        }
        /// <summary>
        /// ���o�ɮפ��e
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string file_get_contents(string filename) {
            StreamReader sw = new StreamReader(filename, Encoding.Default);
            string data = sw.ReadToEnd();
            sw.Close();
            return data;
        }

        /// <summary>
        /// ��m�ɮפ��e
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="content"></param>
        private void file_put_contents(string filename, string content) {
            StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);            
            sw.Write(content); 
            sw.Close();  
        }

        /// <summary>
        /// �P�B��s UI �W�����|
        /// </summary>
        private void updatePath() {

            
            txtSrcPath.Text     = lblSource.Text    = col.SourcePath;
            txtDestPath.Text    = lblDest2.Text =  lblDest.Text      = col.DestPath;
            toolTip1.SetToolTip(lblSource, col.SourcePath);
            toolTip1.SetToolTip(lblDest, col.DestPath);  
        }

        #endregion
        #region �ɮצ����ƥ�B�z���
        public void onAbort() {
            isRunning = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void onFail(CollectEventArgs e) {
            ++iFail;
            this.display(lstFail, "[����] " + e.Message);
        }
        /// <summary>
        /// �ƻs���\�ƥ�B�z�禡 
        /// </summary>
        /// <param name="e"></param>
        public void onSuccess(CollectEventArgs e) {
            ++iSuccess;
            this.display(lstMessage, "[���\] " + e.Message);
        }

        /// <summary>
        /// �ƻs���\�ƥ�B�z�禡 
        /// </summary>
        /// <param name="e"></param>
        public void onEnumFile(CollectEventArgs e) {
            txtFileList.AppendText(e.Message);
            txtFileList.AppendText("\n");
            Application.DoEvents(); 
        }

        public void onReplace(CollectEventArgs e) {
            ++iSuccess;
            this.display(lstMessage, "[�л\] " + e.Message);
        }

        public void onNoExist(CollectEventArgs e) {
            //this.display("[���s�b] " + e.Message);
            ++iFail;
            this.display(lstFail, "[���s�b] " + e.Message);
        }

 
        #endregion

        private void btnBrowseSrc_Click(object sender, EventArgs e) {
            if (dlgFolder.ShowDialog() != DialogResult.Cancel) {
                txtSrcPath.Text = dlgFolder.SelectedPath;
            }
        }

        private void btnBrowseDest_Click(object sender, EventArgs e) {
            if (dlgFolder.ShowDialog() != DialogResult.Cancel) {
                txtDestPath.Text = dlgFolder.SelectedPath;
            }
        }

        /// <summary>
        /// �H�e mail 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendMail_Click(object sender, EventArgs e) {             
            Mapi ma = new Mapi();
            Regex reg = new Regex(";");
            string[] toList = reg.Split(txtMailTo.Text);
            if (toList.Length <= 0) {
                MessageBox.Show("�|�����w���H��̩m�W");
                return;
            }
            for (int i = 0; i < toList.Length; ++i) {
                ma.AddRecip(toList[i], toList[i], false);
            }

            for (int j = 0; j < lstAttatch.Items.Count; ++j) {
                if (File.Exists(lstAttatch.Items[j].ToString()) ) {
                    ma.Attach( lstAttatch.Items[j].ToString()) ;
                }
            }
            ma.SetSender(txtMailFrom.Text, txtMailFrom.Text);           
            ma.Send(txtMailSubject.Text, txtMailBody.Text );            

/*
 *  SMTP
            try {
                if (mail == null) {
                    mail = new CMail();
                }

                mail.From       = txtFrom.Text;
                mail.To         = txtTo.Text;
                mail.SMTPServer = _conf.SMTPServer;// "email.sun.net.tw";
                mail.Subject    = txtSubject.Text;
                mail.Body       = txtMailBody.Text;
                //mail.addFile(@"c:\ScanCDROM.rar");
                mail.HTMLFormat = chkHTML.Checked ;
                mail.send();
                MessageBox.Show("�o�e����");  
            } catch (Exception ex) {
                MessageBox.Show("Exception: " + ex.Message ); 
            }
 */ 
        }
        /// <summary>
        /// �ӷ����|�ܧ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSrcPath_TextChanged(object sender, EventArgs e)
        {            
            col.SourcePath = txtSrcPath.Text;
            updatePath();
        }
        /// <summary>
        /// �ت����|�ܧ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDestPath_TextChanged(object sender, EventArgs e)
        {
            col.DestPath = txtDestPath.Text;
            updatePath();
        }

        /// <summary>
        /// ���U Delete �ɧR�����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstAttatch_KeyDown(object sender, KeyEventArgs e) {
            // �R��������ɮ�
            if (e.KeyCode == Keys.Delete) {
                AttachRemove();
            }
        }

        /// <summary>
        /// �s�W�q�l�l�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAttachment_Click(object sender, EventArgs e) {
            dlgOpen.Multiselect = true;   
            if (dlgOpen.ShowDialog() != DialogResult.Cancel) {
                //dlgOpen.FileNames//
  
                lstAttatch.Items.AddRange( dlgOpen.FileNames);
            }
        }

        /// <summary>
        /// �����J��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FCollect_Load(object sender, EventArgs e) {
            wm = new CWMConfig();
            wm.Load(string.Format(@"{0}{1}", getAppPath(), XML_PROJECT) );
            wmv = wm.GetWMs();
            //int index = 0;
            for (int i = 0; i < wmv.Count; ++i) {
                cbWMVersion.Items.Add(wmv[i].ID);
            }
            if (wmv.Count > 0) {
                cbWMVersion.SelectedIndex = 0;
            }
             
            state = new CState(); 
//            state = new Settings();
//            txtSrcPath.Text = state.SourcePath;
//            txtDestPath.Text = state.DestPath;  
        }


        /// <summary>
        /// WM �����ܧ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWMVersion_SelectedIndexChanged_1(object sender, EventArgs e) {
            if (cbWMVersion.SelectedIndex > -1) {
                cbWMProject.Items.Clear();
                for (int j = 0; j < wmv[cbWMVersion.SelectedIndex].Projects.Count; ++j) {
                    cbWMProject.Items.Add(wmv[cbWMVersion.SelectedIndex].Projects[j].tag);
                }

                if (cbWMProject.Items.Count > 0) {
                    cbWMProject.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// �M�׿���ܧ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWMProject_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbWMVersion.SelectedIndex == -1 || cbWMProject.SelectedIndex == -1) {
                return;
            }

            toolTip1.SetToolTip(cbWMProject, wmv[cbWMVersion.SelectedIndex].Projects[cbWMProject.SelectedIndex].name);             
        }

        /// <summary>
        /// �ɮ׵Ѩ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExtractList_Click(object sender, EventArgs e) {
            if (ext == null) {
                ext = new CExtractor();
            }

            foreach (string line in txtModifyDetail.Lines) {
                ext.Extract(line);
                if (ext.IsHaveValue == true) {
                    txtFileList.AppendText(ext.Item);
                    txtFileList.AppendText("\n");                    
                }				

            }
            MessageBox.Show("�Ѩ�����");  			
        }

        /// <summary>
        /// �}�Ҫ��A
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuFileOpenState_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            
            ofd.Filter = "�պA��(*.xfg)|*.xfg";
            if (DialogResult.OK  == ofd.ShowDialog()) {
                state = CState.Load(ofd.FileName);
                createCollect(null);
                col.SourcePath = state.SourcePath;
                col.DestPath = state.DestPath;
                updatePath(); 
                //txtSrcPath.Text = state.SourcePath;
                //txtDestPath.Text = state.DestPath;
                txtCompressName.Text = state.CompressName;
                txtReadme.Text = state.InstallGuide;
                txtFolder.Text = state.MapPath;
                txtModifyDetail.Text = state.ModifyDetail;
                txtFileList.Text = state.ModifyList;
                chkPackAll.Checked = state.PackAll;
                txtExcepList.Text = state.UnencodeList;
                txtVersion.Text = state.Version;
                cbWMProject.SelectedIndex = state.WMProject;
                cbWMVersion.SelectedIndex = state.WMVersion;
            }
        }

        /// <summary>
        /// �x�s���A
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuFileSaveState_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "�պA��(*.xfg)|*.xfg";

            if (DialogResult.OK == sfd.ShowDialog()) {
                state.SourcePath    = txtSrcPath.Text;
                state.DestPath      = txtDestPath.Text;
                state.CompressName  = txtCompressName.Text;
                state.InstallGuide  = txtReadme.Text;
                state.MapPath       = txtFolder.Text;
                state.ModifyDetail  = txtModifyDetail.Text;
                state.ModifyList    = txtFileList.Text;
                state.PackAll       = chkPackAll.Checked;
                state.UnencodeList  = txtExcepList.Text;
                state.Version       = txtVersion.Text;
                state.WMProject     = cbWMProject.SelectedIndex;
                state.WMVersion     = cbWMVersion.SelectedIndex;  
 
                CState.Save(sfd.FileName, state);                  
            }
        }

        /// <summary>
        /// �M�׳]�w
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuProjectManager_Click(object sender, EventArgs e) {
            FProjectManager pm = new FProjectManager();
            pm.SetWM( wmv ); 
            pm.ShowDialog();
            pm = null;
        }

        /// <summary>
        /// ��ܪ��󪺥\����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstAttatch_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                mnuAttach.Show(lstAttatch, e.X, e.Y);
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAttachRemove_Click(object sender, EventArgs e) {
            AttachRemove();
        }

        /// <summary>
        /// �����l������������
        /// </summary>
        private void AttachRemove() {
            int total = lstAttatch.SelectedItems.Count;
            for (int i = total - 1; i >= 0; --i) {
                lstAttatch.Items.Remove(lstAttatch.SelectedItems[i]);
            }
        }
        /// <summary>
        /// ���󶵥إ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAttachSelectAll_Click(object sender, EventArgs e) {
            for (int i = 0; i < lstAttatch.Items.Count; ++i) {
                lstAttatch.SetSelected(i, true);
            }
        }

        private void txtVersion_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                System.Windows.Forms.SendKeys.Send("\t");   
            }
        }

        private void mnuEdit_MouseDown(object sender, MouseEventArgs e) {
            TextBox txt = this.ActiveControl as TextBox;
            if (txt == null || txt.SelectionLength <= 0) {
                mnuEditCopy.Enabled = false;
                mnuEditCut.Enabled = false;
                mnuEditDel.Enabled = false;
            } else {
                mnuEditCopy.Enabled = true;
                mnuEditCut.Enabled = true;
                mnuEditDel.Enabled = true;
            }
        }

        private void mnuEditCut_Click(object sender, EventArgs e) {
            TextBox txt = this.ActiveControl as TextBox;
            if (txt != null) {
                txt.Cut();
            }
        }

        private void mnuEditCopy_Click(object sender, EventArgs e) {
            TextBox txt = this.ActiveControl as TextBox;
            if (txt != null) {
                txt.Copy();
            }
        }

        private void mnuEditPaste_Click(object sender, EventArgs e) {
            TextBox txt = this.ActiveControl as TextBox;
            if (txt != null) {
                txt.Paste();
            }
        }

        private void mnuEditSelectAll_Click(object sender, EventArgs e) {
            TextBox txt = this.ActiveControl as TextBox;
            if (txt != null) {
                txt.SelectAll(); 
            }
        }

        private void mnuEditDel_Click(object sender, EventArgs e) {
            TextBox txt = this.ActiveControl as TextBox;
            if (txt != null) {
                txt.SelectedText = ""; 
                //txt.Clear(); 
            }
        }

        private void mnuEditUndo_Click(object sender, EventArgs e) {
            TextBox txt = this.ActiveControl as TextBox;
            if (txt != null) {
                txt.Undo();
            }
        }

        private void mnuEdit_DropDownOpening(object sender, EventArgs e) {
            TextBox txt = this.ActiveControl as TextBox;
            if (txt == null || txt.SelectionLength <= 0) {
                mnuEditCopy.Enabled = false;
                mnuEditCut.Enabled = false;
                mnuEditDel.Enabled = false;
            } else {
                mnuEditCopy.Enabled = true;
                mnuEditCut.Enabled = true;
                mnuEditDel.Enabled = true;
            }
             
            if (txt != null && Clipboard.GetDataObject().GetDataPresent(typeof(string))) {
                mnuEditPaste.Enabled = true; 
            } else {
                mnuEditPaste.Enabled = false; 
            }

            if (txt != null && txt.CanUndo == true ) {
                mnuEditUndo.Enabled = true;
            } else {
                mnuEditUndo.Enabled = false;
            }

            if (txt != null && txt.CanSelect == true) {
                mnuEditSelectAll.Enabled = true;
            } else {
                mnuEditSelectAll.Enabled = false;
            }
        }

        private void txtModifyDetail_DragDrop(object sender, DragEventArgs e) {
            TextBox txt = (TextBox)sender;

            if (e.Data.GetDataPresent(DataFormats.FileDrop)  ){
                string[] astr = (string[])e.Data.GetData(DataFormats.FileDrop);
                StreamReader sr = new StreamReader(astr[0], Encoding.Default  );
                txt.SelectedText = sr.ReadToEnd();
                sr.Close(); 
            } else if (e.Data.GetDataPresent(DataFormats.StringFormat)) {
                txt.SelectedText = (string)e.Data.GetData(DataFormats.StringFormat);   
            }
        }

        private void txtModifyDetail_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) || e.Data.GetDataPresent(DataFormats.FileDrop)) {
                if ((e.AllowedEffect & DragDropEffects.Move) != 0) {
                    e.Effect = DragDropEffects.Move;
                }

                if (((e.AllowedEffect & DragDropEffects.Copy) != 0) &&
                     ((e.KeyState & 0x08) != 0)
                    ) {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

    }
}