using Application.Configurations;
using Application.Repositories;
using Application.StaticServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Identity;
using Persistence.Permission;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class ServiceRegistration
    {
        public async static void AddPersistenceServices(this IServiceCollection services)
        {
            //var builder = WebApplication.CreateBuilder();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));
            // TODO: Canlıda yorumdan çıkarılacak
            try
            {
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    //dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                ex.IsLogged();
            }

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //TODO: Canlıda yorumdan çıkarılacak



            services.AddScoped(provider =>
            {
                var assembly = Assembly.GetEntryAssembly();
                return new HashService(assembly);
            });


            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

            })
                .AddErrorDescriber<CustomErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
                options.OnRefreshingPrincipal = context =>
                {

                    context.NewPrincipal = context.CurrentPrincipal;
                    return Task.CompletedTask;
                };
            });



        }

        public static void AddApiPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ClaimApiService, ClaimApiService>();
            var serviceProvider = services.BuildServiceProvider();
            var getAudience = "www.rctemplate.com";
            var getIssuer = "www.rctemplate.com";
            var listIssuer = getIssuer.Split(',');
            var getSecurityKey = "asd";
            services.AddScoped(provider =>
            {
                var assembly = Assembly.GetEntryAssembly();
                return new HashService(assembly);
            });
            var assembly = Assembly.GetEntryAssembly();
            HashService hash = new(assembly);

            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

            })
                .AddErrorDescriber<CustomErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
                options.OnRefreshingPrincipal = context =>
                {

                    context.NewPrincipal = context.CurrentPrincipal;
                    return Task.CompletedTask;
                };
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = getIssuer,
                    ValidAudience = listIssuer.FirstOrDefault(),
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(getSecurityKey),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

    }
}
