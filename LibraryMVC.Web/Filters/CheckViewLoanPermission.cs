using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMVC.Web.Filters
{
    public class CheckViewLoanPermission : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsInRole("Bibliotekarz") || context.HttpContext.User.IsInRole("Administrator"))
            {
                return;
            }

            var loanService = context.HttpContext.RequestServices.GetRequiredService<ILoanService>();
            var userService = context.HttpContext.RequestServices.GetRequiredService<ILibraryUserService>();
            var identityUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var loanId = context.HttpContext.Request.RouteValues["loanId"].ToString();

            bool isAuthorized = CheckUserPermission(identityUserId, loanId, loanService, userService);

            if (!isAuthorized)
            {
                context.Result = new ForbidResult(); ;
            }
        }

        private bool CheckUserPermission(string identityUserId, string loanId, ILoanService loanService, ILibraryUserService userService)
        {
            int libraryUserId = userService.GetLibraryUserIdByIdentityUserId(identityUserId);
            var loan = loanService.GetLoanForDetails(Int32.Parse(loanId));
            return libraryUserId == loan.LibraryUserId;
        }
    }
}