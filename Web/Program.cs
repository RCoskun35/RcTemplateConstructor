using Application;
using Application.StaticServices;
using Hangfire;
using Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Seeds.SeedConfiguration;
using Persistence.Seeds.SeedData;
using Persistence.Services;
using Web;
using Web.Hangfire;
using Lang = Application.StaticServices.LanguageService;

var builder = WebApplication.CreateBuilder(args);
LogService.Init();
Lang.GetLanguage("tr");
builder.Services.AddWebServices();
builder.Services.AddPersistenceServices();
// Add services to the container.
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ClaimService, ClaimService>();
var conn = Persistence.Configuration.ConnectionString;
builder.Services.AddHangfire(config => config.UseSqlServerStorage(Persistence.Configuration.ConnectionString));
builder.Services.AddHangfireServer();
builder.Services.AddSingleton(x => JobStorage.Current.GetMonitoringApi());

builder.Services.Configure<MvcOptions>(options => { options.MaxModelBindingCollectionSize = 209715200; });
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue;
    options.KeyLengthLimit = int.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureServices();


var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    AppPath = "/anasayfa",
    DisplayStorageConnectionString = false,
    DashboardTitle = "rctemplate Servisi",
    Authorization = new[] { new HangfireAuthorizationFilter(app.Services.GetRequiredService<IHttpContextAccessor>()) }

});

app.MapControllers();

app.ConfigureEndpoints();
// TODO: Canlýda yorumdan çýkarýlacak
await app.Seed();

app.Run();
