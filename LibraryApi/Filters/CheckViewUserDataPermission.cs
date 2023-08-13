using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LibraryMVC.Domain.Model;

namespace LibraryApi.Filters
{
    public class CheckViewUserDataPermission : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsInRole("Bibliotekarz") || context.HttpContext.User.IsInRole("Administrator"))
            {
                return;
            }

            var libraryUserService = context.HttpContext.RequestServices.GetRequiredService<ILibraryUserService>();
            var identityUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = context.HttpContext.Request.RouteValues["id"]?.ToString();
            
            if (identityUserId == null || userId == null)
            {
                context.Result = new BadRequestResult();
            }

            bool isAuthorized = CheckUserPermission(identityUserId, userId, libraryUserService);

            if (!isAuthorized)
            {
                context.Result = new ForbidResult(); ;
            }
        }

        private bool CheckUserPermission(string identityUserId, string userId, ILibraryUserService libraryUserService)
        {
            var currentUserId = libraryUserService.GetLibraryUserIdByIdentityUserId(identityUserId);
            return currentUserId == Int32.Parse(userId);
        }
    }
}
