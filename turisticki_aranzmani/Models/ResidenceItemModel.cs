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
        [DisplayName("Sifra smestaja")]
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
            System.Diagnostics.Debug.WriteLine("Id from function: " + FileObjectSerializer.GetInsertID(path));
            this.fileContent = FileObjectSerializer.GetFileContent(path);
            System.Diagnostics.Debug.WriteLine("ID from constructor: " + this.ID);

        }
        public ResidenceItemModel(String[] fields) {
            this.ID = Convert.ToInt32(fields[0]);
            this.ResidenceID = Convert.ToInt32(fields[1]);
            this.UnitName = fields[2];
            this.MaxGuests = Convert.ToInt32(fields[3]);
            this.AllowPets = Convert.ToBoolean(fields[4]);
            this.Price = Convert.ToInt32(fields[5]);
        }

        internal static ResidenceItemModel GetByID(int id)
        {
            foreach(ResidenceItemModel unit in ResidenceItemModel.getAllItems()) {
                if (unit.ID == id) {
                    return unit;
                }
            }
            return null;
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
            for (int i = allItems.Count - 1; i >= 0; i--)
            {
                if (allItems[i].ResidenceID != id)
                {
                    allItems.RemoveAt(i);
                }
            }
            return allItems;
        }
        public static List<ResidenceItemModel> getAvailableItems(int arrangement_id) {
            ArrangementModel arrangement = ArrangementModel.GetByID(arrangement_id);
            int arrangement_residence = arrangement.ResidenceID;
            List<ResidenceItemModel> allResidenceUnits = ResidenceItemModel.getAllItems(arrangement_residence);
            List<ReservationModel> allReservations = ReservationModel.getAllItems(arrangement_id);
            for (int i = 0; i < allReservations.Count; i++) {
                int unit_id = allReservations[i].residence_item_id;
                System.Diagnostics.Debug.WriteLine("resid: " + unit_id);
                for (int j = allResidenceUnits.Count - 1; j >=0; j--) {
                    System.Diagnostics.Debug.WriteLine("unitid: " + allResidenceUnits[j].ID);

                    if (unit_id == allResidenceUnits[j].ID) {
                        allResidenceUnits.RemoveAt(j);
                    }
                }
            }
            return allResidenceUnits;

        }
        public static List<ResidenceItemModel> getBookedItems(int arrangement_id) {
            ArrangementModel arrangement = ArrangementModel.GetByID(arrangement_id);
            int arrangement_residence = arrangement.ResidenceID;
            List<ResidenceItemModel> allResidenceUnits = ResidenceItemModel.getAllItems(arrangement_residence);
            List<ReservationModel> allReservations = ReservationModel.getAllItems(arrangement_id);
            for (int i = 0; i < allReservations.Count; i++)
            {
                int unit_id = allReservations[i].residence_item_id;
                System.Diagnostics.Debug.WriteLine("resid: " + unit_id);
                for (int j = allResidenceUnits.Count - 1; j >= 0; j--)
                {
                    System.Diagnostics.Debug.WriteLine("unitid: " + allResidenceUnits[j].ID);

                    if (unit_id != allResidenceUnits[j].ID)
                    {
                        allResidenceUnits.RemoveAt(j);
                    }
                }
            }
            return allResidenceUnits;

        }

        public Boolean save()
        {
            this.ID = FileObjectSerializer.GetInsertID(path);

            if (this.exists())
            {
                return false;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ID:");

                System.Diagnostics.Debug.WriteLine(this.ID);

                bool writeRestult = FileObjectSerializer.AppendToFile(path, this.ToString());
                return writeRestult;
            }
        }
    }
}