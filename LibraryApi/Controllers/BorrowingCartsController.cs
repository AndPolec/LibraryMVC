using LibraryApi.Filters;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.BorrowingCart;
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
        public ActionResult<BorrowingCartDetailsVm> GetBorrowingCart()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return BadRequest("Brak Id użytkownika.");
            }

            var model = _loanService.GetBorrowingCartForDetailsByIndentityUserId(userId);
            return Ok(model);
        }

        [HttpPost("{bookId}")]
        public ActionResult AddToBorrowingCart(int bookId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_loanService.IsBorrowingCartFull(userId))
                return BadRequest("Koszyk jest pełny.");

            if (_loanService.IsBookInBorrowingCart(bookId, userId))
                return BadRequest("Książka już znajduje się w koszyku.");

            _loanService.AddToBorrowingCart(bookId, userId);
            return Ok("Książka dodana do koszyka.");
        }

        [HttpDelete("{borrowingCartId}/{bookId}")]
        [CheckBorrowingCartPermission]
        public IActionResult RemoveFromBorrowingCart(int borrowingCartId, int bookId)
        {
            var result = _loanService.RemoveFromBorrowingCart(bookId, borrowingCartId);
            if (result)
                return Ok("Książka usunięta z koszyka.");
            else
                return BadRequest("Książka lub koszyk nie został znaleziony.");
        }

        [HttpDelete("{borrowingCartId}")]
        [CheckBorrowingCartPermission]
        public IActionResult RemoveAllFromBorrowingCart(int borrowingCartId)
        {
            var result = _loanService.ClearBorrowingCart(borrowingCartId);
            if (result)
                return Ok("Wszystkie książki usunięte z koszyka.");
            else
                return BadRequest("Koszyk nie został znaleziony.");
        }
    }
}
