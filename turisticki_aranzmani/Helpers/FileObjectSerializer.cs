using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace turisticki_aranzmani.Helpers
{
    public class FileObjectSerializer
    {
        public static List<String> GetFileContent(String path)
        {
            List<String> objects = File.ReadAllLines(path).ToList();
            return objects;
        }
        public static int GetInsertID(String path)
        {
            int min_id = 1;
            foreach (String line in FileObjectSerializer.GetFileContent(path))
            {
                String[] dataRow = line.Split(';');
                System.Diagnostics.Debug.WriteLine(Convert.ToInt32(dataRow[0]));

                if (Convert.ToInt32(dataRow[0]) >= min_id)
                {
                    min_id = Convert.ToInt32(dataRow[0]) + 1;
                }
            }
            System.Diagnostics.Debug.WriteLine("Returning: " + min_id);
            return min_id;
        }
        public static Boolean WriteToFile(String path, String dataRow)
        {
            List<String> fileContent = FileObjectSerializer.GetFileContent(path);
            fileContent.Add(dataRow);
            try
            {
                File.WriteAllLines(path, fileContent);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static Boolean AppendToFile(String path, String dataRow)
        {
            List<String> fileContent = FileObjectSerializer.GetFileContent(path);
            fileContent.Add(dataRow);
            try
            {
                File.AppendAllText(path, dataRow);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}