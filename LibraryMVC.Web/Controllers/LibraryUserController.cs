using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.User;
using LibraryMVC.Domain.Model;
using LibraryMVC.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class LibraryUserController : Controller
    {
        private readonly ILibraryUserService _userService;
        private readonly IValidator<NewLibraryUserVm> _validator;
        public LibraryUserController(ILibraryUserService userService, IValidator<NewLibraryUserVm> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult Index()
        {
            var model = _userService.GetAllLibraryUsersForList();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddNewUser() 
        {
            return View(new NewLibraryUserVm());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddNewUser(NewLibraryUserVm model)
        {
            var result = _validator.Validate(model);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(model);
            }

            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var identityUserEmail = User.FindFirstValue(ClaimTypes.Email);
            model.IdentityUserId = identityUserId;
            model.Email = identityUserEmail;
            _userService.AddUser(model);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("EditUserData/{libraryUserId}")]
        [CheckViewUserDataPermission]
        public IActionResult EditUserData(int libraryUserId)
        {
            var model = _userService.GetInfoForUserEdit(libraryUserId);
            if (model == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult EditUserData(NewLibraryUserVm user)
        {
            if (!ModelState.IsValid)
            {
                var model = _userService.GetInfoForUserEdit(user.Id);
                return View(model);
            }

            _userService.UpdateUser(user);
            TempData["success"] = "Dane zaktualizowane.";
            return RedirectToAction("ViewUserDetails");
        }

        [HttpGet("ViewUserDetails/{libraryUserId}")]
        [CheckViewUserDataPermission]
        public IActionResult ViewUserDetails(int libraryUserId)
        {
            var model = _userService.GetLibraryUserForDetails(libraryUserId);
            if (model == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewLoggedUserDetails()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var libraryUserId = _userService.GetLibraryUserIdByIdentityUserId(identityUserId);
            var model = _userService.GetLibraryUserForPersonalData(libraryUserId);
            if (model == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View("ViewUserPersonalDetails", model);
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult BlockUser(int userId) 
        { 
            _userService.BlockUser(userId);
            TempData["warning"] = $"Użytkownik zablokowany.";
            return RedirectToAction("ViewUserDetails", new { libraryUserId = userId });
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult UnblockUser(int userId)
        {
            _userService.UnblockUser(userId);
            TempData["warning"] = $"Użytkownik odblokowany.";
            return RedirectToAction("ViewUserDetails", new { libraryUserId = userId });
        }
    }
}
