using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Models;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reservation
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection) {
            String[] referer = Request.UrlReferrer.ToString().Split('/');
            int arrangement_id = Convert.ToInt32(referer[referer.Length - 1]);
            int unit_id = Convert.ToInt32(collection["ResidenceUnit"]);
            String username = Session["username"].ToString();
            ReservationModel model= new ReservationModel();
            model.arrangement_id = arrangement_id;
            model.residence_item_id = unit_id;
            model.username = username;
            model.save();
            TempData["message"] = "Rezervacija je uspesno kreirana";
            return Redirect("~/");
        }
        public ActionResult ViewReservations(string role) {
            List<dynamic> detailedReservations = new List<dynamic>();

            if ((role.Equals("admin") || role.Equals("seller")) && (Session["role"] != null && Session["role"].Equals(role)))
            {
                List<ReservationModel> allReservations = new List<ReservationModel>();
                if (role.Equals("admin")) {
                    allReservations = ReservationModel.getAllItems();
                }
                else
                {
                    allReservations = ReservationModel.getAllItems(Session["username"].ToString());
                    detailedReservations = Utility.GetDetailedReservations(allReservations);
                }


                return View(detailedReservations);
            }
            else {
                TempData["error"] = "Nemate dozvolu pristupa ovom delu sajta";
                return Redirect("~/");
            }
        }
    }
}