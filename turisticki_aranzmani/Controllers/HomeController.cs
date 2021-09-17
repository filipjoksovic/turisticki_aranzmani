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
            ViewBag.ArrangementTypes = new SelectList(ArrangementTypeModel.getAllItems().AsEnumerable(), "ID", "Name");
            ViewBag.DriveTypes = new SelectList(RideTypeModel.getAllItems().AsEnumerable(), "ID", "Name");
            return View(aModel);
        }
        public ActionResult Search(String MinDateStart, String MaxDateStart,String MinDateEnd, String MaxDateEnd, String ArrangementType, String DriveType, String Name ) {
            dynamic aModel = new ExpandoObject();
            List<ArrangementModel> arrangements = ArrangementModel.getAllItems();
            arrangements.OrderBy(arrangement => arrangement.DateStart);
            aModel.arrangements = arrangements;

            DateTime minDateStart, maxDateStart, minDateEnd, maxDateEnd;
            if (Name != null)
            {
                for (int i = aModel.arrangements.Count - 1; i >= 0; i--) {
                    if (!aModel.arrangements[i].Name.ToLower().Contains(Name.ToLower())) {
                        aModel.arrangements.RemoveAt(i);
                    }
                }
            }
            if (MinDateStart.Length > 0)
            {
                minDateStart = DateTime.Parse(MinDateStart);
                for (int i = aModel.arrangements.Count - 1; i >= 0; i--)
                {
                    if (aModel.arrangements[i].DateStart.CompareTo(minDateStart) == -1)
                    {
                        aModel.arrangements.RemoveAt(i);
                    }
                }
            }
            if (MaxDateStart.Length > 0)
            {
                maxDateStart = DateTime.Parse(MaxDateStart);
                for (int i = aModel.arrangements.Count - 1; i >= 0; i--)
                {
                    if (aModel.arrangements[i].DateStart.CompareTo(maxDateStart) == 1)
                    {
                        aModel.arrangements.RemoveAt(i);
                    }
                }
            }
            if (MinDateEnd.Length > 0)
            {
                minDateEnd = DateTime.Parse(MinDateEnd);
                for (int i = aModel.arrangements.Count - 1; i >= 0; i--)
                {
                    if (aModel.arrangements[i].DateStart.CompareTo(minDateEnd) == -1)
                    {
                        aModel.arrangements.RemoveAt(i);
                    }
                }
            }
            if (MaxDateEnd.Length > 0)
            {
                maxDateEnd = DateTime.Parse(MaxDateEnd);
                for (int i = aModel.arrangements.Count - 1; i >= 0; i--)
                {
                    if (aModel.arrangements[i].DateStart.CompareTo(maxDateEnd) == 1)
                    {
                        aModel.arrangements.RemoveAt(i);
                    }
                }
            }

            if (DriveType.Length > 0)
            {
                int dt = Convert.ToInt32(DriveType);
                for (int i = aModel.arrangements.Count - 1; i >= 0; i--)
                {
                    if (aModel.arrangements[i].DriveTypeID!=dt)
                    {
                        aModel.arrangements.RemoveAt(i);
                    }
                }
            }
            if (ArrangementType.Length > 0)
            {
                int at = Convert.ToInt32(ArrangementType);

                for (int i = aModel.arrangements.Count - 1; i >= 0; i--)
                {
                    if (aModel.arrangements[i].TypeID != at)
                    {
                        aModel.arrangements.RemoveAt(i);
                    }
                }
            }
            ViewBag.ArrangementTypes = new SelectList(ArrangementTypeModel.getAllItems().AsEnumerable(), "ID", "Name");
            ViewBag.DriveTypes = new SelectList(RideTypeModel.getAllItems().AsEnumerable(), "ID", "Name");
            return View("Index", aModel);
            return Json(aModel,JsonRequestBehavior.AllowGet);
        }
    }
}
