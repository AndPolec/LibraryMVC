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

        [HttpGet]
        public async Task<IActionResult> AddRolesToUser(string id)
        {
            var userRolesVm = await _rolesService.GetUserRolesDetailsAsync(id);
            return PartialView("_AddRolesToUser",userRolesVm);
        }

        [HttpPost]
        public async Task<IActionResult> AddRolesToUser(string id, List<string> newRoles)
        {
            if (newRoles == null || !newRoles.Any())
            {
                return BadRequest("Lista nowych ról nie może być pusta.");
            }

            await _rolesService.ChangeUserRolesAsync(id,newRoles);
            TempData["success"] = "Zaktualizowano uprawnienia.";
            return Ok();
        }
    }
}
