using LibraryMVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;

namespace LibraryMVC.Web.Controllers
{
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
            var model = _bookService.GetAllBooksForList(3,1,"");
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
            return View();
        }

        //[HttpPost]
        //public IActionResult AddNewBook(BookModel model)
        //{
        //    var id = _bookService.AddBook(model);
        //    return View();
        //}
    }
}
