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
    }
}