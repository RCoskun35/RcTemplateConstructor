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
    public class ClaimApiService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimApiService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        public int UserId
        {

            get { return Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value); }

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
        public Dictionary<string, bool> Roles
        {
            get { return GetRoles(); }

        }
        public static Dictionary<string, object> AllClaims
        {
            get { return GetAllClaims(); }

        }

        public static ClaimApiService CurrentUser
        {
            get
            {
                var httpContextAccessor = new HttpContextAccessor();
                return new ClaimApiService(httpContextAccessor);
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
                { "Roles", CurrentUser.Roles}
            };

        }
        public Dictionary<string, bool> GetRoles()
        {
            return new Dictionary<string, bool>();
        }
    }
}
