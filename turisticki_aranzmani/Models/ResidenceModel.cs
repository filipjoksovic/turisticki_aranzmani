using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Models
{
    public class ResidenceModel
    {
        String path = HttpContext.Current.Server.MapPath("~/App_Data/residences.csv");
        public int ID { get; set; }
        public String Name { get; set; }
        public int StarRating{ get; set; }
        public Boolean HasPool { get; set; }
        public Boolean HasSpa { get; set; }
        public Boolean DisabilityApproved { get; set; }
        public Boolean HasWifi { get; set; }

        public override string ToString()
        {
            return this.ID + ";" + this.Name + ";" + this.StarRating + ";" + this.HasPool + ";" + this.HasSpa + ";" + this.DisabilityApproved + ";" + this.HasSpa + Environment.NewLine;
        }

        public ResidenceModel() {
            this.ID = FileObjectSerializer.GetInsertID(path);
        }
        public ResidenceModel(String[] fields) {
            this.ID = Convert.ToInt32(fields[0]);
            this.Name = fields[1];
            this.StarRating = Convert.ToInt32(fields[2]);
            this.HasPool = Convert.ToBoolean(fields[3]);
            this.HasSpa = Convert.ToBoolean(fields[4]);
            this.DisabilityApproved = Convert.ToBoolean(fields[5]);
            this.HasWifi = Convert.ToBoolean(fields[6]);
        }

        public static List<ResidenceModel> getAllItems() {
            List<ResidenceModel> allItems = new List<ResidenceModel>();
            String filePath = HttpContext.Current.Server.MapPath("~/App_Data/residences.csv");
            List<String> fileContent = FileObjectSerializer.GetFileContent(filePath);
            foreach (String dataRow in fileContent) {
                ResidenceModel modelInstance = new ResidenceModel(dataRow.Split(';'));
                allItems.Add(modelInstance);
            }
            return allItems;
        }
        public Boolean save() {
            bool writeResult = FileObjectSerializer.WriteToFile(path, this.ToString());
            return writeResult;
        }
    }
}