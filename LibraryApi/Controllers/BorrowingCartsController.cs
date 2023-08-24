using LibraryApi.Filters;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Domain.Model;
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
        private readonly ILogger<BorrowingCartsController> _logger;

        public BorrowingCartsController(ILoanService loanService, ILogger<BorrowingCartsController> logger)
        {
            _loanService = loanService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<BorrowingCartDetailsVm> GetBorrowingCart()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return BadRequest("Brak Id użytkownika.");
            }

            try
            {
                var model = _loanService.GetBorrowingCartForDetailsByIndentityUserId(userId);
                return Ok(model);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "Error while fetching borrowing cart for userId = {userId}", userId);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddToBorrowingCart([FromBody] int bookId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_loanService.IsBorrowingCartFull(userId))
                return BadRequest("Koszyk jest pełny.");

            if (_loanService.IsBookInBorrowingCart(bookId, userId))
                return BadRequest("Książka już znajduje się w koszyku.");

            try
            {
                _loanService.AddToBorrowingCart(bookId, userId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

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
