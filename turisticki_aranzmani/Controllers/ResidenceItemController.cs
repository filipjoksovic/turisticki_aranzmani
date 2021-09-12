using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Models;

namespace turisticki_aranzmani.Controllers
{
    public class ResidenceItemController : Controller
    {
        // GET: ResidenceItem
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListResidenceUnits(int id) {
            List<ResidenceItemModel> data= ResidenceItemModel.getAllItems(id);
            return View(data);
        }
        public ActionResult CreateResidenceUnit(int id) {
            //return Content("test");
            ViewBag.id = id;
            return View("CreateResidenceUnit");
        }
        [HttpPost]
        public ActionResult CreateResidenceUnit(ResidenceItemModel model)
        {
            if (!model.save())
            {
                TempData["error"] = "Doslo je do greske prilikom kreiranja smestajne jedinice. Proverite da li vec postoji smestajna jedinica pod ovim nazivom i pokusajte ponovo";
                return RedirectToRoute("User/Seller");
            }
            else {
                TempData["message"] = "Uspesno sacuvana smestajna jedinica.";
                return RedirectToRoute("User/Seller");
            }
        }
    }
}