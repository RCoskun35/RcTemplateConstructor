using Application.Abstractions;
using Application.Configurations;
using Application.DTOs;
using Application.StaticServices;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Application.DAO.Permissions;

namespace Persistence.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        HashService hash;
        public TokenService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            var assembly = Assembly.GetEntryAssembly();
            hash = new HashService(assembly);
        }
        private string CreateRefreshToken()

        {
            var numberByte = new Byte[32];

            using var rnd = RandomNumberGenerator.Create();

            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }
        private async Task<IEnumerable<Claim>> GetClaims(User user, List<String> audiences)
        {
            var userList = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim("UserId",user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim("Name",user.Name),
            new Claim("Surname",user.Surname),
            new Claim("FullName",user.Name + " " + user.Surname),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                var claims = await _roleManager.GetClaimsAsync(role);
                userList.AddRange(claims.Select(x => new Claim("Permission", x.Value)));
                // userList.AddRange(claims.Select(x=> new Claim("Permission",GetAccessListValue(x.Value).ToString())));
            }

            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return userList.Distinct();
        }
        public int GetAccessListValue(string accessName)
        {
            AccessList accessList;
            bool isValidAccessList = Enum.TryParse(accessName, out accessList);

            if (isValidAccessList)
            {
                return (int)accessList;
            }
            return 0;
        }
        public DTO_Token CreateToken(User user)
        {

            var AccessTokenExpiration = 1400;
            var RefreshTokenExpiration = 1400;
            var Audience = "rcTemplate";
            var Issuer = "rcTemplate";
            var SecurityKey = "mySecurityKey123*!?";

            var accessTokenExpiration = DateTime.Now.AddMinutes(AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: Issuer,
                expires: accessTokenExpiration,
                 notBefore: DateTime.Now,
                 claims: GetClaims(user, new List<string> { Audience }).Result,
                 signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new DTO_Token
            {
                AccessToken = token,
                RefreshToken = token,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }

    }
}
