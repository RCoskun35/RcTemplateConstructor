using Application.Abstractions;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;

namespace GrpcServer.Services
{
    public class AuthenticationService:Authentication.AuthenticationBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public override async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, ServerCallContext context)
        {
            var result = await _authenticationService.CreateTokenAsync(new Application.DTOs.DTO_Login { Email = request.Email,Password= request.Password,DeviceId="",FirebaseToken="" });
            if (result.IsSuccessful)
                return new AuthenticationResponse { AccessToken = result.Data.AccessToken, ExpiresIn = (int)result.Data.AccessTokenExpiration.Subtract(DateTime.Now).TotalSeconds, ErrorMessage="",IsSuccessful=true };
            
            return new AuthenticationResponse { AccessToken = "", ExpiresIn = default, ErrorMessage = string.Join(",",result.Error.Errors),IsSuccessful=false };
            
        }
    }
}
