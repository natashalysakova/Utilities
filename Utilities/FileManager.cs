using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Utilities.DataModel;

namespace Utilities
{
    public class FileManager
    {
        private const int NumberOfBackupFiles = 20;
        static string backupFolder = "Backup\\";
        //static string filename = "Utility.data.json";

        static ILog Log = LogManager.GetLogger("FileManager");


        private static UtilityDataModel? Load(string filePath)
        {
            var jsonString = File.ReadAllText(filePath, Encoding.UTF8);
            try
            {
                var model = JsonSerializer.Deserialize<UtilityDataModel>(jsonString);
                return model;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
        }

        public static bool Save(UtilityDataModel model, string path)
        {
            var jsonString = JsonSerializer.Serialize(model, new JsonSerializerOptions()); 
            var folder = Path.GetDirectoryName(path);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (File.Exists(path))
            {
                Backup(path, folder);
            }
            File.WriteAllText(path, jsonString, Encoding.UTF8);

            return true;
        }

        internal static UtilityDataModel LoadDefault()
        {
            var path = Properties.Settings.Default.SavePath;
            return Load(path);
        }

        internal static void SaveDefault(UtilityDataModel model)
        {
            var path = Properties.Settings.Default.SavePath;
            Save(model, path);
        }

        private static void Backup(string path, string folder)
        {
            var backupDirectory = Path.Combine(folder, backupFolder);
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }
            var filename = Path.GetFileName(path);
            var backupFileName = Path.Combine(backupDirectory, $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{filename}.backup");
            File.Copy(path, backupFileName, true);

            var backupFiles = Directory.GetFiles(backupDirectory);
            if (backupFiles.Length > NumberOfBackupFiles)
            {
                Array.Sort(backupFiles);
                foreach (var item in backupFiles.Take(backupFiles.Length - NumberOfBackupFiles))
                {
                    File.Delete(item);
                }
            }
        }
    }
}
