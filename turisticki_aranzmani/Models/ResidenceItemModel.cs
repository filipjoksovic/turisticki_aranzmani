using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using turisticki_aranzmani.Helpers;
namespace turisticki_aranzmani.Models
{
    public class ResidenceItemModel
    {
        private List<String> fileContent;
        private String path = HttpContext.Current.Server.MapPath("~/App_Data/seller_appartments.csv");
        private static String static_path = HttpContext.Current.Server.MapPath("~/App_Data/seller_appartments.csv");

        public int ID { get; set; }
        public int MaxGuests { get; set; }
        public Boolean AllowPets { get; set; }
        public int Price { get; set; }

        public ResidenceItemModel()
        {
            this.fileContent = FileObjectSerializer.GetFileContent(path);

        }
        public ResidenceItemModel(String[] fields) {
            this.ID = FileObjectSerializer.GetInsertID(this.path);
        }

        public Boolean exists()
        {
            List<String> fileContent = FileObjectSerializer.GetFileContent(path);
            foreach (String model in fileContent)
            {
                if (model.ToString().Equals(this.ToString()))
                    return true;
            }
            return false;
        }

        public static List<ResidenceItemModel> getAllItems()
        {
            List<ResidenceItemModel> allItems = new List<ResidenceItemModel>();
            List<String> fileContent = FileObjectSerializer.GetFileContent(static_path);

            foreach (String row in fileContent)
            {
                allItems.Add(new ResidenceItemModel(row.Split(';')));
            }
            return allItems;
        }
        public Boolean save()
        {
            if (this.exists())
            {
                return false;
            }
            else
            {
                bool writeRestult = FileObjectSerializer.WriteToFile(path, this.ToString());
                return writeRestult;
            }
        }
    }
}