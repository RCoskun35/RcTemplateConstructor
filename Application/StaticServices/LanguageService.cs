using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.StaticServices
{
    public class LanguageService
    {
        public static string CurrentLanguage = "tr";
        public static Dictionary<string, string> lang = new Dictionary<string, string>();

        public static void GetLanguage(string langCode)
        {
            try
            {
                if (string.IsNullOrEmpty(langCode))
                    langCode = "tr";

                CurrentLanguage = langCode;

                string basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\.."));
                string jsonPath = Path.Combine(basePath, $"Application\\Language\\langs\\{langCode}.json");

                if (!File.Exists(jsonPath))
                {
                    jsonPath = GetLanguageFilePath(langCode);
                }

                StreamReader r = new StreamReader(jsonPath);
                string json = r.ReadToEnd();
                r.Close();

                lang = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<string> GetAllLanguages()
        {
            try
            {
                var fileList = new List<string>();
                var flagsPath = "wwwroot\\Content\\images\\flags";
                string basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\.."));
                string folderPath = Path.Combine(basePath, "Core\\Application\\Language\\langs\\");
                string jsonPath = Path.Combine(basePath, "Core\\Application\\Language\\langs\\tr.json");

                if (!File.Exists(jsonPath))
                {
                    folderPath = GetLanguageFilePath2();
                }

                var serverFlagPath = Directory.GetCurrentDirectory();
                var jsonFiles = Directory.GetFiles(folderPath, "*.JSON");

                foreach (var flagFile in Directory.EnumerateFiles(Path.Combine(serverFlagPath, flagsPath)))
                {
                    var flagFileName = Path.GetFileName(flagFile);
                    var flagName = Path.GetFileNameWithoutExtension(flagFileName);
                    var jsonFileName = flagName + ".json";
                    var jsonFile = jsonFiles.FirstOrDefault(f => f.EndsWith(jsonFileName));

                    if (jsonFile != null)
                    {
                        var flagPath = $"Content/images/flags/{flagFileName}";
                        fileList.Add(flagName);
                    }
                }

                fileList.Remove("tr");
                return fileList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetLanguageFilePath(string langCode)
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            var path = Path.Combine(assemblyDirectory, "Language", "langs", $"{langCode}.json");
            return path;
        }

        private static string GetLanguageFilePath2()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            var path = Path.Combine(assemblyDirectory, "Language", "langs");
            return path;
        }
    }
}
