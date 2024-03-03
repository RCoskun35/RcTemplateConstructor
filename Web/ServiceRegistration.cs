using Application.Validators.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
            {
                opts.LoginPath = "/Login/LoginIndex";
                opts.AccessDeniedPath = "/Login/LoginIndex";
                opts.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                opts.SlidingExpiration = true;


            });

            services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>());



            return services;
        }
    }
}
