using LibraryApi.Models;
using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApi.Filters
{
    public class CheckCreateLoanPermission : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var loanService = context.HttpContext.RequestServices.GetRequiredService<ILoanService>();
            var userService = context.HttpContext.RequestServices.GetRequiredService<ILibraryUserService>();
            var identityUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            CreateLoanModel? createRequest = context.ActionArguments["createRequest"] as CreateLoanModel;

            if (createRequest == null || identityUserId == null)
            {
                context.Result = new BadRequestResult();
                return;
            }

            bool isAuthorized = CheckUserPermission(identityUserId, createRequest.UserId, createRequest.BorrowingCartId, loanService, userService);
            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }

        private bool CheckUserPermission(string identityUserId, int libraryUserId, int borrowingCartId, ILoanService loanService, ILibraryUserService userService)
        {
            int checkedUserId = userService.GetLibraryUserIdByIdentityUserId(identityUserId);
            int checkedUserBorrowingCartId = loanService.GetBorrowingCartForDetailsByIndentityUserId(identityUserId).Id;

            return (borrowingCartId == checkedUserBorrowingCartId && libraryUserId == checkedUserId);
        }
    }
}
