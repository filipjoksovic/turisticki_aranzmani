using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turisticki_aranzmani.Helpers;

namespace turisticki_aranzmani.Models
{
    public class ArrangementCommentModel
    {
        private string path = HttpContext.Current.Server.MapPath("~/App_Data/arrangement_comments.csv");
        public int ID { get; set; }
        public String Username { get; set; }
        public int ArrangementID { get; set; }
        public int Grade { get; set; }
        public String Comment { get; set; }
        public Boolean Allowed { get; set; }
        public DateTime DateCreated { get; set; }

        public ArrangementCommentModel()
        {
            this.ID = FileObjectSerializer.GetInsertID(this.path);
            this.DateCreated = DateTime.Now;
            this.Allowed = false;
        }
        public ArrangementCommentModel(String[] fields) {
            this.ID = Convert.ToInt32(fields[0]);
            this.Username = fields[1];
            this.ArrangementID = Convert.ToInt32(fields[2]);
            this.Grade = Convert.ToInt32(fields[3]);
            this.Comment = fields[4];
            this.Allowed = Convert.ToBoolean(fields[5]);
            this.DateCreated = DateTime.Parse(fields[6]);
        }
        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5};{6}{7}", this.ID, this.Username, this.ArrangementID, this.Grade, this.Comment, this.Allowed, this.DateCreated, Environment.NewLine);
        }
        public static List<ArrangementCommentModel> GetComments()
        {
            List<ArrangementCommentModel> allComments = new List<ArrangementCommentModel>();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/arrangement_comments.csv");

            foreach(String row in FileObjectSerializer.GetFileContent(path))
            {
                ArrangementCommentModel commentInstance = new ArrangementCommentModel(row.Split(';'));
                allComments.Add(commentInstance);
            }
            return allComments;
        }
        public static ArrangementCommentModel GetByID(int id) {
            foreach (ArrangementCommentModel comment in ArrangementCommentModel.GetComments()) {
                if (comment.ID == id) {
                    return comment;
                }
            }
            return null;
        }
        public Boolean save()
        {
            return FileObjectSerializer.AppendToFile(this.path, this.ToString());
        }
        public Boolean delete() {
            return FileObjectSerializer.Delete(this.path, this.ToString());
        }
        public Boolean update() {
            String oldVal = ArrangementCommentModel.GetByID(this.ID).ToString();
            return FileObjectSerializer.UpdateLine(this.path, oldVal, this.ToString());
        }
        public static ArrangementCommentModel GetComment(int arrangement_id, String username) {
            foreach (ArrangementCommentModel comment in ArrangementCommentModel.GetComments()) {
                System.Diagnostics.Debug.WriteLine(comment.ToString());
                if (comment.ArrangementID == arrangement_id && comment.Username.Equals(username)) {
                    return comment;
                }
            }
            return null;
        }


    }
}