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
            modelInstance.ResidenceName = ResidenceModel.getResidenceName(model.ResidenceID);
            modelInstance.Residence = ResidenceModel.GetByID(model.ResidenceID);
            return modelInstance;
        }
        public static List<dynamic> GetDetailedReservations(List<ReservationModel> reservations)
        {
            List<dynamic> expandedReservations = new List<dynamic>();
            foreach (ReservationModel model in reservations)
            {
                dynamic expandedModel = Utility.ToExpandoObject(model);
                expandedModel.ArrangementName = ArrangementModel.GetByID(model.arrangement_id).Name;
                expandedModel.ResidenceUnit = ResidenceItemModel.GetByID(model.residence_item_id).UnitName;
                expandedModel.Status = model.status == 0 ? "Aktivna" : "Otkazana";
                expandedModel.ArrangementImage = ArrangementModel.GetByID(model.arrangement_id).ImagePath;

                expandedReservations.Add(expandedModel);
            }
            return expandedReservations;
        }
    }
}