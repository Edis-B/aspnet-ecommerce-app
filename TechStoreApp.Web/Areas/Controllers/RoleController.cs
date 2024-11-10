using Microsoft.AspNetCore.Identity;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class RoleController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleController(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }
    }
}
