using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using turisticki_aranzmani.Models;

namespace turisticki_aranzmani.Helpers
{
    public class Utility
    {
        public static ExpandoObject ToExpandoObject(Object o) {
            dynamic expandedModel = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expandedModel;

            foreach (var property in o.GetType().GetProperties())
                dictionary.Add(property.Name, property.GetValue(o));
            return expandedModel;
        }
        public static ExpandoObject ExpandArrangement(ArrangementModel model)
        {
            dynamic modelInstance = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)modelInstance;

            foreach (var property in model.GetType().GetProperties())
                dictionary.Add(property.Name, property.GetValue(model));
            modelInstance.RideTypeName = RideTypeModel.getRideName(model.DriveTypeID);
            modelInstance.ArrangementTypeName = ArrangementTypeModel.getTypeName(model.TypeID);
            modelInstance.GroupingPlace = PlaceModel.PrettifyPlace(model.StartingPointID);
            modelInstance.PlaceObject = PlaceModel.GetByID(model.StartingPointID);
            modelInstance.ResidenceName = ResidenceModel.getResidenceName(model.ResidenceID);
            modelInstance.Residence = ResidenceModel.GetByID(model.ResidenceID);
            if (HttpContext.Current.Session["username"] != null) {
                String username = HttpContext.Current.Session["username"].ToString();
                modelInstance.CanReview = model.canReview(username);
                modelInstance.HasReview = model.hasReview(username);
                if (modelInstance.HasReview) {
                    modelInstance.Comment = ArrangementCommentModel.GetComment(model.ID, username);
                }
            }
            else
            {
                modelInstance.CanReview = false;
                modelInstance.HasReview = false;
            }
            return modelInstance;

        }
        public static List<dynamic> GetDetailedReservations(List<ReservationModel> reservations)
        {
            List<dynamic> expandedReservations = new List<dynamic>();
            foreach (ReservationModel model in reservations)
            {
                dynamic expandedModel = Utility.ToExpandoObject(model);
                ArrangementModel am = ArrangementModel.GetByID(model.arrangement_id);
                expandedModel.ArrangementName = am.Name;
                expandedModel.ResidenceUnit = ResidenceItemModel.GetByID(model.residence_item_id).UnitName;
                expandedModel.Status = model.status == 0 ? "Aktivna" : "Otkazana";
                expandedModel.ArrangementImage = am.ImagePath;
                expandedModel.DateEnd = am.DateEnd;
                expandedModel.ArrangementID = am.ID;
                expandedReservations.Add(expandedModel);
            }
            return expandedReservations;
        }
    }
}