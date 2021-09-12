using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using turisticki_aranzmani.Helpers;
using System.ComponentModel;
namespace turisticki_aranzmani.Models
{
    public class ResidenceItemModel
    {
        private List<String> fileContent;
        private String path = HttpContext.Current.Server.MapPath("~/App_Data/residence_units.csv");
        private static String static_path = HttpContext.Current.Server.MapPath("~/App_Data/residence_units.csv");

        public int ID { get; set; }
        [DisplayName("Sifra smestajne jedinice")]
        public int ResidenceID { get; set; }
        [DisplayName("Naziv smestajne jedinice")]
        public string UnitName { get; set; }

        [DisplayName("Maksimalan broj gostiju")]
        public int MaxGuests { get; set; }
        [DisplayName("Dozvoljeni ljubimci")]
        public Boolean AllowPets { get; set; }
        [DisplayName("Cena smestaja")]
        public int Price { get; set; }

        public ResidenceItemModel()
        {
            this.fileContent = FileObjectSerializer.GetFileContent(path);
            this.ID = FileObjectSerializer.GetInsertID(path);

        }
        public ResidenceItemModel(String[] fields) {
            this.ID = Convert.ToInt32(fields[0]);
            this.ResidenceID = Convert.ToInt32(fields[1]);
            this.UnitName = fields[2];
            this.MaxGuests = Convert.ToInt32(fields[3]);
            this.AllowPets = Convert.ToBoolean(fields[4]);
            this.Price = Convert.ToInt32(fields[5]);
        }
        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5};{6}", this.ID, this.ResidenceID, this.UnitName, this.MaxGuests, this.AllowPets, this.Price, Environment.NewLine);
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
        public static List<ResidenceItemModel> getAllItems(int id) {
            List<ResidenceItemModel> allItems = ResidenceItemModel.getAllItems();
            //for (int i = allItems.Count - 1; i >= 0; i--) {
            //    if (allItems[i].ResidenceID != id) {
            //        allItems.RemoveAt(i);
            //    }
            //}
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
                bool writeRestult = FileObjectSerializer.AppendToFile(path, this.ToString());
                return writeRestult;
            }
        }
    }
}