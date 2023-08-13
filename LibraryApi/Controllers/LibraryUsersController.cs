using FluentValidation;
using LibraryApi.Filters;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.LibraryUser;
using LibraryMVC.Application.ViewModels.User;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LibraryUsersController : ControllerBase
    {
        private readonly ILibraryUserService _userService;
        public LibraryUsersController(ILibraryUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public ActionResult<LibraryUserForPersonalDataVm> GetLoggedUserDetails()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (identityUserId is null)
            {
                return BadRequest("Brak Id użytkownika.");
            }
            var libraryUserId = _userService.GetLibraryUserIdByIdentityUserId(identityUserId);
            var model = _userService.GetLibraryUserForPersonalData(libraryUserId);
            return Ok(model);
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public ActionResult<List<LibraryUserForListVm>> GetAllLibraryUsers()
        {
            var model = _userService.GetAllLibraryUsersForList();
            return Ok(model);
        }

        [HttpGet("{id}")]
        [CheckViewUserDataPermission]
        public ActionResult<LibraryUserDetailsVm> GetLibraryUser(int id)
        {
            var model = _userService.GetLibraryUserForDetails(id);
            if(model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPatch("{userId}/block")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult BlockUser(int userId)
        {
            bool result = _userService.BlockUser(userId);
            if (!result)
            {
                return NotFound("Użytkowniko podanym id nie został znaleziony.");
            }
            return Ok();
        }

        [HttpPatch("{userId}/unblock")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult UnblockUser(int userId)
        {
            bool result = _userService.UnblockUser(userId);
            if (!result)
            {
                return NotFound("Użytkowniko podanym id nie został znaleziony.");
            }
            return Ok();
        }
    }
}
