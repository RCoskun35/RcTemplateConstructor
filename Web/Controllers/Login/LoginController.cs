using Application.StaticServices;
using Application.ViewModels.Auth;
using Domain.Entities;
using Hangfire.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using static Application.StaticServices.LogService;
using Lang = Application.StaticServices.LanguageService;
using Uc = Application.StaticServices.ClaimService;

namespace Web.Controllers.Login
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult LoginIndex()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginIndex(VM_Login_User login)
        {
            User user = null;
            try
            {
                user = await _userManager.FindByEmailAsync(login.Email);
            }
            catch (Exception ex)
            {

                throw;
            }

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, Lang.lang["kullaniciBulunamadi"]);
                return View(login);
            }

            if (user.IsDeleted)
            {
                ModelState.AddModelError(string.Empty, Lang.lang["kullaniciSilinmis"]);
                return View(login);
            }
            var passResult = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!passResult)
            {
                ModelState.AddModelError(string.Empty, Lang.lang["emailYadaSifreHatali"]);
                return View(login);
            }

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserName", user.Name),
                new Claim("Email", user.Email),
                new Claim("Name", user.Name),
                new Claim("Surname", user.Surname),
                new Claim("FullName",$"{user.Name} {user.Surname}"),
            };
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };

            await _signInManager.SignInWithClaimsAsync(user, authProperties, claims);

            IsLogged($"Sisteme Giriş Yapıldı => ID: {user.Id}", LogType.Success);
            return RedirectToAction("Index", "Home");
        }


    }
}
