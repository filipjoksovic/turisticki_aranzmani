using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Models;
using System.Dynamic;

namespace turisticki_aranzmani.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            dynamic aModel = new ExpandoObject();
            List<ArrangementModel> arrangements = ArrangementModel.getAllItems();
            arrangements.OrderBy(arrangement => arrangement.DateStart);
            aModel.arrangements = arrangements;
            return View(aModel);
        }
    }
}
