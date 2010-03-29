using System;
//using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Collections;
//using System.Windows.Forms;
  
namespace ChuiWenChiu {
    class CMail {
        private string _SMTP_Server;    // 寄件伺服器
        private string _body;           // 信件本文
        private string _subject;        // 信件標題
        private string _to;
        private string _from;
        private bool _HTML_Format;
        private ArrayList _fromList;    // 收件者清單
        private ArrayList _toList;      // 寄件者清單
        private ArrayList _fileList;    // 夾檔清單
        public CMail() {
            _fileList = new ArrayList();
            _fromList = new ArrayList();
            _toList = new ArrayList();

            _from = "";
            _to = "";
            _subject = "";
            _body = "";
            _SMTP_Server = "";

            _HTML_Format = false;
        }
        public bool send() {            
            // Create a message and set up the recipients.

            MailMessage message = new MailMessage(
               this.From,
               this.To,
               this.Subject,
               this.Body 
            );            

            // Create  the file attachment for this e-mail message.
            string file;
            Attachment data;
            System.Net.Mime.ContentDisposition disposition;

            for (int i = 0; i < _fileList.Count; ++i) {
                file = (string)_fileList[i];
                if (System.IO.File.Exists(file) == true) {
                    data = new Attachment(file, System.Net.Mime.MediaTypeNames.Application.Octet);
                    disposition = data.ContentDisposition;

                    // Add time stamp information for the file.
                    disposition.CreationDate = System.IO.File.GetCreationTime(file);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(file);

                    // Add the file attachment to this e-mail message.
                    message.Attachments.Add(data);
                }
            }           
            
            message.BodyEncoding = System.Text.Encoding.Default;
            message.SubjectEncoding = System.Text.Encoding.Default;
            message.IsBodyHtml = HTMLFormat; 
            //Send the message.
            SmtpClient client = new SmtpClient( SMTPServer );

            // Add credentials if the SMTP server requires them.
            client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            client.Send(message);

            return true;
        }

        public bool HTMLFormat {
            get {
                return _HTML_Format;
            }

            set {
                _HTML_Format = value;
            }
        }
        public void addFile(string filename) {
            _fileList.Add(filename); 
        }
        public void addSender(string username) {

        }

        public void addReceiver(string mail) {

        }

        public string From {
            get {
                return _from;
            }

            set {
                _from = value;
            }
        }
        public string To {
            get {
                return _to;
            }
            set {
                _to = value;
            }
        }
        public string SMTPServer {
            get {
                return _SMTP_Server; 
            }

            set {
                _SMTP_Server = value;
            }
        }

        public string Body {
            get {
                return _body;
            }

            set {
                _body = value;
            }
        }

        public string Subject {
            get {
                return _subject;
            }

            set {
                _subject = value;
            }
        }
    }
}
