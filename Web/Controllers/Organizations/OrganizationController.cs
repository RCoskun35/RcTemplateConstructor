using Application.DAO;
using Microsoft.AspNetCore.Mvc;
using Persistence.Permission;

namespace Web.Controllers.Organizations
{
    [HasAnyPermission(Permissions.AccessList.Organization)]
    public class OrganizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
