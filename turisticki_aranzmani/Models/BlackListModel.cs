using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Models
{
    public class BlackListModel
    {
        private String path = HttpContext.Current.Server.MapPath("~/App_Data/blacklist.csv");
        private static String spath = HttpContext.Current.Server.MapPath("~/App_Data/blacklist.csv");

        public String Username { get; set; }
        public BlackListModel(String username) {
            this.Username = username;
        }
        public BlackListModel(String[] fields) {
            this.Username = fields[0];
        }
        public static List<BlackListModel> getAllBlockedUsers() {
            List<BlackListModel> blockedUsers = new List<BlackListModel>();
            foreach(String row in FileObjectSerializer.GetFileContent(spath)) {
                blockedUsers.Add(new BlackListModel(row.Split(';')));
            }
            return blockedUsers;
        }
        public override string ToString()
        {
            return this.Username + ";" + Environment.NewLine;
        }
        public Boolean save() {
            return FileObjectSerializer.AppendToFile(path, this.ToString());
        }
        public Boolean delete() {
            return FileObjectSerializer.Delete(path, this.ToString());
        }
        public static Boolean isBlocked(UserModel user) {
            foreach (BlackListModel blockedUser in BlackListModel.getAllBlockedUsers()) {
                if (blockedUser.Username.Equals(user.Username)) {
                    return true;
                }
            }
            return false;
        }
        public static List<UserModel> GetBlockedUsers() {
            List<UserModel> users = new List<UserModel>();
            foreach (BlackListModel blItem in BlackListModel.getAllBlockedUsers()) {
                users.Add(UserModel.GetUser(blItem.Username));
            }
            return users;
        }
    }
}