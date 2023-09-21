using LibraryApi.Filters;
using LibraryMVC.Application.Exceptions;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
                if (model == null)
                {
                    return NoContent();
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using GetBorrowingCart");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult AddToBorrowingCart([FromBody] int bookId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try
            {
                if (_loanService.IsBorrowingCartFull(userId))
                    return BadRequest("Koszyk jest pełny.");

                if (_loanService.IsBookInBorrowingCart(bookId, userId))
                    return BadRequest("Książka już znajduje się w koszyku.");

                _loanService.AddToBorrowingCart(bookId, userId);
                return Ok("Książka dodana do koszyka.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using AddToBorrowingCart with bookId={bookId}", bookId);
                return StatusCode(500);
            }
        }

        [HttpDelete("{borrowingCartId}/{bookId}")]
        [CheckBorrowingCartPermission]
        public IActionResult RemoveFromBorrowingCart(int borrowingCartId, int bookId)
        {
            try
            {
                _loanService.RemoveFromBorrowingCart(bookId, borrowingCartId);
                return Ok("Książka usunięta z koszyka.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using RemoveFromBorrowingCart with borrowingCartId={borrowingCartId}, bookId={bookId}.", borrowingCartId, bookId);
                return StatusCode(500);
            }
        }

        [HttpDelete("{borrowingCartId}")]
        [CheckBorrowingCartPermission]
        public IActionResult RemoveAllFromBorrowingCart(int borrowingCartId)
        {
            try
            {
                var result = _loanService.ClearBorrowingCart(borrowingCartId);
                if (result)
                    return Ok("Wszystkie książki usunięte z koszyka.");
                else
                    return NotFound("Koszyk nie został znaleziony.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using RemoveAllFromBorrowingCart with borrowingCartId={borrowingCartId}.", borrowingCartId);
                return StatusCode(500);
            }
        }
    }
}
