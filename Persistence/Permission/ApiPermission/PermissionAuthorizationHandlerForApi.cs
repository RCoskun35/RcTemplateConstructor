using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ucApi = Application.StaticServices.ClaimApiService;

namespace Persistence.Permission.ApiPermission
{
    public class PermissionAuthorizationHandlerForApi : AuthorizationHandler<PermissionRequirementForApi>
    {
        public PermissionAuthorizationHandlerForApi()
        {

        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirementForApi requirements)
        {
            try
            {
                if (context.User == null)
                {
                    await Task.Run(context.Fail);
                    return;
                }

                var userApiRoles = ucApi.CurrentUser.Roles;

                if (userApiRoles[requirements.Permission])
                {
                    await Task.Run(() => context.Succeed(requirements));
                    return;
                }


                context.Fail();
            }
            catch (Exception)
            {

                context.Fail();
            }
        }
    }
}
