using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace turisticki_aranzmani.Models
{
    public class PlaceModel
    {
        private String table = HttpContext.Current.Server.MapPath("~/App_Data/places.csv");

        public int PlaceID { get; set; }
        public String Street { get; set; }
        public String StreetNumber { get; set; }
        public String City { get; set; }
        public String ZipCode { get; set; }
        public String Longitute { get; set; }
        public String Latitude { get; set; }

        public PlaceModel()
        {

        }
        public PlaceModel(String[] fields)
        {
            this.PlaceID = PlaceModel.getInsertID();
            this.Street = fields[0];
            this.StreetNumber = fields[1];
            this.City = fields[2];
            this.ZipCode = fields[3];
            this.Longitute = fields[4];
            this.Latitude = fields[5];
        }

        public override string ToString()
        {
            return this.PlaceID + ";" + this.Street+ ";" + this.StreetNumber + ";" + this.City + ";" + this.ZipCode + ";" + this.Longitute + ";" + this.Latitude + Environment.NewLine;
        }

        public static List<PlaceModel> getAllPlaces()
        {
            String table = HttpContext.Current.Server.MapPath("~/App_Data/places.csv");

            List<String> fileContent = System.IO.File.ReadAllLines(table).ToList();
            List<PlaceModel> allPlaces = new List<PlaceModel>();
            foreach (String row in fileContent)
            {
                allPlaces.Add(new PlaceModel(row.Split(';')));
            }
            return allPlaces;

        }
        private static int getInsertID()
        {
            int max_id = 1;
            foreach(PlaceModel place in PlaceModel.getAllPlaces()) {
                if (max_id <= place.PlaceID) {
                    max_id = place.PlaceID + 1;
                }
            }
            return max_id;

        }
        public Boolean save() {
            try
            {
                System.IO.File.AppendAllText(table, this.ToString());
                return true;
            }
            catch (IOException e) {
                return false;
            }
            
        }
        public Boolean delete()
        {
            try {
                List<PlaceModel> allPlaces = PlaceModel.getAllPlaces();
                for (int i = allPlaces.Count; i >= 0; i--) {
                    PlaceModel place = allPlaces[i];
                    if (place.PlaceID == this.PlaceID) {
                        allPlaces.Remove(place);
                        break;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean update()
        {
            try {
                bool found = false;
                List<PlaceModel> allPlaces = PlaceModel.getAllPlaces();
                for (int i = allPlaces.Count; i >= 0; i--) {
                    PlaceModel place = allPlaces[i];
                    if (place.PlaceID == this.PlaceID) {
                        place.Latitude = this.Latitude;
                        place.Longitute = this.Longitute;
                        place.Street = this.Street;
                        place.StreetNumber = this.StreetNumber;
                        place.ZipCode = this.ZipCode;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    //writing to files to be done if needed
                    return true;    
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


    }
}