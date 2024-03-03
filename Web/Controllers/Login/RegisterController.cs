using Application.ViewModels.Auth;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using static Application.StaticServices.LogService;
using Lang = Application.StaticServices.LanguageService;
using Uc = Application.StaticServices.ClaimService;

namespace Web.Controllers.Login
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public RegisterController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult RegisterIndex()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(VM_Register_User newUser)
        {
            User user = null;
            try
            {
                user = await _userManager.FindByEmailAsync(newUser.Email);
            }
            catch (Exception ex)
            {

                throw;
            }
            if (newUser.Password != newUser.PasswordVerify)
            {
                ModelState.AddModelError(string.Empty, Lang.lang["sifrelerUyusmuyor"]);
                return View(newUser);
            }

            if (user != null)
            {
                ModelState.AddModelError(string.Empty, Lang.lang["buKullaniciZatenVar"]);
                return View(newUser);
            }
            var recordUser = new User { Name = newUser.Email, Surname = newUser.Email, Email = newUser.Email, UserName = newUser.Email };
            await _userManager.CreateAsync(recordUser, newUser.Password);

            var claims = new List<Claim>
            {
                new Claim("UserId", recordUser.Id.ToString()),
                new Claim("UserName", recordUser.Name),
                new Claim("Email", recordUser.Email),
                new Claim("Name", recordUser.Name),
                new Claim("Surname", recordUser.Surname),
                new Claim("FullName",$"{recordUser.Name} {recordUser.Surname}"),
            };
            var roles = _roleManager.Roles.ToList();
            foreach (var role in roles)
            {

                foreach (var permission in await _roleManager.GetClaimsAsync(role))
                {
                    if (!claims.Any(x => x.Value == permission.Value))
                    {
                        claims.Add(permission);
                    }

                }
            }
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };

            await _signInManager.SignInWithClaimsAsync(recordUser, authProperties, claims);
            IsLogged($"Kullanıcı Kaydedildi: {JsonConvert.SerializeObject(recordUser)}", LogType.Success);
            return RedirectToAction("Index", "Home");

        }
    }
}
