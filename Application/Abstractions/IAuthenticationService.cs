using Application.DTOs;
using Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IAuthenticationService
    {
        Task<Response<DTO_Token>> CreateTokenAsync(DTO_Login loginDto);
        Task<Response<DTO_Token>> CreateTokenRefreshToken(string refreshToken);
        Task<Response<bool>> RevokeRefreshToken(string refreshToken);
    }
}
