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
            var borrowingCart = _loanService.GetBorrowingCart(userId);
            
            return View();
        }
    }
}
