using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Web.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly IIdentityUserRolesService _rolesService; 
        public UserRolesController(IIdentityUserRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public IActionResult Index()
        {
            var model = _rolesService.GetAllUsers();
            return View(model);
        }

        
        public PartialViewResult AddRolesToUser(string id)
        {
            var userRolesVm = _rolesService.GetUserRolesDetails(id);
            return PartialView(userRolesVm);
        }
    }
}
