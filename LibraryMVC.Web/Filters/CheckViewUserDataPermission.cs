using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMVC.Web.Filters
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
            var identityUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = context.HttpContext.Request.RouteValues["libraryUserId"].ToString();

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
