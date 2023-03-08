using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        [HttpPost]
        public IActionResult CreateNewLoan(int borrowingCartId, int userId)
        {
            var loanId = _loanService.AddNewLoan(borrowingCartId, userId);

            return RedirectToAction("Index");
        }
    }
}
