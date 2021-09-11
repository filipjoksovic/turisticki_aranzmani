using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turisticki_aranzmani.Models
{
    public class ArrangementModel
    {
        private int ID { get; set; }
        private String Name { get; set; }
        private int TypeID { get; set; }
        private int DriveTypeID { get; set; }
        private String Location { get; set; }
        private DateTime DateStart { get; set; }
        private DateTime DateEnd { get; set; }
        private int StartingPointID { get; set; }//gets information from placeModel
        private DateTime TimeStarting { get; set; }
        private int MaxCustomers { get; set; }
        private String Description { get; set; }
        private String Programme { get; set; }
        private String ImagePath { get; set; }
        private int ResidenceID { get; set; }//gets information from residenceModel

        public ArrangementModel()
        {

        }
        public ArrangementModel(String[] fields)
        {
            this.ID = Convert.ToInt32(fields[0]);
            this.Name = fields[1];
            this.TypeID = Convert.ToInt32(fields[2]);
            this.DriveTypeID = Convert.ToInt32(fields[3]);
            this.Location = fields[4];
            this.DateStart = DateTime.Parse(fields[5]);
            this.DateEnd = DateTime.Parse(fields[6]);
            this.StartingPointID = Convert.ToInt32(fields[7]);
            this.TimeStarting = DateTime.Parse(fields[8]);
            this.MaxCustomers = Convert.ToInt32(fields[9]);
            this.Description = fields[10];
            this.Programme = fields[11];
            this.ImagePath = fields[12];
            this.ResidenceID = Convert.ToInt32(fields[13]);//gets information from residenceModel
        }
    }
}