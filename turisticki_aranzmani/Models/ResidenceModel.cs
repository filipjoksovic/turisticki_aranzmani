using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turisticki_aranzmani.Helpers;

using System.ComponentModel;

namespace turisticki_aranzmani.Models
{
    public class ResidenceModel
    {
        String path = HttpContext.Current.Server.MapPath("~/App_Data/residences.csv");
        [DisplayName("ID")]
        public int ID { get; set; }
        public String Username { get; set; }
        [DisplayName("Tip smestaja")]
        public String BuildingType { get; set; }
        [DisplayName("Naziv smestaja")]
        public String Name { get; set; }
        [DisplayName("Broj zvezda")]
        public int StarRating { get; set; }
        [DisplayName("Ima bazen")]
        public Boolean HasPool { get; set; }
        [DisplayName("Ima spa centar")]
        public Boolean HasSpa { get; set; }
        [DisplayName("Zadovoljave standarde za osobe sa invaliditetom")]
        public Boolean DisabilityApproved { get; set; }
        [DisplayName("Ima Wifi")]
        public Boolean HasWifi { get; set; }

        public override string ToString()
        {
            return this.ID + ";" +this.Username + ";" + this.Name + ";" + this.BuildingType + ";" + this.StarRating + ";" + this.HasPool + ";" + this.HasSpa + ";" + this.DisabilityApproved + ";" + this.HasSpa + Environment.NewLine;
        }

        public ResidenceModel()
        {
            this.ID = FileObjectSerializer.GetInsertID(path);
        }
        public ResidenceModel(String[] fields)
        {
            this.ID = Convert.ToInt32(fields[0]);
            this.Name = fields[1];
            this.StarRating = Convert.ToInt32(fields[2]);
            this.HasPool = Convert.ToBoolean(fields[3]);
            this.HasSpa = Convert.ToBoolean(fields[4]);
            this.DisabilityApproved = Convert.ToBoolean(fields[5]);
            this.HasWifi = Convert.ToBoolean(fields[6]);
        }

        public static List<ResidenceModel> getAllItems()
        {
            List<ResidenceModel> allItems = new List<ResidenceModel>();
            String filePath = HttpContext.Current.Server.MapPath("~/App_Data/residences.csv");
            List<String> fileContent = FileObjectSerializer.GetFileContent(filePath);
            foreach (String dataRow in fileContent)
            {
                ResidenceModel modelInstance = new ResidenceModel(dataRow.Split(';'));
                allItems.Add(modelInstance);
            }
            return allItems;
        }
        public Boolean save()
        {
            bool writeResult = FileObjectSerializer.AppendToFile(path, this.ToString());
            return writeResult;
        }
    }
}