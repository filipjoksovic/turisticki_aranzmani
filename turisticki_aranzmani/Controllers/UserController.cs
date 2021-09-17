using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Models;
using System.Dynamic;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Account() {
            if (Session["username"] == null)
            {
                TempData["error"] = "Morate biti ulogovani kako biste mogli da pristupite Vasem nalogu";
                return Redirect("~/");
            }
            else {
                dynamic account = new ExpandoObject();
                account.User = UserModel.GetUser(Session["username"].ToString());

                account.Reservations = Utility.GetDetailedReservations(ReservationModel.getAllItems(Session["username"].ToString()));
                return View(account);
            }
        }
        [HttpPost]
        public ActionResult Account(FormCollection collection) {
            if (Session["username"] == null)
            {
                TempData["error"] = "Morate biti ulogovani kako biste mogli da pristupite Vasem nalogu";
                return Redirect("~/");
            }
            else {
                UserModel userInstance = UserModel.GetUser(Session["Username"].ToString());
                userInstance.FirstName = collection["FirstName"];
                userInstance.LastName = collection["LastName"];
                userInstance.Gender = collection["Gender"];
                userInstance.Email = collection["Email"];
                userInstance.Birthday = DateTime.Parse(collection["Birthday"]);
                if (userInstance.update())
                {
                    TempData["message"] = "Uspesno izmenjeni podaci o nalogu";
                }
                else
                {
                    TempData["error"] = "Greska prilikom izmene naloga";
                }
                return RedirectToRoute("User/Account");
            }

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Models.UserModel userModel)
        {
            Models.UserModel loggedInUser = Models.UserModel.find(Session["username"].ToString());
            if (loggedInUser == null)
            {
                TempData["error"] = "Morate biti ulogovani kao administrator kako biste mogli da kreirate menadzera";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (!userModel.save())
                {
                    TempData["error"] = "Nalog sa ovim korisnickim imenom ili email adresom vec postoji";
                    return RedirectToAction("admin");
                }
                else
                {
                    TempData["message"] = "Novi korisnik sa korisnickim imenom " + userModel.Username + " je uspesno kreiran";
                    return RedirectToAction("admin");
                }

            }
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
                userModel.Role = "user";
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
                Session["role"] = foundInstance.Role;
                TempData["message"] = "Uspesno prijavljivanje. Dobrodosli nazad, " + foundInstance.Username;

                if (foundInstance.Role.Equals("admin"))
                {
                    return RedirectToAction("admin");
                }
                else if (foundInstance.Role.Equals("seller"))
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
            Session["role"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ViewUsers()
        {
            IEnumerable<UserModel> data = UserModel.GetUsers();
            List<SelectListItem> userRoles = new List<SelectListItem>();
            userRoles.Add(new SelectListItem() { Text = "Prodavac", Value = "seller" });
            userRoles.Add(new SelectListItem() { Text = "Korisnik", Value = "user" });


            SelectList userRolesList = new SelectList(userRoles,"Value","Text");
            ViewBag.userRoles = userRolesList;
            ViewBag.checkedRoles = new List<String>();

            return View(data);
        }
        public ActionResult Search(String FirstName, String LastName, List<String> Role, String Sort) {

            List<UserModel> users = UserModel.GetUsers();
            for (int i = users.Count - 1; i >=0; i--) {
                UserModel user = users[i];
                if (!user.FirstName.ToLower().Contains(FirstName.ToLower())) {
                    users.RemoveAt(i);
                }
            }
            for (int i = users.Count - 1; i >= 0; i--)
            {
                UserModel user = users[i];
                if (!user.LastName.ToLower().Contains(LastName.ToLower()))
                {
                    users.RemoveAt(i);
                }
            }
            if (Role != null)
            {
                for (int i = users.Count - 1; i >= 0; i--)
                {
                    UserModel user = users[i];
                    if (!Role.Contains(user.Role))
                    {
                        users.RemoveAt(i);
                    }
                }

            }

            List<SelectListItem> userRoles = new List<SelectListItem>();
            userRoles.Add(new SelectListItem() { Text = "Prodavac", Value = "seller" });
            userRoles.Add(new SelectListItem() { Text = "Korisnik", Value = "user" });
            SelectList userRolesList = new SelectList(userRoles, "Value", "Text");
            ViewBag.userRoles = userRolesList;
            if (Role != null)
            {
                ViewBag.checkedRoles = Role;
            }
            else
            {
                ViewBag.checkedRoles = new List<String>();
            }
            return View("ViewUsers",users);
        }
        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult Seller()
        {
            if (Session["username"] != null && Session["role"].Equals("seller"))
            {
                return View("Seller");
            }
            else
            {
                TempData["error"] = "Morate biti registrovani kao menadzer kako biste pristupili ovom delu sajta";
                return Redirect("~/");
            }

        }
        public ActionResult Delete(string username)
        {
            if (Session["role"] != null && Session["role"].Equals("admin"))
            {
                UserModel userModel = UserModel.GetUser(username);
                if (userModel.delete())
                {
                    TempData["message"] = "Uspesno uklonjen korisnik iz baze podataka";
                }
                else
                {
                    TempData["error"] = "Doslo je do greske prilikom uklanjanja korisnika iz baze podataka";
                }
                return RedirectToRoute("User/Admin");

            }
            else
            {
                TempData["error"] = "Nemate pristup uklanjanju korisnika. Morate biti ulogovani kao administrator kako biste imali pristup ovoj funkciji";
                return Redirect("~/");
            }
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