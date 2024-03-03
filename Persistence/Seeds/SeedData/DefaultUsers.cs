using Application.DAO;
using Application.Repositories;
using Application.StaticServices;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds.SeedData
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var defaultUser = new User
            {
                Name = "test",
                Surname = "test",
                UserName = "test@test.com",
                Email = "test@test.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123");
                    await userManager.AddToRoleAsync(defaultUser, "Basic");

                }
            }
            await roleManager.SeedClaimsForBasicUser();
        }

        public static async Task SeedSuperAdminAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {

            var defaultUser = new User
            {
                Name = "admin",
                Surname = "admin",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };



            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123");
                    await userManager.AddToRoleAsync(defaultUser, "SuperAdmin");
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                    await userManager.AddToRoleAsync(defaultUser, "Basic");
                    await userManager.AddClaimAsync(defaultUser, new Claim("HangfireAccess", "true"));
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }

        private async static Task SeedClaimsForSuperAdmin(this RoleManager<Role> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            var allModules = Permissions.AllModuleList();

            foreach (var module in allModules)
            {
                await roleManager.AddPermissionClaim(adminRole, module);
            }

        }
        private async static Task SeedClaimsForBasicUser(this RoleManager<Role> roleManager)
        {
            var basicRole = await roleManager.FindByNameAsync("Basic");
            var allModules = Permissions.AllModuleList();
            foreach (var module in allModules)
            {
                await roleManager.AddPermissionClaim(basicRole, module);
            }

        }

        public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, Permissions.Module module)
        {

            var allClaims = await roleManager.GetClaimsAsync(role);

            if (!allClaims.Any(a => a.Type == "Permission"))
            {
                var claim = new Claim("Permission", module.AccessName, module.Id.ToString());

                await roleManager.AddClaimAsync(role, claim);
            }
        }

        public static List<Claim> AddPermissionForRoleIds(List<int> moduleList)
        {
            List<Claim> claims = new List<Claim>();
            List<Permissions.Module> modules = new List<Permissions.Module>();
            List<Permissions.Module> resultmodules = new List<Permissions.Module>();
            try
            {

                foreach (var module in moduleList)
                {
                    var getModule = Permissions.AllModuleList().Find(x => x.Id == module);
                    modules.Add(getModule);



                    if (!resultmodules.Any(x => x.AccessName == getModule.ParentModule.AccessName))
                    {
                        var asd = getModule.ParentModule;
                        if (asd.ParentModule != null)
                        {
                            if (!resultmodules.Any(x => x == asd))
                            {
                                resultmodules.Add(asd.ParentModule);
                            }

                        }
                        resultmodules.Add(getModule.ParentModule);

                        resultmodules.Add(getModule);

                    }
                    else
                    {

                        resultmodules.Add(getModule);
                    }

                }


                foreach (var result in resultmodules)
                {
                    var claim = new Claim("Permission", result.AccessName, result.Id.ToString());
                    claims.Add(claim);
                }

            }
            catch (Exception ex)
            {
                ex.IsLogged();
                throw;
            }
            return claims;
        }
    }
}
