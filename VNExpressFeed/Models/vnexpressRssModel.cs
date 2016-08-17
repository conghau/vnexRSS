using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VNExpressFeed.Models
{
    public class VNExpressRssModel
    {
        public String link;
        public String description;
        public String title;
        public String image;
        public String getImage(string type="desktop")
        {
            if(type=="desktop")
            {
                return image.Replace("_180x108", "");
            }
            return image;
        }
    }

    public class EndPointRSSModel
    {
        public String label;
        public String rss;
        public String base_link;
        public String get_full_link() { return this.base_link + this.rss;}
    }
}