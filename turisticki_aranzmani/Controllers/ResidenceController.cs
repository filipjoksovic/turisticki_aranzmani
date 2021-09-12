using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Models;

namespace turisticki_aranzmani.Controllers
{
    public class ResidenceController : Controller
    {
        private Boolean Middleware() {
            System.Diagnostics.Debug.WriteLine("here1");

            if (Session["username"] != null && Session["role"] != null)
            {
                String username = Session["username"].ToString();
                String role = Session["role"].ToString();
                if (role.Equals("admin") || role.Equals("seller"))
                {
                    //TempData["error"] = "Morate biti ulogovani kao menadzer ili administrator kako biste imali pristup ovom delu sajta";
                    return true;
                }
                return false;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("here");
                TempData["error"] = "Morate biti ulogovani kao menadzer ili administrator kako biste imali pristup ovom delu sajta";
                return false;
            }
        }
        // GET: Residence
        public ActionResult Index()
        {
            return View();
        }

        // GET: Residence/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult ListResidences()
        {
            if (this.Middleware())
            {
                System.Diagnostics.Debug.WriteLine("data");
                System.Diagnostics.Debug.WriteLine(Session["username"].ToString());
                System.Diagnostics.Debug.WriteLine(Session["role"].ToString());

                IEnumerable<ResidenceModel> data;
                if (Session["username"].ToString() != "" && Session["role"].ToString().Equals("seller"))
                {
                    String uname = Session["username"].ToString();
                    data = ResidenceModel.getAllItems(uname);

                }
                else if (Session["username"].ToString() != "" && Session["role"].ToString().Equals("admin"))
                {
                    data = ResidenceModel.getAllItems();
                }
                else
                {
                    TempData["error"] = "Morate biti ulogovani kao administrator ili menadzer kako biste pristupili ovom delu sajta";
                    return Redirect("~/");

                }
                return View(data);
            }
            else {
                return Redirect("~/");
            }
        }

        // GET: Residence/Create
        public ActionResult Create()
        {
            if (this.Middleware())
            {
                return View("CreateResidence");
            }
            else {
                return Redirect("~/");
            }
        }

        // POST: Residence/Create
        [HttpPost]
        public ActionResult Create(Models.ResidenceModel model)
        {
            if (this.Middleware())
            {
                try
                {
                    try
                    {
                        model.Username = Session["username"].ToString();
                    }
                    catch
                    {
                        model.Username = "null";
                    }
                    if (!model.save())
                    {
                        TempData["error"] = "Doslo je do greske prilikom kreiranja smestaja";
                    }
                    else
                    {
                        TempData["message"] = "Novi smestaj je uspesno kreiran. Kreirajte nove smestajne jedinice za njega kako biste mogli da dodajete aranzmane povezane s njim";
                    }
                    return RedirectToAction("seller", "User");

                }
                catch
                {
                    return View();
                }
            }
            else {
                return Redirect("~/");
            }
        }

        // GET: Residence/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Residence/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Residence/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Residence/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
