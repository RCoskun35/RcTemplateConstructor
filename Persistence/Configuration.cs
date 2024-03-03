using Application.StaticServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class Configuration
    {
        static public string ConnectionString
        {
            get
            {
                //if (System.Diagnostics.Debugger.IsAttached == false)
                //{
                //    System.Diagnostics.Debugger.Launch();
                //}

                string currentDirectory = Directory.GetCurrentDirectory();

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(currentDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                string encryptedConnectionString = configuration.GetConnectionString("MsSql");
                HashService hashService = new HashService(System.Reflection.Assembly.GetExecutingAssembly());
                string decryptedConnectionString = hashService.Decrypt(encryptedConnectionString);

                return decryptedConnectionString;
            }
        }
    }
}
