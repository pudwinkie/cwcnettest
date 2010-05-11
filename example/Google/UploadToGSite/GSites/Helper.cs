using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GSites {
    public class Helper {
        static public String APP_NAME = "google-SitesAPIDemo-v1.2";
        static public String STORE_PATH = "2B6EAF674D7D436AB12094990500A102";

        public static string GetMimeType(string fileName) {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public static bool IsValidGSiteUrl(String url, ref String domain, ref String site_name) {
            Regex reg = new Regex(@"sites\.google\.com\/(\w*)\/(\w*)[\/]{0,1}", RegexOptions.Compiled);
            Match match = reg.Match(url);
            if (match.Groups.Count != 3) {                
                return false;
            }

            domain = match.Groups[1].Value;
            site_name = match.Groups[2].Value;
            return true;
        }
    }
}
