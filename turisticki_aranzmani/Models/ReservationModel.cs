using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Models
{
    public class ReservationModel
    {

        //status values:
        //0:created
        //1:cancelled
        //string line format : username;arrangement_id;residence_item_id;status;created_at

        String path = HttpContext.Current.Server.MapPath("~/App_Data/reservations.csv");
        static String static_path = HttpContext.Current.Server.MapPath("~/App_Data/reservations.csv");
        public String Path { get { return this.path; } }
        public int id { get; set; }
        public String username { get; set; }
        public int arrangement_id { get; set; }
        public int residence_item_id { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }

        public ReservationModel()
        {
            this.id = FileObjectSerializer.GetInsertID(path);
            this.status = 0;
            this.created_at = DateTime.Now;
        }
        public ReservationModel(String[] fields)
        {
            this.id = Convert.ToInt32(fields[0]);
            this.username = fields[1];
            this.arrangement_id = Convert.ToInt32(fields[2]);
            this.residence_item_id = Convert.ToInt32(fields[3]);
            this.status = Convert.ToInt32(fields[4]);
            this.created_at = DateTime.Parse(fields[5]);
        }
        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5}{6}", this.id, this.username, this.arrangement_id, this.residence_item_id, this.status, this.created_at, Environment.NewLine);
        }
        public static ReservationModel GetByID(int id) {
            foreach (ReservationModel rm in ReservationModel.getAllItems()) {
                if (rm.id == id) {
                    return rm;
                }
            }
            return null;
        }
        public static List<ReservationModel> getAllItems()
        {
            List<ReservationModel> allItems = new List<ReservationModel>();
            List<String> fileContent = FileObjectSerializer.GetFileContent(static_path);
            foreach (String row in fileContent)
            {
                ReservationModel instance = new ReservationModel(row.Split(';'));
                allItems.Add(instance);
            }
            return allItems;
        }
        public static List<ReservationModel> getSellerReservations(String username) {
            List<ReservationModel> allItems = ReservationModel.getAllItems();
            for (int i = allItems.Count - 1; i >= 0; i--)
            {
                ArrangementModel am = ArrangementModel.GetByID(allItems[i].arrangement_id);

                if (!am.Username.Equals(username))
                {
                    allItems.RemoveAt(i);
                }
            }
            return allItems;
        }
        public static List<ReservationModel> getAllItems(String username) {
            List<ReservationModel> allItems = ReservationModel.getAllItems();
            for (int i = allItems.Count - 1; i >= 0; i--) {
                if (!allItems[i].username.Equals(username)) {
                    allItems.RemoveAt(i);
                }
            }
            return allItems;
        }
        public static List<ReservationModel> getAllItems(int arrangement_id)
        {
            List<ReservationModel> allItems = ReservationModel.getAllItems();
            for (int i = allItems.Count - 1; i >= 0; i--)
            {
                if (allItems[i].arrangement_id != arrangement_id)
                {
                    allItems.RemoveAt(i);
                }
            }
            return allItems;
        }
        public Boolean save()
        {
            try
            {
                FileObjectSerializer.AppendToFile(path, this.ToString());
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Boolean cancel() {
            String prevVal = this.ToString();
            this.status = 1;
            String upVal = this.ToString();
            return FileObjectSerializer.UpdateLine(this.path, prevVal, upVal);
        }
        public Boolean delete() {
            FileObjectSerializer.Delete(path, this.ToString());
            return true;
        }
        public static Boolean HasUserReservation(String username, int id) {
            foreach (ReservationModel rm in ReservationModel.getAllItems()) {
                if (rm.username.Equals(username) && rm.arrangement_id == id) {
                    return true;
                }
            }
            return false;
        }
        public static Boolean delete(String username) {
            foreach (ReservationModel rm in ReservationModel.getAllItems(username)) {
                FileObjectSerializer.Delete(HttpContext.Current.Server.MapPath("~/App_Data/reservations.csv"), rm.ToString());
            }
            return true;
        }
    }
}