using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using VNExpressFeed.Models;

namespace VNExpressFeed.Utils
{
    public class VNExpressRssUtil
    {
        public static List<EndPointRSSModel> getEndPointRSS(String pathRssConfig = "")
        {
            if ("".Equals(pathRssConfig))
            {
                return null;
            }
            dynamic vnExpressRSS = parserJson(pathRssConfig);
            if (null == vnExpressRSS)
            {
                return null;
            }

            String url = vnExpressRSS.link;
            var result = new List<EndPointRSSModel>();
            var endPoints = vnExpressRSS.end_point;
            foreach (var item in endPoints)
            {
                result.Add(new EndPointRSSModel { label = item.label, rss = url + item.rss });
            }
            return result;
        }

        public static object parserJson(String pathRssConfig)
        {
            dynamic vnExpressRSS = new ExpandoObject();
            using (StreamReader sr = new StreamReader(pathRssConfig))
            {
                string json = sr.ReadToEnd();
                vnExpressRSS = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            }
            return vnExpressRSS;
        }

        public static String getContentRSS(string linkRSS = "")
        {
            String html = "";
            if ("".Equals(linkRSS))
            {
                return html;
            }
            using (WebClient client = new WebClient())
            {
                html = client.DownloadString(linkRSS);
            }
            return html;
        }

        public static List<VNExpressRssModel> parserXML(string linkRSS, bool parserToObject = true)
        {
            XmlDocument rssXmlDoc = new XmlDocument();
            rssXmlDoc.Load("http://vnexpress.net/rss/tin-moi-nhat.rss");

            // Parse the Items in the RSS file
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            //StringBuilder rssContent = new StringBuilder();
            List<VNExpressRssModel> rssContent = new List<VNExpressRssModel>();

            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("link");
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("description");
                string content = rssSubNode != null ? rssSubNode.InnerText : "";

                string description = "";
                string image = "";
                if(!"".Equals(content))
                {
                    getImageAndDescription(content, ref description, ref image);
                }

                rssContent.Add(new VNExpressRssModel
                {
                    title = title,
                    description = description,
                    link = link,
                    image = image
                });
            }

            // Return the string that contain the RSS items
            return rssContent;

        }

        private static void getImageAndDescription(String html, ref string description, ref string image)
        {
            HtmlDocument document2 = new HtmlDocument();
            document2.LoadHtml(html);

            description = document2.DocumentNode.InnerText;

            image = document2.DocumentNode.SelectNodes("//img").First().OuterHtml;
           
        }
    }
}