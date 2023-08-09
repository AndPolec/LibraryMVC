using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BorrowingCartsController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public BorrowingCartsController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public IActionResult GetBorrowingCart()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _loanService.GetBorrowingCartForDetailsByIndentityUserId(userId);
            return Ok(model);
        }

        [HttpPost("{bookId}")]
        public IActionResult AddToBorrowingCart(int bookId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_loanService.IsBorrowingCartFull(userId))
                return BadRequest("Koszyk jest pełny.");

            if (_loanService.IsBookInBorrowingCart(bookId, userId))
                return BadRequest("Książka już znajduje się w koszyku.");

            _loanService.AddToBorrowingCart(bookId, userId);
            return Ok("Książka dodana do koszyka.");
        }

        [HttpDelete("{bookId}/{borrowingCartId}")]
        public IActionResult RemoveFromBorrowingCart(int bookId, int borrowingCartId)
        {
            _loanService.RemoveFromBorrowingCart(bookId, borrowingCartId);
            return Ok("Książka usunięta z koszyka.");
        }

        [HttpDelete("all/{borrowingCartId}")]
        public IActionResult RemoveAllFromBorrowingCart(int borrowingCartId)
        {
            _loanService.ClearBorrowingCart(borrowingCartId);
            return Ok("Wszystkie książki usunięte z koszyka.");
        }
    }
}
