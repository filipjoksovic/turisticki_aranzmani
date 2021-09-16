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
        public ActionResult ListReviews(String role, String id)
        {
            if (Session["role"] == null || !Session["role"].Equals(role))
            {
                TempData["error"] = "Nemate dozvolu pristupa ovom delu sajta";
                return Redirect("~/");
            }
            else
            {
                List<ArrangementCommentModel> reviews = new List<ArrangementCommentModel>();

                if (id != null)
                {
                    int arrangement_id = Convert.ToInt32(id);
                    reviews = ArrangementCommentModel.GetComments(arrangement_id);
                    if (reviews != null)
                    {
                        ViewBag.ArrangementTitle = ArrangementModel.GetByID(arrangement_id).Name;
                    }
                }
                else
                {
                    reviews = ArrangementCommentModel.GetComments();
                }
                List<dynamic> ExpandedReviews = new List<dynamic>();
                foreach (ArrangementCommentModel comment in reviews)
                {
                    dynamic ExpandedReview = Utility.ToExpandoObject(comment);
                    ExpandedReview.ArrangementName = ArrangementModel.GetByID(comment.ArrangementID).Name;
                    ExpandedReviews.Add(ExpandedReview);
                }

                return View(ExpandedReviews);
            }
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
        public ActionResult AllowReview(String id)
        {
            if (id != null)
            {
                if (Session["role"] == null)
                {
                    TempData["error"] = "Morate biti ulogvani kako biste mogli da odobrite ocenu";
                    return Redirect("~/");
                }
                else
                {
                    if (Session["role"].Equals("admin"))
                    {
                        ArrangementCommentModel comment = ArrangementCommentModel.GetByID(Convert.ToInt32(id));
                        comment.Allowed = true;
                        if (comment.update())
                        {
                            TempData["message"] = "Komentar je uspesno odobren";
                        }
                        else
                        {
                            TempData["error"] = "Doslo je do greske prilikom odobravanja komentara";
                        }
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    else if (Session["role"].Equals("seller"))
                    {
                        int arrangement_id = Convert.ToInt32(id);
                        ArrangementModel arrangement = ArrangementModel.GetByID(arrangement_id);
                        if (arrangement.Username.Equals(Session["username"].ToString()))
                        {
                            ArrangementCommentModel comment = ArrangementCommentModel.GetByID(Convert.ToInt32(id));
                            comment.Allowed = true;
                            if (comment.update())
                            {
                                TempData["message"] = "Komentar je uspesno odobren";
                            }
                            else
                            {
                                TempData["error"] = "Doslo je do greske prilikom odobravanja komentara";
                            }
                            return Redirect(Request.UrlReferrer.ToString());

                        }
                        else
                        {
                            TempData["error"] = "Nemate dozvolu pristupa ovom delu sajta";
                            return Redirect("~/");
                        }
                    }
                    else
                    {
                        TempData["error"] = "Nemate dozvolu pristupa ovom delu sajta";
                        return Redirect("~/");
                    }
                }
            }
            else
            {
                TempData["erorr"] = "Sifra ocene nije postavljena";
                return Redirect("~/");
            }
        }
        public ActionResult DenyReview(String id)
        {
            if (id != null)
            {
                if (Session["role"] == null)
                {
                    TempData["error"] = "Morate biti ulogvani kako biste mogli da odobrite ocenu";
                    return Redirect("~/");
                }
                else
                {
                    if (Session["role"].Equals("admin"))
                    {
                        ArrangementCommentModel comment = ArrangementCommentModel.GetByID(Convert.ToInt32(id));
                        comment.Allowed = false;
                        if (comment.update())
                        {
                            TempData["message"] = "Komentar je uspesno odbijen";
                        }
                        else
                        {
                            TempData["error"] = "Doslo je do greske prilikom odbijanja komentara";
                        }
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    else if (Session["role"].Equals("seller"))
                    {
                        int arrangement_id = Convert.ToInt32(id);
                        ArrangementModel arrangement = ArrangementModel.GetByID(arrangement_id);
                        if (arrangement.Username.Equals(Session["username"].ToString()))
                        {
                            ArrangementCommentModel comment = ArrangementCommentModel.GetByID(Convert.ToInt32(id));
                            comment.Allowed = false;
                            if (comment.update())
                            {
                                TempData["message"] = "Komentar je uspesno odobren";
                            }
                            else
                            {
                                TempData["error"] = "Doslo je do greske prilikom odobravanja komentara";
                            }
                            return Redirect(Request.UrlReferrer.ToString());

                        }
                        else
                        {
                            TempData["error"] = "Nemate dozvolu pristupa ovom delu sajta";
                            return Redirect("~/");
                        }
                    }
                    else
                    {
                        TempData["error"] = "Nemate dozvolu pristupa ovom delu sajta";
                        return Redirect("~/");
                    }
                }
            }
            else
            {
                TempData["erorr"] = "Sifra ocene nije postavljena";
                return Redirect("~/");
            }
        }
    }
}