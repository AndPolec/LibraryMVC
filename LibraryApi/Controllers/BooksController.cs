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
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IValidator<NewBookVm> _validator;


        public BooksController(ILogger<BooksController> logger,IBookService bookService, IValidator<NewBookVm> newBookValidator)
        {
            _logger = logger;
            _bookService = bookService;
            _validator = newBookValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<ListOfBookForListVm> GetBooks(string? searchString, int pageSize = 10, int pageNumber = 1)
        {
            ListOfBookForListVm result;
            try
            {
                result = _bookService.GetAllBooksForList(pageSize, pageNumber, searchString ?? string.Empty);
            }
            catch (ArgumentException ex)
            {
                _logger.LogInformation(ex, "Error while fetching books for searchString={searchString}, pageSize={pageSize}, pageNumber={pageNumber}.",searchString,pageSize,pageNumber);
                return BadRequest(ex.Message);
            }            
            
            if (result.Count == 0)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
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
        public IActionResult AddBook([FromBody] NewBookVm book)
        {
            var validationResult = _validator.Validate(book);
            validationResult.AddToModelState(ModelState);

            if (ModelState.IsValid && book.Id == 0)
            {
                int id = _bookService.AddBook(book);
                return CreatedAtAction(nameof(GetBook), new { id }, null);
            }
            
            if (book.Id != 0)
            {
                ModelState.AddModelError("Id", "Id musi być równe 0.");
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                _bookService.DeleteBook(id);
                return Ok();

            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "Error while deleting book for ID = {id}", id);
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Bibliotekarz,Administrator")]
        public IActionResult EditBook([FromBody] NewBookVm book)
        {
            var validationResult = _validator.Validate(book);
            validationResult.AddToModelState(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _bookService.UpdateBook(book);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "Error while updating book for ID = {id}", book.Id);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("authors")]
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<ListOfAuthorForListVm> GetAuthors()
        {
            var authors = _bookService.GetAllAuthorsForList();
            return Ok(authors);
        }

        [HttpGet("genres")]
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<ListOfGenreForListVm> GetGenres()
        {
            var genres = _bookService.GetAllGenresForList();
            return Ok(genres);
        }

        [HttpGet("publishers")]
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<ListOfPublisherForListVm> GetPublishers()
        {
            var publishers = _bookService.GetAllPublishersForList();
            return Ok(publishers);
        }
    }
}
