using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            services.AddCors(options => options.AddDefaultPolicy(policy =>
           policy
            .SetIsOriginAllowed((origin) => true)
           .AllowAnyHeader()
           .AllowAnyMethod()
           ));

            return services;
        }
    }
}
