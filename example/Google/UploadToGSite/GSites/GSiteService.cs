using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Extensions;
using Google.GData.Client;
using System.Xml;

namespace Google.GData.Documents.GSites
{
    public class SiteEntry : AbstractEntry {
        public SiteEntry() : base() { }
    }


    public class GSiteService : MediaService
    {
        public const string GSitesService = "jotspot";
        public const string SITES_NAMESPACE = "http://schemas.google.com/sites/2008";
        public const string KIND_SCHEME = "http://schemas.google.com/g/2005#kind";
        public const string ATTACHMENT_TERM = SITES_NAMESPACE + "#attachment";
        public const string WEBPAGE_TERM = SITES_NAMESPACE + "#webpage";
        public const string FILECABINET_TERM = SITES_NAMESPACE + "#filecabinet";
        public const string PARENT_REL = SITES_NAMESPACE + "#parent";

        public String Domain { get; set; }
        public String SiteName { get; set; }
        public GSiteService(string applicationName, string domain, string site_name)
            : base(GSitesService, applicationName)
        {
            Domain = domain;
            SiteName = site_name;
        }

        private String MakeFeedUri(String type) {
            return String.Format("http://sites.google.com/feeds/{0}/{1}/{2}/", type, Domain, SiteName);
        }

        private XmlExtension MakePageNameExtension(String pageName) {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode pageNameNode = xmlDocument.CreateNode(XmlNodeType.Element,
              "sites", "pageName", GSiteService.SITES_NAMESPACE);
            pageNameNode.InnerText = pageName;

            return new XmlExtension(pageNameNode);
        }




        public AtomFeed GetContentFeedPath(String path) {
            return GetContentFeed(MakeFeedUri("content") + "?path=" + path);
        }

        public AtomFeed GetContentFeed() {
            return GetContentFeed(MakeFeedUri("content"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedUri"></param>
        /// <returns></returns>
        public AtomFeed GetContentFeed(String feedUri) {
            FeedQuery query = new FeedQuery(feedUri);
            return this.Query(query);            
        }

        public AtomFeed GetContentByType(String type) {
            String feedUri = MakeFeedUri("content") + String.Format("?kind={2}", type);
            return GetContentFeed(feedUri);
        }

        public AtomFeed GetActivityFeed() {
            return GetActivityFeed(MakeFeedUri("activity"));
        }

        public AtomFeed GetActivityFeed(String feedUri) {
            FeedQuery query = new FeedQuery();
            query.Uri = new Uri(feedUri);
            return  this.Query(query);
        }

        public AtomFeed GetRevisionFeed(String entryId) {
            FeedQuery query = new FeedQuery();
            String feedUri = MakeFeedUri("revision") + entryId;
            query.Uri = new Uri(feedUri);
            return this.Query(query);
        }

        /// <summary>
        /// 建立新網頁
        /// </summary>
        /// <param name="title"></param>
        /// <param name="html"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public AtomEntry CreateWebPage(String title, String html, String pageName) {
            SiteEntry entry = new SiteEntry();
            AtomCategory category = new AtomCategory(GSiteService.WEBPAGE_TERM, GSiteService.KIND_SCHEME);
            category.Label = "webpage";
            entry.Categories.Add(category);
            entry.Title.Text = title;
            entry.Content.Type = "xhtml";
            entry.Content.Content = html;
            entry.ExtensionElements.Add(MakePageNameExtension(pageName));

            AtomEntry newEntry = null;
            try {
                newEntry = this.Insert<AtomEntry>(new Uri(MakeFeedUri("content")), entry);
            } catch (GDataRequestException e) {
                Console.WriteLine(e.ResponseString);
            }


            return newEntry;
        }

        public AtomEntry UpdloadAttachment(String filename, String contentType, AtomEntry parent, String title, String description) {
            SiteEntry entry = new SiteEntry();

            AtomCategory category = new AtomCategory(GSiteService.ATTACHMENT_TERM, GSiteService.KIND_SCHEME);
            category.Label = "attachment";
            entry.Categories.Add(category);

            AtomLink parentLink = new AtomLink(AtomLink.ATOM_TYPE, GSiteService.PARENT_REL);
            parentLink.HRef = parent.SelfUri;
            entry.Links.Add(parentLink);

            entry.MediaSource = new MediaFileSource(filename, contentType);
            entry.Content.Type = contentType;

            if (title == "") {
                entry.Title.Text = entry.MediaSource.Name;
            } else {
                entry.Title.Text = title;
            }

            entry.Summary.Text = description;

            AtomEntry newEntry = null;
            try {
                newEntry = this.Insert<AtomEntry>(new Uri(MakeFeedUri("content")), entry);
            } catch (GDataRequestException e) {
                Console.WriteLine(e.ResponseString);
            }

            return newEntry;
        }
    }
}
