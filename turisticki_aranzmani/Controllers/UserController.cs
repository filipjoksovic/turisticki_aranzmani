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
        public ActionResult BlockUser(String id) {
            if (Session["role"].Equals("admin"))
            {
                UserModel userToBlock = UserModel.GetUser(id);
                userToBlock.blockUser();
                TempData["message"] = "Korisnik je uspesno blokiran";
                return RedirectToRoute("User/Admin");
            }
            else { 
                TempData["message"] = "Nemate pristup ovom delu sajta";
                return Redirect("~/");
            }
        }
        public ActionResult UnblockUser(String id) {
            if (Session["role"].Equals("admin"))
            {
                UserModel userToUnblock = UserModel.GetUser(id);
                userToUnblock.unblockUser();
                TempData["message"] = "Korisnik je uspesno odblokiran";
                return RedirectToRoute("User/Admin");

            }
            else {
                TempData["error"] = "Nemate pristup ovom delu sajta";
                return Redirect("~/");

            }
        }
        public ActionResult DisplaySuspicious() {

            dynamic BlackList = new ExpandoObject();
            BlackList.BlockedUsers = BlackListModel.GetBlockedUsers();
            BlackList.UsersToBlock = UserModel.GetUsersToBlock();
            for (int i = BlackList.UsersToBlock.Count - 1; i >= 0; i--) {
                if (BlackList.UsersToBlock[i].isBlocked()) {
                    BlackList.UsersToBlock.RemoveAt(i);
                }
            }
            return View(BlackList);
            return View();
        }
        public ActionResult Account()
        {
            if (Session["username"] == null)
            {
                TempData["error"] = "Morate biti ulogovani kako biste mogli da pristupite Vasem nalogu";
                return Redirect("~/");
            }
            else
            {
                dynamic account = new ExpandoObject();
                account.User = UserModel.GetUser(Session["username"].ToString());

                account.Reservations = Utility.GetDetailedReservations(ReservationModel.getAllItems(Session["username"].ToString()));
                return View(account);
            }
        }
        [HttpPost]
        public ActionResult Account(FormCollection collection)
        {
            if (Session["username"] == null)
            {
                TempData["error"] = "Morate biti ulogovani kako biste mogli da pristupite Vasem nalogu";
                return Redirect("~/");
            }
            else
            {
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
            if (!foundInstance.isBlocked())
            {
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
            else {
                TempData["error"] = "Vas nalog je privremeno blokiran od strane administratora";
                return Redirect("~/");
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

            List<SelectListItem> sort = new List<SelectListItem>();
            sort.Add(new SelectListItem() { Text = "Ime rastuce", Value = "fnameAsc" });
            sort.Add(new SelectListItem() { Text = "Ime opadajuce", Value = "fnameDesc" });
            sort.Add(new SelectListItem() { Text = "Prezime rastuce", Value = "lnameAsc" });
            sort.Add(new SelectListItem() { Text = "Prezime opadajuce", Value = "lnameDesc" });
            sort.Add(new SelectListItem() { Text = "Uloga rastuce", Value = "roleAsc" });
            sort.Add(new SelectListItem() { Text = "Uloga opadajuce", Value = "roleDesc" });

            SelectList userRolesList = new SelectList(userRoles, "Value", "Text");
            SelectList sortList = new SelectList(sort, "Value", "Text");
            ViewBag.userRoles = userRolesList;
            ViewBag.sortList = sortList;
            ViewBag.checkedRoles = new List<String>();

            return View(data);
        }
        public ActionResult Search(String FirstName, String LastName, List<String> Role, String Sort)
        {

            List<UserModel> users = UserModel.GetUsers();
            for (int i = users.Count - 1; i >= 0; i--)
            {
                UserModel user = users[i];
                if (!user.FirstName.ToLower().Contains(FirstName.ToLower()))
                {
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
            if (Sort != null) {
                if (Sort.Equals("fnameAsc")) {
                    users.OrderBy(user => user.FirstName);
                }
                if (Sort.Equals("fnameDesc")) { 
                    users.OrderBy(user => user.FirstName);
                    users.Reverse();
                }
                if (Sort.Equals("lnameAsc"))
                {
                    users.OrderBy(user => user.LastName);
                }
                if (Sort.Equals("lnameDesc"))
                {
                    users.OrderBy(user => user.LastName);
                    users.Reverse();
                }
                if (Sort.Equals("roleAsc"))
                {
                    users.OrderBy(user => user.Role);
                }
                if (Sort.Equals("roleDesc"))
                {
                    users.OrderBy(user => user.Role);
                    users.Reverse();
                }
            }

            List<SelectListItem> userRoles = new List<SelectListItem>();
            userRoles.Add(new SelectListItem() { Text = "Prodavac", Value = "seller" });
            userRoles.Add(new SelectListItem() { Text = "Korisnik", Value = "user" });
            SelectList userRolesList = new SelectList(userRoles, "Value", "Text");
            ViewBag.userRoles = userRolesList;
            List<SelectListItem> sort = new List<SelectListItem>();
            sort.Add(new SelectListItem() { Text = "Ime rastuce", Value = "fnameAsc" });
            sort.Add(new SelectListItem() { Text = "Ime opadajuce", Value = "fnameDesc" });
            sort.Add(new SelectListItem() { Text = "Prezime rastuce", Value = "lnameAsc" });
            sort.Add(new SelectListItem() { Text = "Prezime opadajuce", Value = "lnameDesc" });
            sort.Add(new SelectListItem() { Text = "Uloga rastuce", Value = "roleAsc" });
            sort.Add(new SelectListItem() { Text = "Uloga opadajuce", Value = "roleDesc" });
            SelectList sortList = new SelectList(sort, "Value", "Text", Sort);
            ViewBag.sortList = sortList;

            if (Role != null)
            {
                ViewBag.checkedRoles = Role;
            }
            else
            {
                ViewBag.checkedRoles = new List<String>();
            }
            return View("ViewUsers", users);
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