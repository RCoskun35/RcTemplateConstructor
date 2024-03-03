using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.StaticServices
{
    public class GeneralService
    {
        public static string GetAssemblyName()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            string version = assembly.GetName().Version.ToString();

            return version;
        }
        public static async Task<List<string>> ReadJsonGetAsList(string path, string key)
        {
            try
            {
                var json = await File.ReadAllTextAsync(path);
                var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

                var keyList = new List<string>();
                foreach (var item in data)
                {
                    keyList.Add(item[key].ToString());
                }

                return new List<string>(keyList);
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
    }
}
