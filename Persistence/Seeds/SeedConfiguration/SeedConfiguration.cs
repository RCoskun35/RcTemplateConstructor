using Application.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.CreateDbObjects;
using Persistence.Seeds.SeedData;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds.SeedConfiguration
{
    public static class SeedConfiguration
    {
        public static async Task Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {

                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<Role>>();
                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    await DefaultUsers.SeedBasicUserAsync(userManager, roleManager);
                    await DefaultUsers.SeedSuperAdminAsync(userManager, roleManager);

                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    CreateFunction.Create(dbContext);
                    CreateProcedure.Create(dbContext);

                }
                catch (Exception ex)
                {

                }
            }


        }


    }
}
