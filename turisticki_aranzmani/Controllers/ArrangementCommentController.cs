using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Models;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Controllers
{
    public class ArrangementCommentController : Controller
    {
        // GET: ArrangementCommentt
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DeleteReview(String role, int id)
        {
            if (Session["role"] == null || !Session["role"].Equals(role))
            {
                TempData["error"] = "Nemate dozvolu da uklonite ovu ocenu";
                return Redirect("~/");
            }
            else
            {
                ArrangementCommentModel reviewInstance = ArrangementCommentModel.GetByID(id);
                if (reviewInstance.delete())
                {
                    TempData["message"] = "Komentar je uspesno izmenjen";
                }
                else
                {
                    TempData["error"] = "Doslo je do greske prilikom uklanjanja komentara";
                }
                return RedirectToRoute("User/Account");
            }
        }
        public ActionResult LeaveReview(FormCollection collection)
        {
            if (Session["username"] == null)
            {
                TempData["error"] = "Morate biti ulogovani kako biste mogli ostaviti ocenu na aranzman";
                return Redirect("~/");
            }
            else
            {
                int arrangementID = Convert.ToInt32(collection["ArrangementID"]);
                String username = Session["username"].ToString();
                ArrangementModel arrangementModel = ArrangementModel.GetByID(arrangementID);
                if (arrangementModel.canReview(username))
                {
                    ArrangementCommentModel review = new ArrangementCommentModel();
                    review.Grade = Convert.ToInt32(collection["Grade"]);
                    review.Comment = collection["Comment"];
                    review.Username = username;
                    review.ArrangementID = arrangementModel.ID;
                    if (review.save())
                    {
                        TempData["message"] = "Ocena uspesno ostavljena";
                        return Redirect("~/");
                    }
                    else
                    {
                        TempData["error"] = "Doslo je do greske prilikom ostavljanja ocene na aranzman";
                        return Redirect("~/");
                    }
                }
                else
                {
                    TempData["error"] = "Trenutno ne mozete ostaviti ocenu na dati aranzman";
                    return Redirect("~/");
                }
            }
        }
        public ActionResult EditReview(FormCollection collection)
        {
            if (Session["username"] == null)
            {
                TempData["error"] = "Morate biti ulogovani kako biste mogli ostaviti ocenu na aranzman";
                return Redirect("~/");
            }
            else
            {
                int reviewID = Convert.ToInt32(collection["ReviewID"]);
                ArrangementCommentModel review = ArrangementCommentModel.GetByID(reviewID);
                review.Grade = Convert.ToInt32(collection["Grade"]);
                review.Comment = collection["Comment"];
                if (review.update())
                {
                    TempData["message"] = "Ocena uspesno izmenjena";
                    return Redirect("~/");
                }
                else
                {
                    TempData["error"] = "Doslo je do greske prilikom izmene ocene na aranzmanu";
                    return Redirect("~/");
                }

            }
        }
    }
}