using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Author;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Application.ViewModels.Publisher;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IValidator<NewBookVm> _validator;


        public BooksController(IBookService bookService, IValidator<NewBookVm> newBookValidator)
        {
            _bookService = bookService;
            _validator = newBookValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<ListOfBookForListVm> GetBooks(string? searchString, int pageSize = 10, int pageNumber = 1)
        {
            var result = _bookService.GetAllBooksForList(pageSize, pageNumber, searchString ?? string.Empty);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDetailsVm> GetBook(int id)
        {
            var result = _bookService.GetBookForDetails(id);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public ActionResult AddBook([FromBody] NewBookVm book)
        {
            var validationResult = _validator.Validate(book);
            validationResult.AddToModelState(ModelState);

            if (ModelState.IsValid && book.Id != 0)
            {
                int id = _bookService.AddBook(book);
                return Ok(id);
            }
            else if (book.Id != 0)
            {
                ModelState.AddModelError("Id", "Id musi być równe 0.");
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public ActionResult DeleteBook(int id)
        {
            if (!_bookService.IsBookInDatabase(id))
            {
                return NotFound();
            }

            _bookService.DeleteBook(id);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public ActionResult EditBook([FromBody] NewBookVm book)
        {
            var validationResult = _validator.Validate(book);
            validationResult.AddToModelState(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_bookService.IsBookInDatabase(book.Id))
            {
                return NotFound();
            }

            _bookService.UpdateBook(book);
            return Ok();
        }

        [HttpGet("authors")]
        public ActionResult<ListOfAuthorForListVm> GetAuthors()
        {
            var authors = _bookService.GetAllAuthorsForList();
            return Ok(authors);
        }

        [HttpGet("genres")]
        public ActionResult<ListOfGenreForListVm> GetGenres()
        {
            var genres = _bookService.GetAllGenresForList();
            return Ok(genres);
        }

        [HttpGet("publishers")]
        public ActionResult<ListOfPublisherForListVm> GetPublishers()
        {
            var publishers = _bookService.GetAllPublishersForList();
            return Ok(publishers);
        }
    }
}
