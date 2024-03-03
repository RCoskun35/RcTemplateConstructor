using Application.StaticServices;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class Configuration
    {
        public static void ConfigureEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                #region Home
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: LanguageService.lang["anasayfa"].ToLower(),
                    defaults: new { controller = "Home", action = "Index" });
                #endregion

                #region userAndRole
                endpoints.MapControllerRoute(
                name: "user",
                pattern: LanguageService.lang["kullanici-islemleri"],
                defaults: new { controller = "Users", action = "UserIndex" });
                endpoints.MapControllerRoute(
                    name: "role",
                    pattern: LanguageService.lang["rol-islemleri"],
                    defaults: new { controller = "Roles", action = "RoleIndex" });
                #endregion
                #region Login
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=LoginIndex}/{id?}");
                #endregion

                #region Organization
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "organizations",
                  defaults: new { controller = "Organization", action = "Index" });
                #endregion
            });
        }
    }
}
