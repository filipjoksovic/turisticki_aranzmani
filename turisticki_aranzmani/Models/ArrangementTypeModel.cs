using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Models
{
    public class ArrangementTypeModel
    {
        public int ID { get; set; }
        public String name { get; set; }
        public ArrangementTypeModel(String[] fields)
        {
            this.ID = Convert.ToInt32(fields[0]);
            this.name = fields[1];
        }
        public static List<ArrangementTypeModel> getAllItems()
        {
            List<String> fileContent = FileObjectSerializer.GetFileContent(HttpContext.Current.Server.MapPath("~/App_Data/arrangement_types.csv"));
            List<ArrangementTypeModel> allItems = new List<ArrangementTypeModel>();
            foreach (String line in fileContent)
            {
                allItems.Add(new ArrangementTypeModel(line.Split(';')));
            }
            return allItems;
        }
        public static String getTypeName(int id)
        {
            foreach (ArrangementTypeModel tm in ArrangementTypeModel.getAllItems())
            {
                if (tm.ID == id)
                {
                    return tm.name;
                }
            }
            return null;
        }
    }

}