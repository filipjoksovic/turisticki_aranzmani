using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Models;
using System.Dynamic;


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
            List<ResidenceItemModel> data = ResidenceItemModel.getAllItems(id);
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
        public JsonResult Details(int id) {
            ResidenceItemModel Response = null;
            if (id == null)
            {
                Response = null;
            }
            else {
                Response = ResidenceItemModel.GetByID(id);
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id) {
            if (Session["username"] == null || Session["role"] == null || Session["role"].Equals("user"))
            {
                TempData["erorr"] = "Nemate pristup ovom delu sajta";
            }
            else {
                ResidenceItemModel model = ResidenceItemModel.GetByID(id);
                if (model.delete())
                {
                    TempData["message"] = "Uspesno uklonjena smestajna jedinica";
                }
                else {
                    TempData["error"] = "Greska prilikom uklanjanja smestajne jedinice";
                }
                return RedirectToRoute("User/Seller");
            }
        }
    }
}