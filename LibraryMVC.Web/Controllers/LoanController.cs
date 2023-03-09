using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _loanService.GetAllLoansForListByIndentityUserId(userId,5,1);
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateNewLoan(int borrowingCartId, int userId)
        {
            var loanId = _loanService.AddNewLoan(borrowingCartId, userId);

            return RedirectToAction("Index");
        }
    }
}
