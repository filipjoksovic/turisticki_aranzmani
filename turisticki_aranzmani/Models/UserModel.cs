using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Models
{
    [Bind]
    public class UserModel
    {
        String username;
        String password;
        String first_name;
        String last_name;
        String gender;
        String email;
        DateTime birthday;
        String role;

        private string table = HttpContext.Current.Server.MapPath("~/App_Data/users.csv");
        private static string stable = HttpContext.Current.Server.MapPath("~/App_Data/users.csv");
        public UserModel()
        {

        }
        public UserModel(Models.UserModel model)
        {
            this.Username = model.Username;
            this.Password = model.Password;
            this.FirstName = model.FirstName;
            this.LastName = model.LastName;
            this.Gender = model.Gender;
            this.Email = model.Email; ;
            this.Birthday = model.Birthday;
            this.Role = "guest";
        }
        public UserModel(String[] data)
        {
            this.Username = data[0];
            this.Password = data[1];
            this.FirstName = data[2];
            this.LastName = data[3];
            this.Gender = data[4];
            this.Email = data[5];
            this.Birthday = DateTime.Parse(data[6]);
            this.Role = data[7];
        }
        public UserModel(String username, String password, String first_name, String last_name, String gender, String email, DateTime birthday, String role)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = first_name;
            this.LastName = last_name;
            this.Gender = gender;
            this.Email = email;
            this.Birthday = birthday;
            this.Role = role;
        }
        [Required(ErrorMessage = "Korisnicko ime je obavezno polje")]
        public String Username { get; set; }
        [Required(ErrorMessage = "Lozinka je obavezno bolje")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Required(ErrorMessage = "Ime je obavezno bolje")]
        public String FirstName { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno polje")]
        public String LastName { get; set; }
        public String Gender { get; set; }
        [Required(ErrorMessage = "Email adresa je obavezno bolje")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email adresa nije u odgovarajucem formatu")]
        public String Email { get; set; }
        [Required(ErrorMessage = "Datum rodjenja je obavezno polje")]
        [DataType(DataType.Date, ErrorMessage = "Datum rodjenja nije u odgovarajucem formatu")]
        public DateTime Birthday { get; set; }
        public String Role { get; set; }
        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5};{6};{7}{8}", this.Username, this.Password, this.FirstName, this.LastName, this.Gender, this.Email, this.Birthday, this.Role, Environment.NewLine); ;
        }
        public Boolean exists()
        {
            List<String> fileContent = System.IO.File.ReadAllLines(table).ToList();
            foreach (String row in fileContent)
            {
                UserModel newUserInstance = new UserModel(row.Split(';'));
                System.Diagnostics.Debug.WriteLine("Hello");
                System.Diagnostics.Debug.WriteLine(newUserInstance.Username);
                if (this.Username == newUserInstance.Username || this.Email == newUserInstance.Email)
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
                try
                {
                    System.IO.File.AppendAllText(this.table, this.ToString());
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public Boolean update()
        {
            List<String> fileContent = System.IO.File.ReadAllLines(table).ToList();
            bool found = false;
            for (int i = 0; i < fileContent.Count; i++)
            {
                UserModel newUserInstance = new UserModel(fileContent[i].Split(';'));
                if (this.Username.Equals(newUserInstance.Username))
                {
                    fileContent[i] = this.ToString();
                    found = true;
                    break;
                }
            }
            if (found)
            {
                System.IO.File.WriteAllLines(table, fileContent);
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean delete()
        {
            System.Diagnostics.Debug.WriteLine(this.ToString());
            if(this.role == "seller")
            {
                List<ResidenceModel> residences = ResidenceModel.getAllItems(this.username);
                foreach (ResidenceModel residence in residences) {
                    residence.delete();
                }
            }
            List<ReservationModel> reservations = ReservationModel.getAllItems(this.Username);
            foreach (ReservationModel reservation in reservations) {
                reservation.delete();
            }
            //add comment delete method
            FileObjectSerializer.Delete(table, this.ToString());
            return true;
        }
        public static UserModel find(String username)
        {
            List<String> fileContent = System.IO.File.ReadAllLines(stable).ToList();
            foreach (String row in fileContent)
            {
                UserModel userInstance = new UserModel(row.Split(';'));
                if (userInstance.Username.Equals(username))
                {
                    return userInstance;
                }
            }
            return null;
        }
        public static UserModel find(UserModel model)
        {
            List<String> fileContent = System.IO.File.ReadAllLines(stable).ToList();
            for (int i = 0; i < fileContent.Count; i++)
            {
                UserModel newUserInstance = new UserModel(fileContent[i].Split(';'));
                if (model.Username.Equals(newUserInstance.Username) && model.Password.Equals(newUserInstance.Password))
                {
                    return newUserInstance;
                }
            }
            return null;
        }
        public static List<UserModel> GetUsers() {
            List<String> fileContent = FileObjectSerializer.GetFileContent(stable);
            List<UserModel> allUsers = new List<UserModel>();
            foreach (String dataRow in fileContent) {
                UserModel userInstance = new UserModel(dataRow.Split(';'));
                allUsers.Add(userInstance);
            }
            return allUsers;
        }
        public static UserModel GetUser(String username) {
            List<UserModel> allUsers = UserModel.GetUsers();
            foreach (UserModel user in allUsers) {
                if (user.Username.Equals(username)) {
                    return user;
                }
            }
            return null;
        }

    }
}