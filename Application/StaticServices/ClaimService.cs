using Application.DAO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.StaticServices
{
    public class ClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get { return Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value); }

        }


        public string UserName
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value; }

        }

        public string Email
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value; }

        }
        public string Name
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Name")?.Value; }

        }
        public string Surname
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Surname")?.Value; }

        }
        public string Image
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Image")?.Value; }

        }


        public Dictionary<string, bool> Roles
        {
            get { return GetRoles(); }

        }

        public static Dictionary<string, object> AllClaims
        {
            get { return GetAllClaims(); }

        }
        public static ClaimService CurrentUser
        {
            get
            {
                var httpContextAccessor = new HttpContextAccessor();
                return new ClaimService(httpContextAccessor);
            }
        }

        public static Dictionary<string, object> GetAllClaims()
        {

            return new Dictionary<string, object>()
            {
                { "UserId", CurrentUser.UserId},
                { "UserName", CurrentUser.UserName},
                { "Email", CurrentUser.Email},
                { "Name", CurrentUser.Name},
                { "Surname", CurrentUser.Surname},
                { "Image", CurrentUser.Image},
                { "Roles", CurrentUser.Roles}
            };

        }
        public Dictionary<string, bool> GetRoles()
        {
            var list = Permissions.AllModuleList();
            var resultList = list.ToDictionary(x => x.AccessName, x => true);
            var customDictionary = new CustomDictionary<string, bool>();

            foreach (var kvp in resultList)
            {
                customDictionary[kvp.Key] = kvp.Value;
            }

            return customDictionary;
        }

    }
    public class CustomDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                    return value;

                return default(TValue);
            }
            set => base[key] = value;
        }
    }
}
