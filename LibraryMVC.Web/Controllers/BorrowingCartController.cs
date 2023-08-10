using LibraryMVC.Application.Interfaces;
using LibraryMVC.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize]
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _loanService.GetBorrowingCartForDetailsByIndentityUserId(userId);

            return View(model);
        }

        [HttpGet]
        public IActionResult AddToBorrowingCart(int bookId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_loanService.IsBorrowingCartFull(userId))
            {
                TempData["error"] = "Koszyk jest pełny.";
                return RedirectToAction("ViewBook", "Book", new { id = bookId });
            }

            if (_loanService.IsBookInBorrowingCart(bookId, userId))
            {
                TempData["warning"] = "Książka już znajduje się w koszyku.";
                return RedirectToAction("ViewBook", "Book", new { id = bookId });
            }

            _loanService.AddToBorrowingCart(bookId, userId);
            TempData["success"] = "Książka dodana do koszyka.";

            return RedirectToAction("ViewBook", "Book", new { id = bookId });
        }

        [HttpGet("RemoveFromBorrowingCart/{bookId}/{borrowingCartId}")]
        [CheckBorrowingCartPermission]
        public IActionResult RemoveFromBorrowingCart(int bookId, int borrowingCartId)
        {
            _loanService.RemoveFromBorrowingCart(bookId, borrowingCartId);
            return RedirectToAction("Index");
        }

        [HttpGet("RemoveAllFromBorrowingCart/{borrowingCartId}")]
        [CheckBorrowingCartPermission]
        public IActionResult RemoveAllFromBorrowingCart(int borrowingCartId)
        {
            _loanService.ClearBorrowingCart(borrowingCartId);
            return RedirectToAction("Index");
        }
    }
}
