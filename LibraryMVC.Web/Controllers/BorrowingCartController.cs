using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class BorrowingCartController : Controller
    {
        private readonly ILoanService _loanService;

        public BorrowingCartController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _loanService.GetBorrowingCart(userId);
            
            return View(model);
        }

        [HttpGet]
        public IActionResult AddToBorrowingCart(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _loanService.AddToBorrowingCart(bookId,userId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult RemoveFromBorrowingCart(int bookId, int borrowingCartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _loanService.RemoveFromBorrowingCart(bookId, borrowingCartId);
            return RedirectToAction("Index");
        }
    }
}
