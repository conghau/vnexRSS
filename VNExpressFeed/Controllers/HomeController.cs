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
        private static string _perPage = ConfigurationManager.AppSettings["itemPerPage"].ToString() ?? "10";
        private static string _pathRSS = ConfigurationManager.AppSettings["pathRSSConfig"].ToString() ?? "";
        // GET: Home
        public ActionResult Index(string type = "")
        {
            @ViewBag.type = type;
            @ViewBag.itemPerPage = _perPage;
            List<EndPointRSSModel> listRSSLink = null;
            string base_link = "";
            if (null == Session["listRSSLink"])
            {
                listRSSLink = VNExpressRssUtil.getEndPointRSS(Server.MapPath(_pathRSS));
                base_link = listRSSLink[0].base_link;
                Session["listRSSLink"] = listRSSLink;
                Session["base_link"] = base_link;
            }
            else
            {
                listRSSLink = Session["listRSSLink"] as List<EndPointRSSModel>;
                base_link = Session["base_link"] as string;
            }


            string fullLinkRss = base_link + type;
            if ("".Equals(type))
            {
                fullLinkRss = listRSSLink[0].get_full_link();
            }
            var model = VNExpressRssUtil.parserXML(fullLinkRss);
            return View(model);
        }
        public ActionResult Sidebar()
        {
            //List<EndPointRSSModel> listRSSLink = VNExpressRssUtil.getEndPointRSS(Server.MapPath(_pathRSS));
            List<EndPointRSSModel> listRSSLink = null;
            if (null == Session["listRSSLink"])
            {
                listRSSLink = VNExpressRssUtil.getEndPointRSS(Server.MapPath(_pathRSS));
                Session["listRSSLink"] = listRSSLink;
                Session["base_link"] = listRSSLink[0].base_link;
            }
            else
            {
                listRSSLink = Session["listRSSLink"] as List<EndPointRSSModel>;
            }
            return PartialView("_Sidebar", listRSSLink);
        }
    }
}