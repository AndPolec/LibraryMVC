using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryApi.Filters;
using LibraryApi.Models;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Loan;
using LibraryMVC.Application.ViewModels.ReturnRecord;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IValidator<LoanSettingsVm> _validatorLoanSettingsVm;
        private readonly IValidator<NewReturnRecordVm> _validatorNewReturnRecordVm;
        private readonly ILibraryUserService _libraryUserService;

        public LoansController(ILoanService loanService, IValidator<LoanSettingsVm> validatorLoanSettingsVm, ILibraryUserService libraryUserService, IValidator<NewReturnRecordVm> validatorNewReturnRecordVm)
        {
            _loanService = loanService;
            _validatorLoanSettingsVm = validatorLoanSettingsVm;
            _libraryUserService = libraryUserService;
            _validatorNewReturnRecordVm = validatorNewReturnRecordVm;
        }

        [HttpGet]
        public ActionResult<ListOfLoanForListVm> GetUserLoans(int pageSize = 10, int pageNumber = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return BadRequest("Brak Id użytkownika.");
            }

            var model = _loanService.GetAllLoansForListByIndentityUserId(userId, pageSize, pageNumber);
            return Ok(model);
        }

        [HttpGet("{id}")]
        [CheckViewLoanPermission]
        public ActionResult<LoanDetailsVm> GetLoan(int id)
        {
            var model = _loanService.GetLoanForDetails(id);
            if(model is null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        [CheckCreateLoanPermission]
        public IActionResult CreateLoan([FromBody] CreateLoanModel createRequest)
        {
            if (_libraryUserService.IsBlocked(createRequest.UserId))
            {
                return Forbid("Użytkownik zablokowany.");
            }

            var loanId = _loanService.AddNewLoan(createRequest.BorrowingCartId, createRequest.UserId);
            if (loanId == -1)
            {
                return BadRequest("Zamówienie nie zostało utworzone. Wybrane książki są niedostępne.");
            }

            return CreatedAtAction(nameof(GetLoan), new { id = loanId }, null);
        }

        [HttpDelete("{id}")]
        [CheckViewLoanPermission]
        public IActionResult CancelLoan(int id) 
        {
            var result = _loanService.CancelLoan(id);
            if (result)
            {
                return Ok("Zamówienie anulowane.");
            }
            else
            {
                return BadRequest("Nie znaleziono zamówienia o podanym id.");
            }
        }

        [HttpGet("confirm-checkout")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public ActionResult<LoanForConfirmCheckOutListVm> GetLoansForConfirmCheckOut()
        {
            var model = _loanService.GetAllLoansForConfirmCheckOutList();
            if(model.Count == 0)
                return NoContent();
            return Ok(model);
        }

        [HttpPut("{loanId}/confirm-checkout")]
        [Authorize(Roles = "Bibliotekarz")]
        public IActionResult ConfirmCheckOut(int loanId)
        {
            var librarianId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int result = _loanService.ConfirmCheckOut(loanId, librarianId);
            if (result == -1)
                return BadRequest("Nie znaleziono zamówienia o podanym id.");
            else
                return Ok();
        }

        [HttpGet("confirm-return")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public ActionResult<LoanForConfirmCheckOutListVm> GetLoansForConfirmReturn()
        {
            var model = _loanService.GetAllLoansForConfirmReturnList();
            if (model.Count == 0)
                return NoContent();
            return Ok(model);
        }

        [HttpPut("{loanId}/confirm-return")]
        [Authorize(Roles = "Bibliotekarz")]
        public IActionResult ConfirmReturn(int loanId, [FromBody] NewReturnRecordVm returnRecord) 
        {
            var result = _validatorNewReturnRecordVm.Validate(returnRecord);
            result.AddToModelState(ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var librarianId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _loanService.ConfirmReturn(returnRecord, librarianId);
            return Ok();
        }
    }
}
