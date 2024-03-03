using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ucApi = Application.StaticServices.ClaimApiService;
using ucWeb = Application.StaticServices.ClaimService;

namespace Persistence.Permission
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirements)
        {
            try
            {
                if (context.User == null)
                {
                    await Task.Run(() => context.Fail());
                    return;
                }

                var userWebRoles = ucWeb.CurrentUser.Roles;
                if (userWebRoles.TryGetValue(requirements.Permission, out bool hasPermission) && hasPermission)
                {
                    await Task.Run(() => context.Succeed(requirements));
                    return;
                }

                var httpContext = _httpContextAccessor.HttpContext;
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Fail();
            }
            catch (Exception ex)
            {
                context.Fail();

            }
        }
    }
}

