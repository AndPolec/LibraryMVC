using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace LibraryMVC.Web.Filters
{
    public class CheckCreateLoanPermission : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var loanService = context.HttpContext.RequestServices.GetRequiredService<ILoanService>();
            var userService = context.HttpContext.RequestServices.GetRequiredService<ILibraryUserService>();
            var identityUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var borrowingCartId = context.HttpContext.Request.Form["borrowingCartId"].ToString(); ;
            var libraryUserId = context.HttpContext.Request.Form["userId"].ToString();

            bool isAuthorized = CheckUserPermission(identityUserId, libraryUserId,borrowingCartId,loanService,userService);

            if (!isAuthorized)
            {
                context.Result = new ForbidResult(); ;
            }
        }

        private bool CheckUserPermission(string identityUserId, string libraryUserId, string borrowingCartId, ILoanService loanService, ILibraryUserService userService)
        {
            int intBorrowingCartId = Int32.Parse(borrowingCartId);
            int intlibraryUserId = Int32.Parse(libraryUserId);
            int checkedUserId = userService.GetLibraryUserIdByIdentityUserId(identityUserId);
            int checkedUserBorrowingCartId = loanService.GetBorrowingCartForDetailsByIndentityUserId(identityUserId).Id;

            if (intBorrowingCartId == checkedUserBorrowingCartId && intlibraryUserId == checkedUserId)
                return true;
            else
                return false;
            
        }
    }
}
