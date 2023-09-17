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
        private readonly ILogger _logger;

        public LibraryUsersController(ILibraryUserService userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("me")]
        public ActionResult<LibraryUserForPersonalDataVm> GetLoggedUserDetails()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (identityUserId is null)
            {
                return BadRequest("Brak Id użytkownika.");
            }

            try
            {
                var libraryUserId = _userService.GetLibraryUserIdByIdentityUserId(identityUserId);
                var model = _userService.GetLibraryUserForPersonalData(libraryUserId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using GetLoggedUserDetails.");
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<List<LibraryUserForListVm>> GetAllLibraryUsers()
        {
            try
            {
                var model = _userService.GetAllLibraryUsersForList();
                return Ok(model);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error while using GetAllLibraryUsers.");
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [CheckViewUserDataPermission]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<LibraryUserDetailsVm> GetLibraryUser(int id)
        {
            try
            {
                var model = _userService.GetLibraryUserForDetails(id);
                if (model is null)
                {
                    return NotFound();
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using GetLibraryUser with id={id}.", id);
                return StatusCode(500);
            }
        }

        [HttpPatch("{userId}/block")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult BlockUser(int userId)
        {
            try
            {
                bool result = _userService.BlockUser(userId);
                if (!result)
                {
                    return NotFound("Użytkowniko podanym id nie został znaleziony.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using BlockUser with userId={userId}.", userId);
                return StatusCode(500);
            }
        }

        [HttpPatch("{userId}/unblock")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult UnblockUser(int userId)
        {
            try
            {
                bool result = _userService.UnblockUser(userId);
                if (!result)
                {
                    return NotFound("Użytkownik o podanym id nie został znaleziony.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using UnblockUser with userId={userId}.", userId);
                return StatusCode(500);
            }
        }
    }
}
