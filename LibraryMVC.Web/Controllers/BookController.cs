using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Author;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Application.ViewModels.Publisher;
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
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult AdminBookPanel()
        {
            var model = _bookService.GetAllBooksForList(10, 1, "");
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
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
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult AddNewBook()
        {
            var addNewModel = _bookService.GetInfoForAddNewBook();
            return View(addNewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult AddNewBook(CreateBookVm model)
        {
            var result = _validator.Validate(model.NewBook);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                model.BookFormLists = _bookService.GetBookFormLists();
                return View(model);
            }

            var bookId = _bookService.AddBook(model.NewBook);
            TempData["success"] = "Książka została dodana.";
            return RedirectToAction("ViewBookForLibrarian", new { id = bookId });
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult EditBook(int id)
        {
            var editBookModel = _bookService.GetInfoForBookEdit(id);
            return View(editBookModel);
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult EditBook(CreateBookVm model)
        {
            var result = _validator.Validate(model.NewBook);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                model.BookFormLists = _bookService.GetBookFormLists();
                return View(model);
            }

            _bookService.UpdateBook(model.NewBook);
            TempData["success"] = "Zmiany zostały zapisane.";
            return RedirectToAction("ViewBookForLibrarian", new {id = model.NewBook.Id});
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult DeleteBook(int id)
        {
            _bookService.DeleteBook(id);
            TempData["success"] = "Książka została usunięta.";
            return RedirectToAction("AdminBookPanel");
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult ViewBookForLibrarian(int id)
        {
            var bookModel = _bookService.GetBookForDetails(id);
            return View(bookModel);
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult AddNewPublisher()
        {
            var model = new PublisherForListVm();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult AddNewPublisher(PublisherForListVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _bookService.AddPublisher(model);
            TempData["success"] = "Wydawca został dodany.";

            return RedirectToAction("AdminBookPanel");
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult AddNewGenre()
        {
            var model = new GenreForListVm();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult AddNewGenre(GenreForListVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _bookService.AddGenre(model);
            TempData["success"] = "Gatunek został dodany.";

            return RedirectToAction("AdminBookPanel");
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult AddNewAuthor()
        {
            var model = new NewAuthorVm();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult AddNewAuthor(NewAuthorVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _bookService.AddAuthor(model);
            TempData["success"] = "Autor został dodany.";

            return RedirectToAction("AdminBookPanel");
        }
    }
}
