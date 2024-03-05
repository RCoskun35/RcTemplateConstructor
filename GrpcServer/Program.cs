using Application.Abstractions;
using Application.Configurations;
using Domain.Entities;
using GrpcServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Contexts;
using Persistence.Identity;
using Persistence.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAuthenticationService, Persistence.Services.AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddIdentity<User, Role>(options =>
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


builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));

var getSecurityKey = "mySecurityKey123*!?";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(getSecurityKey),
        ValidateIssuer=false,
        ValidateAudience=false
    };
});
builder.Services.AddAuthorization();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<GrpcServer.Services.AuthenticationService>();
app.MapGrpcService<GrpcServer.Services.CalculationService>();
app.Run();
