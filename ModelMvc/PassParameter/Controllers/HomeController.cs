using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PassParameter.Models;

namespace PassParameter.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            Dictionary<string,object>values = new Dictionary<string, object>() { { "Parameter1", "Item2" }, { "Parameter2", "Value2" } };
            ReportModel reportModel = new ReportModel()
            {
                ReportName = "SampleReport.trdp",
                Parameters = values
            };
            return View(reportModel);
        }

        public ActionResult Report(string id, string parameter1, string parameter2)
        {
            Dictionary<string, object> values = new Dictionary<string, object>() { { "Parameter1", parameter1 }, { "Parameter2", parameter2 } };
            ReportModel reportModel = new ReportModel()
            {
                ReportName = $"{id}.trdp",
                Parameters = values
            };
            return View("Index", reportModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}