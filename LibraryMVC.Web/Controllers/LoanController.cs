
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.Loan;
using LibraryMVC.Application.ViewModels.ReturnRecord;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IValidator<NewReturnRecordVm> _validatorNewReturnRecordVm; 
        private readonly IValidator<LoanSettingsVm> _validatorLoanSettingsVm; 
        private readonly ILibraryUserService _libraryUserService;

        public LoanController(ILoanService loanService, ILibraryUserService libraryUserService, IValidator<NewReturnRecordVm> validatorNewReturnRecordVm,IValidator<LoanSettingsVm> validatorLoanSettingsVm)
        {
            _loanService = loanService;
            _libraryUserService = libraryUserService;
            _validatorNewReturnRecordVm = validatorNewReturnRecordVm;
            _validatorLoanSettingsVm = validatorLoanSettingsVm;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _loanService.GetAllLoansForListByIndentityUserId(userId,10,1);
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(int pageSize, int? pageNumber)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _loanService.GetAllLoansForListByIndentityUserId(userId, pageSize, pageNumber.Value);
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateNewLoan(int borrowingCartId, int userId)
        {
            if (_libraryUserService.IsBlocked(userId))
            {
                TempData["error"] = "Twoje konto zostało zablokowane. Nie możesz stworzyć kolejnego zamówienia.";
                return RedirectToAction("Index", "BorrowingCart");
            }

            var loanId = _loanService.AddNewLoan(borrowingCartId, userId);
            if (loanId == -1)
            {
                TempData["error"] = "Zamówienie nie zostało utworzone. Wybrane książki nie są dostępne.";
                return RedirectToAction("Index", "BorrowingCart");
            }

            TempData["success"] = $"Zamówienie nr {loanId} zostało utworzone.";
            return RedirectToAction("Index", "BorrowingCart");
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
            {
                TempData["success"] = "Zamówienie anulowane.";
                return RedirectToAction("ViewLoan", new { loanId = loanId });
            }
            else
            {
                TempData["error"] = "Zamówienie nie zostało anulowane.";
                return RedirectToAction("ViewLoan", new { loanId = loanId });
            }
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
            TempData["success"] = $"Potwierdzono wydanie dla zamówienia nr {loanId}.";
            return RedirectToAction("ConfirmCheckOut");
        }

        [HttpGet]
        public IActionResult ConfirmReturnIndex()
        {
            var model = _loanService.GetAllLoansForConfirmReturnList();
            return View(model);
        }

        [HttpGet]
        public IActionResult ConfirmReturn(int loanId)
        {
            var model = _loanService.GetInfoForConfirmReturn(loanId);
            return View(model);
        }

        [HttpPost]
        public IActionResult ConfirmReturn(NewReturnRecordVm model)
        {
            var result = _validatorNewReturnRecordVm.Validate(model);
            int numberOfSelectedBooks = model.LostOrDestroyedBooksId.Count + model.ReturnedBooksId.Count;

            if (numberOfSelectedBooks < model.NumberOfBorrowedBooks)
                ModelState.AddModelError("BorrowedBooks","Zaznacz status dla wszystkich wypożyczonych książek");

            if (numberOfSelectedBooks > model.NumberOfBorrowedBooks)
                ModelState.AddModelError("BorrowedBooks", "Książka nie może być oznaczona jednocześnie jako zgubiona i oddana.");

            if (!result.IsValid)
                result.AddToModelState(ModelState);

            if (!ModelState.IsValid)
            {
                model = _loanService.SetParametersToVm(model);
                return View(model);
            }

            var librarianId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var returnRecordId = _loanService.ConfirmReturn(model, librarianId);

            return RedirectToAction("ViewReturnRecord", returnRecordId);
        }

        [HttpGet]
        public IActionResult ViewReturnRecord(int returnRecordId)
        {
            returnRecordId = 1;
            var model = _loanService.GetReturnRecordForDetails(returnRecordId);
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangeGlobalLoanSettings()
        {
            var model =_loanService.GetGlobalLoanSettings();
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeGlobalLoanSettings(LoanSettingsVm model)
        {
            var result = _validatorLoanSettingsVm.Validate(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(model);
            }

            _loanService.SetGlobalLoanSettings(model);
            TempData["success"] = "Ustawienia zostały zmienione.";
            return View(model);
        }
    }
}
