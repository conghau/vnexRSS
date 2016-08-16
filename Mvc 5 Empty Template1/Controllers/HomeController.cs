using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using VNExpressFeed.Models;
using VNExpressFeed.Utils;

namespace VNExpressFeed.Controllers
{
    public class HomeController : Controller
    {
        private static string perPage = ConfigurationManager.AppSettings["itemPerPage"].ToString() ?? "10";
        private static string pathRSS = ConfigurationManager.AppSettings["pathRSSConfig"].ToString() ?? "";
        // GET: Home
        public ActionResult Index(string type = "")
        {
            @ViewBag.type = type;
            @ViewBag.itemPerPage = perPage;
            List<EndPointRSSModel> listRSSLink = VNExpressRssUtil.getEndPointRSS(Server.MapPath(pathRSS));
            string fullLinkRss = listRSSLink[0].base_link + type;
            if ("".Equals(type))
            {
                fullLinkRss = listRSSLink[0].get_full_link();
            }
            var a = VNExpressRssUtil.parserXML(fullLinkRss);
            return View(a);
        }
        public ActionResult Sidebar()
        {
            List<EndPointRSSModel> listRSSLink = VNExpressRssUtil.getEndPointRSS(Server.MapPath("~/ConfigRSS/vnexpress.rss.json"));
            return PartialView("_Sidebar", listRSSLink);
        }
       
    }
}