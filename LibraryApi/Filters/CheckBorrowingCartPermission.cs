using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace LibraryApi.Filters
{
    public class CheckBorrowingCartPermission : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var loanService = context.HttpContext.RequestServices.GetRequiredService<ILoanService>();
            var identityUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var borrowingCartId = context.RouteData.Values["borrowingCartId"]?.ToString();

            if (identityUserId == null || borrowingCartId == null || !CheckUserPermission(identityUserId, borrowingCartId, loanService))
            {
                context.Result = new ForbidResult();
            }
        }

        private bool CheckUserPermission(string identityUserId, string borrowingCartId, ILoanService loanService)
        {
            var borrowingCart = loanService.GetBorrowingCartForDetailsByIndentityUserId(identityUserId);
            if (borrowingCart == null)
                return false;

            return borrowingCart.Id == int.Parse(borrowingCartId);
        }
    }
}
