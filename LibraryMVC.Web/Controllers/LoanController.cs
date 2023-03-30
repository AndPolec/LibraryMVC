using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.ReturnRecord;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IValidator<NewReturnRecordVm> _validator; 

        public LoanController(ILoanService loanService, IValidator<NewReturnRecordVm> validator)
        {
            _loanService = loanService;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _loanService.GetAllLoansForListByIndentityUserId(userId,5,1);
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateNewLoan(int borrowingCartId, int userId)
        {
            var loanId = _loanService.AddNewLoan(borrowingCartId, userId);

            return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            else
                return RedirectToAction("ViewLoan", loanId);
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
            var result = _validator.Validate(model);
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
    }
}
