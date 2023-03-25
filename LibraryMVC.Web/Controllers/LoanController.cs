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

        [HttpGet]
        public IActionResult ViewLoan(int loanId)
        {
            var model = _loanService.GetLoanForDetails(loanId);
            return View(model);
        }

        [HttpGet]
        public IActionResult CancelLoan(int loanId)
        {
            var result = _loanService.CancelLoan(loanId);
            if (result)
                return RedirectToAction("Index");
            else
                return RedirectToAction("ViewLoan", loanId);
        }

        [HttpGet]
        public IActionResult ConfirmCheckOut()
        {
            var model = _loanService.GetAllLoansForConfirmCheckOutList();
            return View(model);
        }

        [HttpPost]
        public IActionResult ConfirmCheckOut(int loanId)
        {
            var librarianId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _loanService.ConfirmCheckOut(loanId, librarianId);
            return RedirectToAction("ConfirmCheckOut");
        }

        [HttpGet]
        public IActionResult ConfirmReturn()
        {
            var model = _loanService.GetAllLoansForConfirmCheckOutList();
            return View(model);
        }

        [HttpPost]
        public IActionResult ConfirmReturn(int loanId, string comments, bool isPenaltyPaid)
        {
            var librarianId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _loanService.ConfirmReturn(loanId, librarianId,comments,isPenaltyPaid);
            return RedirectToAction("ConfirmReturn");
        }
    }
}
