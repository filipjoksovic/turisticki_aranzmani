using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace turisticki_aranzmani.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Models.UserModel userModel)
        {
            if (userModel.exists())
            {
                TempData["error"] = "Nalog sa ovim korisnickim imenom ili email adresom vec postoji";
                return RedirectToAction("register");
            }
            else
            {
                bool saved = userModel.save();
                if (saved)
                {
                    TempData["message"] = "Uspesno kreiranje naloga. Dobrodosli, " + userModel.Username;
                    Session["username"] = userModel.Username;
                    Session["role"] = userModel.Role;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = "Greska prilikom kreiranja naloga";
                    return RedirectToAction("register");
                }

            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.UserModel userModel)
        {
            Models.UserModel foundInstance = Models.UserModel.find(userModel);
            if (foundInstance != null)
            {
                Session["username"] = foundInstance.Username;
                Session["role_id"] = foundInstance.Role;
                TempData["message"] = "Uspesno prijavljivanje. Dobrodosli nazad, " + foundInstance.Username;

                if (foundInstance.Role.Equals("admin"))
                {
                    return RedirectToAction("admin");
                }
                else if (foundInstance.Role.Equals("menadzer"))
                {
                    return RedirectToAction("seller");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["error"] = "Greska prilikom logovanja. Proverite vase podatke i pokusajte ponovo";
                return RedirectToAction("login");
            }
        }
        public ActionResult Logout()
        {
            Session["username"] = null;
            Session["role_id"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult Seller()
        {
            return View();
        }

        public ActionResult createManager()
        {
            return View("CreateManager");
        }
        [HttpPost]
        public ActionResult createManager(Models.UserModel userModel)
        {
            Models.UserModel loggedInUser = Models.UserModel.find(Session["username"].ToString());
            if (loggedInUser == null)
            {
                TempData["error"] = "Morate biti ulogovani kao administrator kako biste mogli da kreirate menadzera";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (userModel.exists())
                {
                    TempData["erorr"] = "Nalog sa ovim korisnickim imenom ili email adresom vec postoji";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    userModel.Role = "menadzer";
                    if (userModel.save())
                    {
                        TempData["message"] = "Novi menadzer sa korisnickim imenom " + userModel.Username + " je uspesno kreiran";
                    }
                    else
                    {
                        TempData["message"] = "Doslo je do kreske prilikom kreiranja novog menadzera. Proverite ponovo podatke i pokusajte ponovo";
                    }
                    return RedirectToAction("admin");
                }

            }
        }
    }
}