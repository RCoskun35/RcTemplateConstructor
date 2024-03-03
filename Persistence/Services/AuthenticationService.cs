using Application.Abstractions;
using Application.DTOs;
using Application.Shared;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Lang = Application.StaticServices.LanguageService;


namespace Persistence.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;

        private readonly UserManager<User> _userManager;
        public AuthenticationService(ITokenService tokenService, UserManager<User> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }



        public async Task<Response<DTO_Token>> CreateTokenAsync(DTO_Login loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            var user = await _userManager.Users.Where(u => u.Email == loginDto.Email).FirstOrDefaultAsync();

            if (user == null)
                return Response<DTO_Token>.Fail(Lang.lang["emailYadaSifreHatali"], 400, true);

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return Response<DTO_Token>.Success(new DTO_Token { AccessToken = "", AccessTokenExpiration = default, RefreshToken = "", RefreshTokenExpiration = default }, 200);
            }


            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Response<DTO_Token>.Fail(Lang.lang["emailYadaSifreHatali"], 400, true);
            var token = _tokenService.CreateToken(user);
            return Response<DTO_Token>.Success(token, 200);
        }

        public async Task<Response<DTO_Token>> CreateTokenRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
        Task<Response<bool>> IAuthenticationService.RevokeRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
