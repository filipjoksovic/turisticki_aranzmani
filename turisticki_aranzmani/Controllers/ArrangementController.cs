﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Models;
using System.Dynamic;

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
            ViewBag.Residences = new SelectList(ResidenceModel.getAllItems().AsEnumerable(), "ID", "Name");
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
            //add a place model first



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
            string FileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ","_");
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
        public ActionResult Details(int id) {
            dynamic modelInstance = new ExpandoObject();
            ArrangementModel arrangement = ArrangementModel.GetByID(id);
            var dictionary = (IDictionary<string, object>)modelInstance;

            foreach (var property in arrangement.GetType().GetProperties())
                dictionary.Add(property.Name, property.GetValue(arrangement));
            modelInstance.RideTypeName = RideTypeModel.getRideName(arrangement.DriveTypeID);
            modelInstance.ArrangementTypeName = ArrangementTypeModel.getTypeName(arrangement.TypeID);
            modelInstance.ResidenceName = ResidenceModel.getResidenceName(arrangement.ResidenceID);
            modelInstance.ResidenceItems = new SelectList(ResidenceItemModel.getAvailableItems(arrangement.ResidenceID).AsEnumerable(),"ID","UnitName", ResidenceItemModel.getBookedItems(arrangement.ResidenceID).AsEnumerable());
            return View("Details",modelInstance);
        }
        //public ActionResult Edit() { 

        //}
        //public ActionResult() Delete{ 

        //}
    }
}