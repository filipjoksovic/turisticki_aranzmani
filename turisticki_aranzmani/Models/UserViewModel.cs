using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turisticki_aranzmani.Models
{
    public class UserViewModel
    {
        public List<UserModel> Users { get; set; }
        public UserViewModel() {
            this.Users = UserModel.GetUsers();
        }
        public UserViewModel(List<UserModel> users) {
            this.Users = users;
        }
    }
}