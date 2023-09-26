using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryApi.Filters;
using LibraryApi.Models;
using LibraryMVC.Application.Helpers;
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
        private readonly ILogger _logger;
        private readonly ILibraryUserService _libraryUserService;

        public LoansController(ILoanService loanService, IValidator<LoanSettingsVm> validatorLoanSettingsVm, ILibraryUserService libraryUserService, IValidator<NewReturnRecordVm> validatorNewReturnRecordVm, ILogger logger)
        {
            _loanService = loanService;
            _validatorLoanSettingsVm = validatorLoanSettingsVm;
            _libraryUserService = libraryUserService;
            _validatorNewReturnRecordVm = validatorNewReturnRecordVm;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<ListOfLoanForListVm> GetUserLoans(int pageSize = 10, int pageNumber = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return BadRequest("Brak Id użytkownika.");
            }

            try
            {
                var model = _loanService.GetAllLoansForListByIndentityUserId(userId, pageSize, pageNumber);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using UnblockUser with userId={userId}.", userId);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [CheckViewLoanPermission]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<LoanDetailsVm> GetLoan(int id)
        {
            try
            {
                var model = _loanService.GetLoanForDetails(id);
                if (model is null)
                {
                    return NotFound();
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using GetLoan with id={id}.", id);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [CheckCreateLoanPermission]
        public IActionResult CreateLoan([FromBody] CreateLoanModel createRequest)
        {
            if (_libraryUserService.IsBlocked(createRequest.UserId))
            {
                return Forbid("Użytkownik zablokowany.");
            }

            try
            {
                var loanId = _loanService.AddNewLoan(createRequest.BorrowingCartId, createRequest.UserId);
                if (loanId == -1)
                {
                    return BadRequest("Zamówienie nie zostało utworzone. Wybrane książki są niedostępne.");
                }

                return CreatedAtAction(nameof(GetLoan), new { id = loanId }, null);
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation(ex, "Error while using CreateLoan with createRequest={createRequest}.", createRequest);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using CreateLoan with createRequest={createRequest}.", createRequest);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [CheckViewLoanPermission]
        public IActionResult CancelLoan(int id) 
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using CancelLoan with id={id}.", id);
                return StatusCode(500);
            }
        }

        [HttpGet("confirm-checkout")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<LoanForConfirmCheckOutListVm> GetLoansForConfirmCheckOut()
        {
            try
            {
                var model = _loanService.GetAllLoansForConfirmCheckOutList();
                if (model.Count == 0)
                {
                    return NoContent();
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using GetLoansForConfirmCheckOut.");
                return StatusCode(500);
            }
        }

        [HttpPut("{loanId}/confirm-checkout")]
        [Authorize(Roles = "Bibliotekarz")]
        public IActionResult ConfirmCheckOut(int loanId)
        {
            var librarianId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                int result = _loanService.ConfirmCheckOut(loanId, librarianId);
                if (result == -1)
                {
                    return BadRequest("Nie znaleziono zamówienia o podanym id.");
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using ConfirmCheckOut with loanId={loanId}.", loanId);
                return StatusCode(500);
            }
        }

        [HttpGet("confirm-return")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<LoanForConfirmCheckOutListVm> GetLoansForConfirmReturn()
        {
            try
            {
                var model = _loanService.GetAllLoansForConfirmReturnList();
                if (model.Count == 0)
                {
                    return NoContent();
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using GetLoansForConfirmReturn.");
                return StatusCode(500);
            }
        }

        [HttpPut("confirm-return")]
        [Authorize(Roles = "Bibliotekarz")]
        public IActionResult ConfirmReturn([FromBody] NewReturnRecordVm returnRecord) 
        {
            var result = _validatorNewReturnRecordVm.Validate(returnRecord);
            result.AddToModelState(ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var librarianId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var returnRecordId = _loanService.ConfirmReturn(returnRecord, librarianId);
                if (returnRecordId == -1) 
                {
                    return NotFound("Nie znaleziono zamówienia o podanym id.");
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while using ConfirmReturn with returnRecord={returnRecord}.", returnRecord);
                return StatusCode(500);
            }
        }
    }
}
