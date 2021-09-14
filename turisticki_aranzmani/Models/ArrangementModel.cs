using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Models
{
    [Bind]
    public class ArrangementModel
    {
        private String path = HttpContext.Current.Server.MapPath("~/App_Data/arrangements.csv");
        private static String pathstatic = HttpContext.Current.Server.MapPath("~/App_Data/arrangements.csv");

        public int ID { get; set; }
        public String Username { get; set; }
        public String Name { get; set; }
        public int TypeID { get; set; }
        public int DriveTypeID { get; set; }
        public String Location { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int StartingPointID { get; set; }//gets information from placeModel
        public DateTime TimeStarting { get; set; }
        public int MaxCustomers { get; set; }
        public String Description { get; set; }
        public String Programme { get; set; }
        public String ImagePath { get; set; }
        public int ResidenceID { get; set; }//gets information from residenceModel
        public String FormattedDateStart { get { return this.DateStart.ToString("dd/M/yyyy"); } }
        public String FormattedDateEnd { get { return this.DateStart.ToString("dd/M/yyyy"); } }
        public String FormattedTimeStarting { get { return this.TimeStarting.ToString("H:mm"); } }


        public ArrangementModel()
        {
            this.ID = FileObjectSerializer.GetInsertID(this.path);
        }
        public ArrangementModel(String[] fields)
        {
            this.ID = Convert.ToInt32(fields[0]);
            this.Name = fields[1];
            this.Username = fields[2];
            this.TypeID = Convert.ToInt32(fields[3]);
            this.DriveTypeID = Convert.ToInt32(fields[4]);
            this.Location = fields[5];
            this.DateStart = DateTime.Parse(fields[6]);
            this.DateEnd = DateTime.Parse(fields[7]);
            this.StartingPointID = Convert.ToInt32(fields[8]);
            this.TimeStarting = DateTime.Parse(fields[9]);
            this.MaxCustomers = Convert.ToInt32(fields[10]);
            this.Description = fields[11];
            this.Programme = fields[12];
            this.ImagePath = fields[13];
            this.ResidenceID = Convert.ToInt32(fields[14]);//gets information from residenceModel
        }
        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15}", this.ID, this.Name, this.Username, this.TypeID, this.DriveTypeID, this.Location, this.DateStart, this.DateEnd, this.StartingPointID, this.TimeStarting, this.MaxCustomers, this.Description, this.Programme, this.ImagePath, this.ResidenceID, Environment.NewLine);
        }
        public static List<ArrangementModel> getAllItems()
        {
            List<String> fileContent = FileObjectSerializer.GetFileContent(pathstatic);
            List<ArrangementModel> allModels = new List<ArrangementModel>();
            foreach (String row in fileContent)
            {
                ArrangementModel am = new ArrangementModel(row.Split(';'));
                allModels.Add(am);
            }
            return allModels;

        }
        public static List<ArrangementModel> getAllItems(String username) {
            List<String> fileContent = FileObjectSerializer.GetFileContent(pathstatic);
            List<ArrangementModel> allModels = new List<ArrangementModel>();
            foreach (String row in fileContent)
            {
                ArrangementModel am = new ArrangementModel(row.Split(';'));
                if(am.Username.Equals(username))
                allModels.Add(am);
            }
            return allModels;
        }
        public static List<ArrangementModel> getAllItems(int residence_id) {
            List<String> fileContent = FileObjectSerializer.GetFileContent(pathstatic);
            List<ArrangementModel> allModels = new List<ArrangementModel>();
            foreach (String row in fileContent) {
                ArrangementModel am = new ArrangementModel(row.Split(';'));
                if(am.ResidenceID == residence_id)
                {
                    allModels.Add(am);
                }
            }
            return allModels;
        }
        public static ArrangementModel GetByID(int id)
        {
            foreach (ArrangementModel am in ArrangementModel.getAllItems())
            {
                if (id == am.ID)
                {
                    return am;
                }
            }
            return null;
        }
        public String getArrangementImage()
        {
            return HttpContext.Current.Server.MapPath("~/App_Data/arrangement_images/") + this.ImagePath;
        }
        public Boolean exists()
        {
            foreach (ArrangementModel am in ArrangementModel.getAllItems())
            {
                if (am.Name.Equals(this.Name))
                {
                    return true;
                }
            }
            return false;
        }
        public Boolean save()
        {
            if (!this.exists())
            {
                FileObjectSerializer.AppendToFile(this.path, this.ToString());
                return true;
            }
            return false;
        }
        public String getRideName
        {
            get
            {
                return RideTypeModel.getRideName(this.DriveTypeID);
            }
        }
        public String getArrangementType
        {
            get
            {
                return ArrangementTypeModel.getTypeName(this.TypeID);
            }
        }
        public Boolean delete() {
            PlaceModel arrangementPlace = PlaceModel.GetByID(this.StartingPointID);
            FileObjectSerializer.Delete(this.path, this.ToString());
            arrangementPlace.delete();
            return true;
        }
    }
}