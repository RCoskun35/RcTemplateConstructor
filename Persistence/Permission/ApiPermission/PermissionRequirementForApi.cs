using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Permission.ApiPermission
{
    public class PermissionRequirementForApi : IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public PermissionRequirementForApi(string permission)
        {
            Permission = permission;

        }
    }
}
