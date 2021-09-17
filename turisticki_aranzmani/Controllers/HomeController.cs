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
        public ActionResult Search(String MinDateStart, String MaxDateStart,String MinDateEnd, String MaxDateEnd, String ArrangementType, String DriveType, String Name, String Sort) {
            dynamic aModel = new ExpandoObject();
            List<ArrangementModel> arrangements = ArrangementModel.getAllItems();
            arrangements.OrderBy(arrangement => arrangement.DateStart);
            aModel.arrangements = arrangements;

            DateTime minDateStart, maxDateStart, minDateEnd, maxDateEnd;
            if (Name != null)
            {
                for (int i = arrangements.Count - 1; i >= 0; i--) {
                    if (!arrangements[i].Name.ToLower().Contains(Name.ToLower())) {
                        arrangements.RemoveAt(i);
                    }
                }
            }
            if (MinDateStart.Length > 0)
            {
                minDateStart = DateTime.Parse(MinDateStart);
                for (int i = arrangements.Count - 1; i >= 0; i--)
                {
                    if (arrangements[i].DateStart.CompareTo(minDateStart) == -1)
                    {
                        arrangements.RemoveAt(i);
                    }
                }
            }
            if (MaxDateStart.Length > 0)
            {
                maxDateStart = DateTime.Parse(MaxDateStart);
                for (int i = arrangements.Count - 1; i >= 0; i--)
                {
                    if (arrangements[i].DateStart.CompareTo(maxDateStart) == 1)
                    {
                        arrangements.RemoveAt(i);
                    }
                }
            }
            if (MinDateEnd.Length > 0)
            {
                minDateEnd = DateTime.Parse(MinDateEnd);
                for (int i = arrangements.Count - 1; i >= 0; i--)
                {
                    if (arrangements[i].DateStart.CompareTo(minDateEnd) == -1)
                    {
                        arrangements.RemoveAt(i);
                    }
                }
            }
            if (MaxDateEnd.Length > 0)
            {
                maxDateEnd = DateTime.Parse(MaxDateEnd);
                for (int i = arrangements.Count - 1; i >= 0; i--)
                {
                    if (arrangements[i].DateStart.CompareTo(maxDateEnd) == 1)
                    {
                        arrangements.RemoveAt(i);
                    }
                }
            }

            if (DriveType.Length > 0)
            {
                int dt = Convert.ToInt32(DriveType);
                for (int i = arrangements.Count - 1; i >= 0; i--)
                {
                    if (arrangements[i].DriveTypeID!=dt)
                    {
                        arrangements.RemoveAt(i);
                    }
                }
            }
            if (ArrangementType.Length > 0)
            {
                int at = Convert.ToInt32(ArrangementType);

                for (int i = arrangements.Count - 1; i >= 0; i--)
                {
                    if (arrangements[i].TypeID != at)
                    {
                        arrangements.RemoveAt(i);
                    }
                }
            }
            if (!Sort.Equals("-1")) {
                if (Sort.Equals("nameAsc")) {
                    arrangements.OrderBy(arrangement => arrangement.Name);
                }
                if (Sort.Equals("nameDesc")) {
                    arrangements.OrderBy(arrangement => arrangement.Name);
                    arrangements.Reverse();
                }
                if (Sort.Equals("dateStartAsc")) {
                    arrangements.OrderBy(arrangement => arrangement.DateStart);
                }
                if (Sort.Equals("dateStartDesc")) { 
                    arrangements.OrderBy(arrangement => arrangement.DateStart);
                    arrangements.Reverse();
                }
                if (Sort.Equals("dateEndAsc")) { 
                    arrangements.OrderBy(arrangement => arrangement.DateStart);
                }
                if (Sort.Equals("dateEndDesc")) {
                    arrangements.OrderBy(arrangement => arrangement.DateStart);
                    arrangements.Reverse();
                }
            }
            ViewBag.ArrangementTypes = new SelectList(ArrangementTypeModel.getAllItems().AsEnumerable(), "ID", "Name");
            ViewBag.DriveTypes = new SelectList(RideTypeModel.getAllItems().AsEnumerable(), "ID", "Name");
            ViewBag.SortOrder = Sort;
            aModel.arrangements = arrangements;
            return View("Index", aModel);
            return Json(aModel,JsonRequestBehavior.AllowGet);
        }
    }
}
