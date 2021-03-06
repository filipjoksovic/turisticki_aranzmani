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
            return this.ID + ";" + this.Username + ";" + this.Name + ";" + this.BuildingType + ";" + this.StarRating + ";" + this.HasPool + ";" + this.HasSpa + ";" + this.DisabilityApproved + ";" + this.HasSpa + Environment.NewLine;
        }

        public ResidenceModel()
        {
            this.ID = FileObjectSerializer.GetInsertID(path);
        }
        public ResidenceModel(String[] fields)
        {
            this.ID = Convert.ToInt32(fields[0]);
            this.Username = fields[1];
            this.Name = fields[2];
            this.BuildingType = fields[3];
            this.StarRating = Convert.ToInt32(fields[4]);
            this.HasPool = Convert.ToBoolean(fields[5]);
            this.HasSpa = Convert.ToBoolean(fields[6]);
            this.DisabilityApproved = Convert.ToBoolean(fields[7]);
            this.HasWifi = Convert.ToBoolean(fields[8]);
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
        public static List<ResidenceModel> getAllItems(String username)
        {
            List<ResidenceModel> allItems = ResidenceModel.getAllItems();
            for (int i = allItems.Count - 1; i >= 0; i--)
            {
                if (!allItems[i].Username.Equals(username))
                {
                    allItems.RemoveAt(i);
                }
            }
            return allItems;
        }
        public Boolean save()
        {
            bool writeResult = FileObjectSerializer.AppendToFile(path, this.ToString());
            return writeResult;
        }

        public static String getResidenceName(int id)
        {
            foreach (ResidenceModel rm in ResidenceModel.getAllItems())
            {
                if (rm.ID == id)
                {
                    return rm.Name;
                }
            }
            return null;
        }
        public static ResidenceModel GetByID(int residence_id)
        {
            foreach (ResidenceModel model in ResidenceModel.getAllItems())
            {
                if (model.ID == residence_id)
                {
                    return model;
                }
            }
            return null;
        }
        public Boolean delete()
        {
            //get residence items
            List<ResidenceItemModel> residenceItems = ResidenceItemModel.getAllItems(this.ID);
            //get arrangement items
            List<ArrangementModel> arrangements = ArrangementModel.getAllItems(this.ID);
            //get arrangement reservations
            List<ReservationModel> reservations = ReservationModel.getAllItems(this.ID);
            if (reservations.Count == 0)
            {

                if (FileObjectSerializer.Delete(path, this.ToString()))
                {
                    System.Diagnostics.Debug.Write("found");
                    foreach (ResidenceItemModel residence in residenceItems)
                    {
                        FileObjectSerializer.Delete(residence.Path, residence.ToString());
                    }
                    foreach (ReservationModel reservation in reservations)
                    {
                        FileObjectSerializer.Delete(reservation.Path, reservation.ToString());
                    }
                    foreach (ArrangementModel arrangement in arrangements)
                    {
                        FileObjectSerializer.Delete(arrangement.Path, arrangement.ToString());
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public Boolean update() {
            return FileObjectSerializer.UpdateLine(path, ResidenceModel.GetByID(this.ID).ToString(), this.ToString());
        }
        public Boolean canDelete() {
            List<ReservationModel> allReservations = ReservationModel.getAllItems();
            foreach (ReservationModel rm in allReservations) {
                ResidenceItemModel ri = ResidenceItemModel.GetByID(rm.residence_item_id);
                ArrangementModel ai = ArrangementModel.GetByID(rm.arrangement_id);
                if (ri.ResidenceID == this.ID && ai.DateEnd.CompareTo(DateTime.Now) == -1) {
                    return false;
                }
            }
            return true;
        }
        public int GetUnitCount()
        {
            return ResidenceItemModel.getAllItems(this.ID).Count;
        }
        public int GetFreeUnitCount()
        {
            int totalCount = this.GetUnitCount();
            foreach (ReservationModel res in ReservationModel.getAllItems()) {
                ResidenceItemModel ri = ResidenceItemModel.GetByID(res.residence_item_id);
                ArrangementModel ai = ArrangementModel.GetByID(res.arrangement_id);
                if (res.status == 0 && ai.DateEnd.CompareTo(DateTime.Now) == 1) {
                    totalCount--;
                }
            }
            return totalCount;
        }
    }
}
