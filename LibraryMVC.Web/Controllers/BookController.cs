using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Book;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Drawing.Text;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IValidator<NewBookVm> _validator;

        public BookController(IBookService bookService, IValidator<NewBookVm> newBookValidator)
        {
            _bookService = bookService;
            _validator = newBookValidator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _bookService.GetAllBooksForList(5,1,"");
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

            var id = _bookService.AddBook(model);
            return RedirectToAction("AdminBookPanel");
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
            _bookService.UpdateBook(model);
            return RedirectToAction("AdminBookPanel");
        }

        [HttpGet]
        public IActionResult DeleteBook(int id)
        {
            _bookService.DeleteBook(id);
            return RedirectToAction("AdminBookPanel");
        }
    }
}
