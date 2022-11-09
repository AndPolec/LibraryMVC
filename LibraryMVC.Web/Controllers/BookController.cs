using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Book;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
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

        public IActionResult ViewBook(int bookId)
        {
            var bookModel = _bookService.GetBook(bookId);
            return View(bookModel);
        }

        [HttpGet]
        public IActionResult AddNewBook()
        {
            var addNewModel = _bookService.GetAllInfoForAddNewBook();
            return View(addNewModel);
        }

        [HttpPost]
        public IActionResult AddNewBook(AddNewBookVm model)
        {
            var id = _bookService.AddBook(model);
            return RedirectToAction("Index");
        }
    }
}
