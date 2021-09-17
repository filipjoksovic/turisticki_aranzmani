using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Models;
using System.Dynamic;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Controllers
{
    public class ArrangementController : Controller
    {
        // GET: Arrangement
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            if ((Session["username"] == null && Session["role"] == null) || (!Session["role"].ToString().Equals("seller") && !Session["role"].Equals("admin")))
            {
                TempData["error"] = "Morate biti ulogovani kao menadzer ili administrator kako biste pristupili ovoj stranici";
                return Redirect("~/");
            }
            List<ArrangementTypeModel> allItems = ArrangementTypeModel.getAllItems();
            ViewBag.ArrangementTypes = new SelectList(allItems.AsEnumerable(), "ID", "Name");
            ViewBag.RideTypes = new SelectList(RideTypeModel.getAllItems().AsEnumerable(), "ID", "Name");
            ViewBag.Residences = new SelectList(ResidenceModel.getAllItems(Session["username"].ToString()).AsEnumerable(), "ID", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase file)
        {
            if ((Session["username"] == null && Session["role"] == null) || (!Session["role"].ToString().Equals("seller") && !Session["role"].Equals("admin")))
            {
                TempData["error"] = "Morate biti ulogovani kao menadzer ili administrator kako biste pristupili ovoj stranici";
                return Redirect("~/");
            }
            ArrangementModel modelInstance = new ArrangementModel();
            modelInstance.Username = Session["username"].ToString();
            modelInstance.Name = collection["Name"];
            modelInstance.TypeID = Convert.ToInt32(collection["TypeID"]);
            modelInstance.DriveTypeID = Convert.ToInt32(collection["DriveTypeID"]);
            modelInstance.Location = collection["Location"];
            modelInstance.DateStart = DateTime.Parse(collection["DateStart"]);
            modelInstance.DateEnd = DateTime.Parse(collection["DateEnd"]);
            modelInstance.Description = collection["Description"];
            modelInstance.MaxCustomers = Convert.ToInt32(collection["MaxCustomers"]);
            modelInstance.TimeStarting = DateTime.Parse(collection["TimeStarting"]);
            modelInstance.Programme = collection["Programme"];
            modelInstance.ResidenceID = Convert.ToInt32(collection["ResidenceID"]);
            string FileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "_");
            System.Diagnostics.Debug.WriteLine(FileName);
            string FileExtension = Path.GetExtension(file.FileName);
            modelInstance.ImagePath = FileName + FileExtension;
            string UploadPath = Server.MapPath("~/Content/arrangement_images/") + FileName + FileExtension;
            System.Diagnostics.Debug.WriteLine(UploadPath);

            PlaceModel placeModelInstance = new PlaceModel();
            placeModelInstance.ArrangementID = modelInstance.ID;
            placeModelInstance.Street = collection["PlaceStreet"];
            placeModelInstance.City = collection["PlaceCity"];
            placeModelInstance.ZipCode = collection["PlaceZipCode"];
            placeModelInstance.Longitute = collection["Longitude"];
            placeModelInstance.Latitude = collection["Latitude"];

            modelInstance.StartingPointID = placeModelInstance.PlaceID;

            if (modelInstance.save())
            {
                placeModelInstance.save();
                file.SaveAs(UploadPath);
                TempData["message"] = "Uspesno kreiran aranzman";
                return RedirectToRoute("User/Seller");
            }
            else
            {
                TempData["error"] = "Doslo je do greske prilikom kreiranja aranzmana";
                return RedirectToRoute("User/Seller");
            }
        }
        public ActionResult Edit(String role, String id)
        {
            if (id == null)
            {
                return Redirect("~/");
            }
            else
            {
                ArrangementModel arrangementModel = ArrangementModel.GetByID(Convert.ToInt32(id));
                if (arrangementModel.canEdit())
                {
                    if ((Session["username"] == null && Session["role"] == null) || (!Session["role"].ToString().Equals("seller") && !Session["role"].Equals("admin")))
                    {
                        TempData["error"] = "Morate biti ulogovani kao menadzer ili administrator kako biste pristupili ovoj stranici";
                        return Redirect("~/");
                    }
                    List<ArrangementTypeModel> allItems = ArrangementTypeModel.getAllItems();
                    dynamic expandedModel = Utility.ExpandArrangement(arrangementModel);
                    ViewBag.ArrangementTypes = new SelectList(allItems.AsEnumerable(), "ID", "Name", arrangementModel.TypeID);
                    List<SelectListItem> atItems = new List<SelectListItem>();
                    ViewBag.RideTypes = new SelectList(RideTypeModel.getAllItems().AsEnumerable(), "ID", "Name", arrangementModel.DriveTypeID);
                    ViewBag.Residences = new SelectList(ResidenceModel.getAllItems(Session["username"].ToString()).AsEnumerable(), "ID", "Name", arrangementModel.ResidenceID);
                    ViewBag.canEdit = true;

                    return View(expandedModel);

                }
                else
                {
                    ViewBag.canEdit = false;
                    return View();
                }

            }
        }
        [HttpPost]
        public ActionResult Edit(string role, string id, FormCollection collection, HttpPostedFileBase file)
        {
            if ((Session["username"] == null && Session["role"] == null) || (!Session["role"].ToString().Equals("seller") && !Session["role"].Equals("admin")))
            {
                TempData["error"] = "Morate biti ulogovani kao menadzer ili administrator kako biste pristupili ovoj stranici";
                return Redirect("~/");
            }
            ArrangementModel modelInstance = ArrangementModel.GetByID(Convert.ToInt32(id));
            modelInstance.Username = Session["username"].ToString();
            modelInstance.Name = collection["Name"];
            modelInstance.TypeID = Convert.ToInt32(collection["TypeID"]);
            modelInstance.DriveTypeID = Convert.ToInt32(collection["DriveTypeID"]);
            modelInstance.Location = collection["Location"];
            modelInstance.DateStart = DateTime.Parse(collection["DateStart"]);
            modelInstance.DateEnd = DateTime.Parse(collection["DateEnd"]);
            modelInstance.Description = collection["Description"];
            modelInstance.MaxCustomers = Convert.ToInt32(collection["MaxCustomers"]);
            modelInstance.TimeStarting = DateTime.Parse(collection["TimeStarting"]);
            modelInstance.Programme = collection["Programme"];
            modelInstance.ResidenceID = Convert.ToInt32(collection["ResidenceID"]);

            PlaceModel placeModelInstance = PlaceModel.GetByID(modelInstance.StartingPointID);
            placeModelInstance.Street = collection["PlaceStreet"];
            placeModelInstance.City = collection["PlaceCity"];
            placeModelInstance.ZipCode = collection["PlaceZipCode"];
            placeModelInstance.Longitute = collection["Longitude"];
            placeModelInstance.Latitude = collection["Latitude"];


            if (modelInstance.update())
            {
                placeModelInstance.update();
                TempData["message"] = "Uspesno azuriran aranzman";
                return RedirectToRoute("User/Seller");
            }
            else
            {
                TempData["error"] = "Doslo je do greske prilikom azuriranja aranzmana";
                return RedirectToRoute("User/Seller");
            }
        }
        public ActionResult ViewArrangements(String role)
        {

            if (Session["username"] == null)
            {
                TempData["error"] = "Morate biti registrovani kao korisnik kako biste pristupili ovom delu sajta";
                return Redirect("~/");
            }
            else if (!Session["role"].Equals(role))
            {
                TempData["error"] = "Nemate dozvolu za pristup ovom delu sajta";
                return Redirect("~/");
            }
            else
            {
                List<ArrangementModel> arrangements;
                List<dynamic> detailed_arrangements = new List<dynamic>();
                if (role.Equals("seller"))
                {
                    arrangements = ArrangementModel.getAllItems(Session["username"].ToString());
                }
                else if (role.Equals("admin"))
                {
                    arrangements = ArrangementModel.getAllItems();
                }
                else
                {
                    return Redirect("~/");
                }
                foreach (ArrangementModel model in arrangements)
                {
                    dynamic expandedModel = Utility.ExpandArrangement(model);
                    detailed_arrangements.Add(expandedModel);
                }
                return View(detailed_arrangements);
            }
        }
        public ActionResult Delete(string role, int id)
        {
            if (Session["role"] == null)
            {
                TempData["error"] = "Morate biti ulogovani kako biste mogli da pristupite ovom delu sajta";
                return Redirect("~/");
            }
            else
            {
                if (!Session["role"].ToString().Equals(role))
                {
                    TempData["error"] = "Nemate dozvolu za pristup ovom delu sajta";
                    return Redirect("~/");

                }
                else
                {
                    ArrangementModel amodel = ArrangementModel.GetByID(id);
                    amodel.delete();
                    TempData["message"] = "Uspesno uklonjen aranzman";
                    return Redirect(HttpContext.Request.UrlReferrer.ToString());
                }
            }
        }

        public ActionResult Details(int id)
        {
            ArrangementModel arrangement = ArrangementModel.GetByID(id);
            dynamic modelInstance = Utility.ExpandArrangement(arrangement);
            modelInstance.ResidenceItems = new SelectList(ResidenceItemModel.getAvailableItems(id).AsEnumerable(), "ID", "UnitName");
            modelInstance.GroupingPlace = PlaceModel.GetByID(arrangement.StartingPointID);
            modelInstance.Reviews = ArrangementCommentModel.GetComments(id, true);
            if (Session["username"] != null)
            {
                modelInstance.HasReserved = ReservationModel.HasUserReservation(Session["username"].ToString(), arrangement.ID);
            }
            return View("Details", modelInstance);
        }

    }
}