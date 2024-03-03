using Application.Abstractions;
using Application.DTOs;
using Application.Repositories;
using Application.Shared;
using Application.StaticServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static API.Extensions.CustomExceptionHandler;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly HashService _hashService;
        public AuthController(IAuthenticationService authenticationService, UserManager<User> userManager, HashService hashService)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
            _hashService = hashService;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateToken([FromForm] string username, [FromForm] string password, [FromForm] string? firebaseToken, [FromForm] string? deviceId)
        {

            DTO_Login loginDto = new DTO_Login { Email = username, Password = password, FirebaseToken = firebaseToken, DeviceId = deviceId };
            var result = await _authenticationService.CreateTokenAsync(loginDto);
            if (result.StatusCode == 400)
            {
                return ActionResultInstance(result);
            }
            return ActionResultInstance(result);

        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserSetPassword([FromForm] string username, [FromForm] string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var temporaryPassword = "temporary_password_123";
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, temporaryPassword);
            var result = await _userManager.ChangePasswordAsync(user, temporaryPassword, password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }



        /*
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(DTO_Change_Password change_Password)
        {
            var email = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return ActionResultInstance(Response<DTO_Token>.Fail("İşlem Hatalı", 400, true));
            }
            var result = await _userManager.ChangePasswordAsync(user, change_Password.OldPassword, change_Password.NewPassword);
            if (!result.Succeeded)
            {
                return ActionResultInstance(Response<DTO_Token>.Fail("İşlem Hatalı", 400, true));
            }
            return ActionResultInstance(Response<NoDataDto>.Success(200));

        }
        */
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(DTO_Login loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return ActionResultInstance(Response<DTO_Token>.Fail("Kullanıcı Bulunamadı", 400, true));
                }

                //if (!string.IsNullOrEmpty(user.PasswordHash))
                //{
                //    return ActionResultInstance(Response<DTO_Token>.Fail("Bu kullanıcı zaten kayıtlı", 400, true));

                //}
                var temporaryPassword = "temporary_password_123";
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, temporaryPassword);
                var result = await _userManager.ChangePasswordAsync(user, temporaryPassword, loginDto.Password);
                if (!result.Succeeded)
                {
                    return ActionResultInstance(Response<DTO_Token>.Fail("işlem Hatalı", 400, true));
                }
                var resultToken = await _authenticationService.CreateTokenAsync(loginDto);

                return ActionResultInstance(resultToken);


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
