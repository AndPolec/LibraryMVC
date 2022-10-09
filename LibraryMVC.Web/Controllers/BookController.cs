using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Web.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            var model = bookService.GetAllBooksForList();
            return View();
        }

        public IActionResult ViewBook(int bookId)
        {
            var bookModel = bookService.GetBook(bookId);
            return View(bookModel);
        }

        [HttpGet]
        public IActionResult AddNewBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewBook(BookModel model)
        {
            var id = bookService.AddBook(model);
            return View();
        }
    }
}
