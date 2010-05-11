using System;
using System.Collections.Generic;
using System.Text;
//using EASendMail;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using System.IO;

using Chilkat;


namespace ConsoleApplication1 {
    class Program {
        static void Main(string[] args) {            
            if (args.Length == 3) {
                GmailSMTP gs = new GmailSMTP(args[0], args[1]);
                gs.SendFolder(new DirectoryInfo(args[2]));
            }
        }
    }


    public class GmailSMTP {
        private SmtpClient client = new SmtpClient();
        private String m_username;
        /// <summary>
        /// GMail 帳密
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public GmailSMTP(String username, String password) {            
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;
            //client.Timeout = 60000;
            m_username = username;
        }

        public bool SendMail(MailMessage mail) {
            try {
                mail.To.Add(new MailAddress(m_username));
                client.Send(mail);
            } catch (Exception e) {
                Console.WriteLine("\t fail:{0}", e.Message);
                return false;
            }

            return true;
        }

        public bool SendMail(String from, String to, String subject, String content) {
            System.Net.Mail.MailAddress fromMail = new MailAddress(from, from, Encoding.UTF8);
            MailMessage mail = new MailMessage(fromMail, new MailAddress(to));
            
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;

            //string body = "Test Body";
            mail.Body = content;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = false;
            mail.Priority = MailPriority.High;
            
            return SendMail(mail);
        }

        public bool SendEml(String eml) {
            Console.WriteLine(eml);
            if (!File.Exists(eml)){
                Console.WriteLine("\tfail: file not exist");
                return false;
            }
            String eml_name = new FileInfo(eml).Name;

            Chilkat.Email email = null;
            try {
                email = new Chilkat.Email();
            } catch (Exception ex) {
                Console.WriteLine("\tLoad Fail: {0}", ex.Message);
            }
            //String eml = eml;
            bool success = email.LoadEml(eml);
            if (!success) {
                Console.WriteLine("\tfail: Load Fail");
            }

            MailMessage mail = new MailMessage();
            // 內嵌影像
            if (email.NumRelatedItems > 0) {
                // 未實作
                Console.WriteLine("\tfail: not process embed image");
                File.Move(eml, Path.ChangeExtension(eml, "embed"));
                return false;
            }

            mail.From = new MailAddress(m_username);
            mail.IsBodyHtml = email.HasHtmlBody();
            try {
                mail.SubjectEncoding = Encoding.GetEncoding(email.Charset);
            } catch (ArgumentException) {

            }
            if (mail.IsBodyHtml) {
                mail.Body = email.GetHtmlBody();
            } else {
                mail.Body = email.Body;
            }

            mail.Subject = email.Subject;
            //StringArray sa = email.GetLinkedDomains();
            if (email.NumAttachments > 0) {
                String folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                DirectoryInfo di = Directory.CreateDirectory(folder);
                for (int i = 0; i < email.NumRelatedItems; ++i) {
                    email.SaveRelatedItem(i, di.FullName);
                }

                //email.SaveRelatedItem                
                if (email.SaveAllAttachments(folder)) {
                    //DirectoryInfo di = new DirectoryInfo(folder);
                    foreach (FileInfo fi in di.GetFiles()) {
                        mail.Attachments.Add(new Attachment(fi.FullName));
                    }
                }
            }
            Console.WriteLine("\tSending...");
            if (SendMail(mail)){
                try {
                    File.Delete(eml);
                } catch (Exception ex) {
                    Console.WriteLine("\tNot Delete: {0}", ex.Message);
                    File.Move(eml, Path.ChangeExtension(eml, "upload"));
                }
                Console.WriteLine("\tSuccess");
                return true;
            }else{
                Console.WriteLine("\tfail");
                File.Move(eml, Path.ChangeExtension(eml, "fail"));
                return false;
            }
        }

        public bool SendFolder(DirectoryInfo folder) {
            foreach (FileInfo fi in folder.GetFiles()) {
                if (Path.GetExtension(fi.FullName) == ".eml" && fi.Length < 19*1024*1024) {
                    SendEml(fi.FullName);
                }
            }

            return true;
        }
    }

    //public class EndOfEmlStrategy : IEndCriteriaStrategy {
    //    #region IEndCriteriaStrategy Members

    //    public bool IsEndReached(char[] data, int pos) {
    //        int previous, current;
            
    //        if (pos > 4) {
    //            int len = data.Length;
    //            if (data[pos-3] == 13 && data[pos-2] == 10 && data[pos-1] == 13 && data[pos] == 10) {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    #endregion
    //}
}
