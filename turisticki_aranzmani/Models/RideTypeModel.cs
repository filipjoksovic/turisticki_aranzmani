using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Models
{
    public class RideTypeModel
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public RideTypeModel(String[] fields) {
            this.ID = Convert.ToInt32(fields[0]);
            this.Name = fields[1];
        }
        public static List<RideTypeModel> getAllItems()
        {
            List<RideTypeModel> allItems = new List<RideTypeModel>();
            List<String> fileContent = FileObjectSerializer.GetFileContent(HttpContext.Current.Server.MapPath("~/App_Data/ride_types.csv"));
            foreach (String row in fileContent) {
                allItems.Add(new RideTypeModel(row.Split(';')));                
            }
            return allItems;
        }
        public static String getRideName(int id) {
            foreach(RideTypeModel model in RideTypeModel.getAllItems()) {
                if (model.ID == id) {
                    return model.Name;
                }
            }
            return null;
        }
    }
}