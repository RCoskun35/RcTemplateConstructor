using API.Extensions;
using Application.Abstractions;
using Application.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Permission.ApiPermission;
using Persistence.Repositories;
using Persistence.Services;
using Lang = Application.StaticServices.LanguageService;
using Log = Application.StaticServices.LogService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddXmlSerializerFormatters()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
        opt.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// Add services to the container.
builder.Services.AddApiPersistenceServices();
builder.Services.AddHangfire(config => config.UseSqlServerStorage(Configuration.ConnectionString));
builder.Services.AddHangfireServer();
builder.Services.AddSingleton(x => JobStorage.Current.GetMonitoringApi());




Log.Init();
Lang.GetLanguage("tr");

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed((origin) => true)
        .AllowCredentials();
    });
});


builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProviderForApi>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandlerForApi>();

builder.Services.AddSignalR();


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RcTemplate", Version = "v1.0.0.0" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             Array.Empty<string>()
         }
     });
});

var app = builder.Build();
app.UseCors("CorsPolicy");
app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseCustomException();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseHangfireDashboard("/hangfireApi", new DashboardOptions
{
    AppPath = "/",
    DisplayStorageConnectionString = false,
    DashboardTitle = "Rc Template Background Servis"
});


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //endpoints.MapHub<ExampleHub>("/exampleHub");

});



app.Run();
