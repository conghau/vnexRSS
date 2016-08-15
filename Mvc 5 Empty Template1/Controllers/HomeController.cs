using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using VNExpressFeed.Models;
using VNExpressFeed.Utils;

namespace VNExpressFeed.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
           
            List<EndPointRSSModel> listRSSLink = VNExpressRssUtil.getEndPointRSS(Server.MapPath("~/ConfigRSS/vnexpress.rss.json"));
            var a = VNExpressRssUtil.parserXML(listRSSLink[0].rss);
            return View(a);
        }

       
    }
}