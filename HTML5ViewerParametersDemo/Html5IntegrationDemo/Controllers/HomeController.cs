using HTML5ViewerParametersDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace HTML5ViewerParametersDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var reportSourceModel = new ReportSourceModel()
            {
                Report = "Barcodes Report.trdp",
                Parameters = new Dictionary<string, object>()
            };
            reportSourceModel.Parameters.Add("MyParameter1", DateTime.Now);
            reportSourceModel.Parameters.Add("MyParameter2", 42);

            return View(reportSourceModel);
        }
    }
}
