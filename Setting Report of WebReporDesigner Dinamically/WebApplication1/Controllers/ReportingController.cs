using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ReportingController : Controller
    {
        public ActionResult ReportDesigner(string name)
        {
            string reportName = name;
            DesignerModel designer = new DesignerModel(reportName);
            ViewBag.Message = designer;
            return View();
        }


    }
}
