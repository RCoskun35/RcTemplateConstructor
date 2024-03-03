using Application.StaticServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DAO
{
    public static class Permissions
    {
        public enum Type
        {
            None = 0,
            All = 1,
            View = 2,
            Create = 3,
            Edit = 4,
            Delete = 5
        }



        public enum AccessList
        {
            Main = 0,
            User = 1,
            Role = 2,
            Organization = 3
        }
        public class Module
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string AccessName { get; set; }
            public Module? ParentModule { get; set; }
        }
        public static List<Module> AllModuleList()
        {
            List<Module> liste = new List<Module>();
            var module = new Module { AccessName = "Main", Id = 1, Name = "Main", ParentModule = null };
            liste.Add(module);
            liste.Add(new Module { AccessName = "User", Id = 2, Name = "User", ParentModule = module });
            liste.Add(new Module { AccessName = "Role", Id = 3, Name = "Role", ParentModule = module });
            liste.Add(new Module { AccessName = "Organization", Id = 4, Name = "Organization", ParentModule = module });
            return liste;


        }
    }
}
