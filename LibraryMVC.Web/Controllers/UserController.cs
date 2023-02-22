using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly ILibraryUserService _userService;
        public UserController(ILibraryUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddNewUser() 
        {
            return View(new NewLibraryUserVm());
        }

        [HttpPost]
        public IActionResult AddNewUser(NewLibraryUserVm model)
        {
            //To do: validate

            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.IdentityUserId = identityUserId;
            _userService.AddUser(model);

            return RedirectToAction("Index", "Home");
        }
    }
}
