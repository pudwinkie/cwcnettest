using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
namespace WebServices {
    class Program {
        static void Main(string[] args) {
            Go2Tw cv = new Go2Tw();
            cv.Convert("www.google.com.tw");
        }
    }

    public class Go2Tw {
        private static HttpWebRequest CreateRequest() {
            HttpWebRequest hwp = HttpWebRequest.Create("http://go2.tw/tw/process/create.php") as HttpWebRequest;
            hwp.KeepAlive = false;
            hwp.Method = "POST";
            hwp.ContentType = "application/x-www-form-urlencoded";
            hwp.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 1.1.4322; .NET CLR 3.0.04506.30)";
            return hwp;
        }

        public String Convert(String url) {
            HttpWebRequest hwp = CreateRequest();
            using (StreamWriter writer = new StreamWriter(hwp.GetRequestStream())) {
                writer.Write("destination=");
                writer.Write( HttpUtility.UrlEncode( url ));
            }

            HttpWebResponse response = null;
            String newUrl = String.Empty;
            try {
                response = (HttpWebResponse)hwp.GetResponse();
                
                using (StreamReader sr = new StreamReader(response.GetResponseStream())) {
                    String s = sr.ReadToEnd(); 
                    //while(!sr.EndOfStream){
                    //    String line = sr.ReadLine();
                    //if (line.Contains("textfield")) {
                    //    Match m = Regex.Match(line, "(http://go2.tw/[0-9a-zA-Z]*)");
                    //}
                    //string realUrl = String.Format("http://www.badongo.com/cn/file/{0}?rs=getFileLink&rst=&rsrnd=1160560943040&rsargs[]=0", m.Groups[2].Value);                                                                     
                    //}
                }
                
            } catch (WebException e) {

                //if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Unauthorized) {
                //    throw new Unauthorized();
                //}
            }

            return newUrl;
        }
    }
}
