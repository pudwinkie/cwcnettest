using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Linq.Expressions;
using System.Xml.Linq;
using Google.GData.Client;
using Google.GData.Documents.GSites;
using Google.GData.Extensions;
using System.Xml;
using System.Text.RegularExpressions;
using GSites;

namespace ConsoleApplication1
{
    class Program
    {


        static void Main(string[] args)
        {
            String id = args[0];
            String pwd = args[1];
            String src_path = args[2];
            string url = args[3];
            
            
            //Regex reg = new Regex(@"sites\.google\.com\/(.*)\/(.*)\/", RegexOptions.Compiled);
            //Match match = reg.Match(url);
            //if (match.Groups.Count != 3) {
              //  Console.WriteLine("Error");
                //return;
            //}
            String domain = String.Empty;// = match.Groups[1].Value;
            String site_name = String.Empty;// = match.Groups[2].Value;
            if (!Helper.IsValidGSiteUrl(url, ref domain, ref site_name)) {
                Console.WriteLine("Error");
                return;
            }

            var service = new GSiteService(Helper.APP_NAME, domain, site_name);
            service.setUserCredentials(id, pwd);
            var p_feed = service.GetContentFeedPath("/" + Helper.STORE_PATH);
            AtomEntry parent_node = null;
            if (p_feed.Entries.Count == 0) {
                parent_node = service.CreateWebPage(Helper.STORE_PATH, "x", Helper.STORE_PATH);
            } else {
                parent_node = p_feed.Entries[0];
            }
            var file_list = from f in Directory.EnumerateFiles(src_path)
                         select f;

            foreach (var filename in file_list) {
                //var filename = "d:\\4b209d15ba08f.gif";
                var fi = new FileInfo(filename);
                var fn = Guid.NewGuid().ToString().Replace("-", "") + fi.Extension;
                var mime_type = Helper.GetMimeType(filename);
                var result = service.UpdloadAttachment(filename, mime_type, parent_node, fn, "");
                Console.WriteLine("{0}=>{1}", filename, result.Content.Src);
            }

            //printContentFeed(p_feed);

            //AtomFeed feed = service.GetContentFeed();
            //AtomFeed act = service.GetActivityFeed();
            //foreach (AtomEntry entry in act.Entries) {
            //    Console.WriteLine(String.Format("Page: {0}", entry.Title.Text));

            //    String actionType = getCategoryLabel(entry.Categories);
            //    Console.WriteLine(String.Format("  {0} on {1}, by {2}", actionType,
            //        entry.Updated.ToShortDateString(), entry.Authors[0].Email));
            //}

            //AtomFeed rev = service.GetRevisionFeed();
            //foreach (AtomEntry entry in rev.Entries) {
            //    XmlExtension revisionNum = (XmlExtension)entry.FindExtension("revision", GSiteService.SITES_NAMESPACE);
            //    Console.WriteLine(String.Format("revision id: {0}", revisionNum.Node.InnerText));
            //    Console.WriteLine(String.Format("  html: {0}...", entry.Content.Content.Substring(0, 100)));
            //    Console.WriteLine(String.Format("  site link: {0}", entry.AlternateUri.ToString()));
            //}


            //Console.ReadKey();
        }

        public static String getCategoryLabel(AtomCategoryCollection categories) {
            foreach (AtomCategory cat in categories) {
                if (cat.Scheme == GSiteService.KIND_SCHEME) {
                    return cat.Label;
                }
            }
            return null;
        }
        public static void printContentFeed(AtomFeed feed) {
            if (feed.Entries.Count == 0) {
                Console.WriteLine("No matching content found.");
            }

            foreach (AtomEntry entry in feed.Entries) {
                String pageType = getCategoryLabel(entry.Categories);
                Console.WriteLine(String.Format("Page: {0} ({1})", entry.Title.Text, pageType));
                Console.WriteLine(String.Format("  link: {0}", entry.AlternateUri));
                AtomPersonCollection authors = entry.Authors;
                foreach (AtomPerson author in authors) {
                    Console.WriteLine(String.Format("\tauthor: {0} - {1}", author.Name, author.Email));
                }
                String pageContent = entry.Content.Content;
                Console.WriteLine(String.Format("  html: {0}...", pageContent));
            }
        }
    }

}
