using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class DictionaryExtensions
    {
        public static bool GetValueOrDefault(this Dictionary<string, bool> dictionary, string key)
        {
            if (dictionary.TryGetValue(key, out bool value))
            {
                return value;
            }
            return false;
        }
    }
}
