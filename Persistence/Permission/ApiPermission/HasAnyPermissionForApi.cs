﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.DAO.Permissions;

namespace Persistence.Permission.ApiPermission
{
    public class HasAnyPermissionForApi : AuthorizeAttribute
    {
        public HasAnyPermissionForApi(params AccessList[] permissions)
        {
            var policy = string.Join(",", permissions.Select(p => p.ToString()));
            Policy = policy;
        }
    }
}
