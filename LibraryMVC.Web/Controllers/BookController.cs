using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Drawing.Text;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILibraryUserService _libraryUserService;
        private readonly IValidator<NewBookVm> _validator;

        public BookController(IBookService bookService, ILibraryUserService libraryUserService, IValidator<NewBookVm> newBookValidator)
        {
            _bookService = bookService;
            _libraryUserService = libraryUserService;
            _validator = newBookValidator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _bookService.GetAllBooksForList(10,1,"");
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(int pageSize, int? pageNumber, string searchString)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (searchString is null)
            {
                searchString = String.Empty;
            }
            var model = _bookService.GetAllBooksForList(pageSize, pageNumber.Value,searchString);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Librarian,Administrator")]
        public IActionResult AdminBookPanel()
        {
            var model = _bookService.GetAllBooksForList(10, 1, "");
            return View(model);
        }

        [HttpPost]
        public IActionResult AdminBookPanel(int pageSize, int? pageNumber, string searchString)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (searchString is null)
            {
                searchString = String.Empty;
            }
            var model = _bookService.GetAllBooksForList(pageSize, pageNumber.Value, searchString);
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewBook(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.IsUserBlocked = true;
                TempData["warning"] = "Zaloguj się aby dodać książkę do koszyka.";
            }
            else if(_libraryUserService.IsBlocked(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                ViewBag.IsUserBlocked = true;
                TempData["warning"] = "Twoje konto zostało zablokowane.";
            }

            ViewBag.IsUserBlocked = false;

            var bookModel = _bookService.GetBookForDetails(id);
            return View(bookModel);
        }

        [HttpGet]
        public IActionResult AddNewBook()
        {
            var addNewModel = _bookService.GetInfoForAddNewBook();
            return View(addNewModel);
        }

        [HttpPost]
        public IActionResult AddNewBook(NewBookVm model)
        {
            var result = _validator.Validate(model);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                model = _bookService.SetParametersToVm(model);
                return View(model);
            }

            var bookId = _bookService.AddBook(model);
            TempData["success"] = "Książka została dodana.";
            return RedirectToAction("ViewBookForLibrarian", new { id = bookId });
        }

        [HttpGet]
        public IActionResult EditBook(int id)
        {
            var editBookModel = _bookService.GetInfoForBookEdit(id);
            return View(editBookModel);
        }

        [HttpPost]
        public IActionResult EditBook(NewBookVm model)
        {
            var result = _validator.Validate(model);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                model = _bookService.SetParametersToVm(model);
                return View(model);
            }

            _bookService.UpdateBook(model);
            TempData["success"] = "Zmiany zostały zapisane.";
            return RedirectToAction("ViewBookForLibrarian", new {id = model.Id});
        }

        [HttpGet]
        public IActionResult DeleteBook(int id)
        {
            _bookService.DeleteBook(id);
            TempData["success"] = "Książka została usunięta.";
            return RedirectToAction("AdminBookPanel");
        }

        [HttpGet]
        public IActionResult ViewBookForLibrarian(int id)
        {
            var bookModel = _bookService.GetBookForDetails(id);
            return View(bookModel);
        }
    }
}
