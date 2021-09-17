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
        private Boolean Middleware()
        {
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

        public ActionResult Search(String Name, String HasPool, String HasSpa, String DisablityApproved, String HasWifi, String Sort)
        {
            if (this.Middleware())
            {
                List<ResidenceModel> allResidences = ResidenceModel.getAllItems(Session["username"].ToString());
                if (Name.Length > 0)
                {
                    for (int i = allResidences.Count - 1; i >= 0; i--)
                    {
                        if (!allResidences[i].Name.ToLower().Contains(Name.ToLower()))
                        {
                            allResidences.RemoveAt(i);
                        }
                    }
                }
                if (HasPool != null)
                {
                    for (int i = allResidences.Count - 1; i >= 0; i--)
                    {
                        if (!allResidences[i].HasPool)
                        {
                            allResidences.RemoveAt(i);
                        }
                    }
                }
                if (HasSpa != null)
                {
                    for (int i = allResidences.Count - 1; i >= 0; i--)
                    {
                        if (!allResidences[i].HasSpa)
                        {
                            allResidences.RemoveAt(i);
                        }
                    }
                }
                if (HasWifi != null)
                {
                    for (int i = allResidences.Count - 1; i >= 0; i--)
                    {
                        if (!allResidences[i].HasWifi)
                        {
                            allResidences.RemoveAt(i);
                        }
                    }
                }
                if (DisablityApproved != null)
                {
                    for (int i = allResidences.Count - 1; i >= 0; i--)
                    {
                        if (!allResidences[i].DisabilityApproved)
                        {
                            allResidences.RemoveAt(i);
                        }
                    }
                }
                if (Sort != null)
                {
                    if (Sort.Equals("nameAsc"))
                    {
                        allResidences.OrderBy(residence => residence.Name);
                    }
                    if (Sort.Equals("nameDesc"))
                    {
                        allResidences.OrderBy(residence => residence.Name);
                        allResidences.Reverse();
                    }
                    if (Sort.Equals("sumAsc"))
                    {
                        allResidences.OrderBy(residence => residence.GetUnitCount());

                    }
                    if (Sort.Equals("sumDesc"))
                    {
                        allResidences.OrderBy(residence => residence.GetUnitCount());
                        allResidences.Reverse();

                    }
                    if (Sort.Equals("freeAsc"))
                    {
                        allResidences.OrderBy(residence => residence.GetFreeUnitCount());

                    }
                    if (Sort.Equals("freeDesc"))
                    {
                        allResidences.OrderBy(residence => residence.GetFreeUnitCount());
                        allResidences.Reverse();

                    }
                }
                List<SelectListItem> sort = new List<SelectListItem>();
                sort.Add(new SelectListItem() { Text = "Naziv rastuce", Value = "nameAsc" });
                sort.Add(new SelectListItem() { Text = "Naziv opadajuce", Value = "nameAsc" });
                sort.Add(new SelectListItem() { Text = "Ukupan broj smestajnih jedinica rastuce", Value = "sumAsc" });
                sort.Add(new SelectListItem() { Text = "Ukupan broj smestajnih jedinica opadajuce", Value = "sumDesc" });
                sort.Add(new SelectListItem() { Text = "Broj slobodnih smestajnih jedinica rastuce", Value = "freeAsc" });
                sort.Add(new SelectListItem() { Text = "Broj slobodnih smestajnih jedinica opadajuce", Value = "freeDesc" });
                SelectList sortList = new SelectList(sort, "Value", "Text", Sort);
                ViewBag.sortList = sortList;

                return View("ListResidences", allResidences);
            }
            else
            {
                TempData["error"] = "Nemate dozvolu pristupa ovom sajtu";
                return Redirect("~/");
            }
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
                List<SelectListItem> sort = new List<SelectListItem>();
                sort.Add(new SelectListItem() { Text = "Naziv rastuce", Value = "nameAsc" });
                sort.Add(new SelectListItem() { Text = "Naziv opadajuce", Value = "nameDesc" });
                sort.Add(new SelectListItem() { Text = "Ukupan broj smestajnih jedinica rastuce", Value = "sumAsc" });
                sort.Add(new SelectListItem() { Text = "Ukupan broj smestajnih jedinica opadajuce", Value = "sumDesc" });
                sort.Add(new SelectListItem() { Text = "Broj slobodnih smestajnih jedinica rastuce", Value = "freeAsc" });
                sort.Add(new SelectListItem() { Text = "Broj slobodnih smestajnih jedinica opadajuce", Value = "freeDesc" });
                SelectList sortList = new SelectList(sort, "Value", "Text");
                ViewBag.sortList = sortList;

                return View(data);
            }
            else
            {
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
            else
            {
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
            else
            {
                return Redirect("~/");
            }
        }
        public ActionResult Edit(int id)
        {
            if (this.Middleware())
            {
                ResidenceModel residence = ResidenceModel.GetByID(id);
                return View(residence);
            }
            else
            {
                return Redirect("~/");
            }
        }
        [HttpPost]
        public ActionResult Edit(ResidenceModel model)
        {
            if (this.Middleware())
            {
                if (model.update())
                {
                    TempData["message"] = "Uspesno izmenjen smestaj";
                }
                else
                {
                    TempData["error"] = "Greska prilikom izmene smestaja";
                }
                return RedirectToRoute("User/Seller");
            }

            else
            {
                return Redirect("~/");
            }
        }



        public ActionResult Delete(string role, int id)
        {
            if (Session["username"] == null)
            {
                TempData["error"] = "Morate biti ulogovani kako biste mogli da pristupite ovom delu sajta";
                return Redirect("~/");
            }
            if (!Session["role"].Equals(role))
            {
                TempData["error"] = "Nemate dozvolu pristupa ovom delu sajta";
                return Redirect("~/");
            }

            ResidenceModel residence = ResidenceModel.GetByID(id);
            if (residence.delete())
            {
                TempData["message"] = "Uspesno uklonjen smestaj sa pratecim smestajnim jedinicama";
            }
            else
            {
                TempData["error"] = "Vec postoje rezervacije u ovom smestaju zbog cega ne moze biti uklonjen";
            }
            return Redirect(Request.UrlReferrer.ToString());

        }

    }
}
