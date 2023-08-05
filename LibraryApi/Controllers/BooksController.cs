﻿using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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

     
        
    }
}
