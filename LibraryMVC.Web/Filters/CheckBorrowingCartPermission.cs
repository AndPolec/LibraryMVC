using LibraryMVC.Application.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace LibraryMVC.Web.Filters
{
    public class CheckBorrowingCartPermission : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var loanService = context.HttpContext.RequestServices.GetRequiredService<ILoanService>();
            var identityUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var borrowingCartId = context.HttpContext.Request.RouteValues["borrowingCartId"].ToString();

            bool isAuthorized = CheckUserPermission(identityUserId, borrowingCartId, loanService);

            if (!isAuthorized)
            {
                context.Result = new ForbidResult(); ;
            }
        }

        private bool CheckUserPermission(string identityUserId, string borrowingCartId, ILoanService loanService)
        {
            var borrowingCart = loanService.GetBorrowingCartForDetailsByIndentityUserId(identityUserId);
            return borrowingCart.Id == Int32.Parse(borrowingCartId); 
        }
    }
}
