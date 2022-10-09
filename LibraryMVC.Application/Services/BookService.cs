using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public int AddBook(NewBookVm book)
        {
            throw new NotImplementedException();
        }

        public ListOfBookForListVm GetAllBooksForList()
        {
            var books = _bookRepository.GetAllBooks().ProjectTo<BookForListVm>(_mapper.ConfigurationProvider).ToList();
            var bookList = new ListOfBookForListVm()
            {
                Books = books,
                Count = books.Count
            };
            return bookList;
           
        }

        public BookDetailsVm GetBook(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);
            var bookVm = _mapper.Map<BookDetailsVm>(book);
            return bookVm;
        }
    }
}
