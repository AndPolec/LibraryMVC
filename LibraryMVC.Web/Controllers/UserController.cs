using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
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
            return View(new NewUserVm());
        }

        [HttpPost]
        public IActionResult AddNewUser(NewUserVm model)
        {
            //To do: validate

            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.IdentityUserId = identityUserId;
            _userService.AddUser(model);

            return RedirectToAction("Index", "Home");
        }
    }
}
