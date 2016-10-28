using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;

namespace HNTeleBot
{
    public static class Feed
    {
        public static IEnumerable<SyndicationItem> GetFeed()
        {
            string url = @"https://news.ycombinator.com/rss";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            return feed.Items;
        }
    }
}
