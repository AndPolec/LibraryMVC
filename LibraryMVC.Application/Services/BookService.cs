using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Author;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Application.ViewModels.Publisher;
using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
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
        private readonly IGenreRepository _genreRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IGenreRepository genreRepository, IAuthorRepository authorRepository, IPublisherRepository publisherRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _mapper = mapper;
        }

        public int AddBook(NewBookVm model)
        {
            var newBook = _mapper.Map<Book>(model);
            var newBookId = _bookRepository.AddBook(newBook);
            return newBookId;
        }

        public BookDetailsVm GetBookForDetails(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);
            var bookVm = _mapper.Map<BookDetailsVm>(book);
            return bookVm;
        }

        public ListOfBookForListVm GetAllBooksForList(int pageSize, int pageNumber, string searchString)
        {
            var books = _bookRepository.GetAllBooks().Where(b => b.Title.StartsWith(searchString))
                .ProjectTo<BookForListVm>(_mapper.ConfigurationProvider).ToList();

            var booksToShow = books.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            var bookList = new ListOfBookForListVm()
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                SearchString = searchString,
                Books = booksToShow,
                Count = books.Count
            };
            return bookList;
           
        }

        public NewBookVm GetInfoForAddNewBook()
        {
            var bookModel = new NewBookVm();

            SetParametersToVm(bookModel);

            return bookModel;
        }

        public NewBookVm GetInfoForBookEdit(int id)
        {
            var book = _bookRepository.GetBookById(id);
            var bookModel = _mapper.Map<NewBookVm>(book);

            SetParametersToVm(bookModel);

            return bookModel;
        }

        public void UpdateBook(NewBookVm model)
        {
            var book = _mapper.Map<Book>(model);
            _bookRepository.UpdateBook(book);
        }

        public void DeleteBook(int id)
        {
            _bookRepository.DeleteBook(id);
        }

        public ListOfGenreForListVm GetAllGenresForList()
        {
            var genres = _genreRepository.GetAllGenres().OrderBy(g => g.Name).ProjectTo<GenreForListVm>(_mapper.ConfigurationProvider).ToList();
            var genreList = new ListOfGenreForListVm()
            {
                Genres = genres
            };
            return genreList;
        }

        public int AddGenre(GenreForListVm model)
        {
            var genre = _mapper.Map<Genre>(model);
            int id = _genreRepository.AddGenre(genre);
            return id;
        }

        public ListOfPublisherForListVm GetAllPublishersForList()
        {
            var publishers = _publisherRepository.GetAllPublishers().OrderBy(p => p.Name).ProjectTo<PublisherForListVm>(_mapper.ConfigurationProvider).ToList();
            var publishersList = new ListOfPublisherForListVm()
            {
                Publishers = publishers
            };
            return publishersList;
        }

        public int AddPublisher(PublisherForListVm model)
        {
            var publisher = _mapper.Map<Publisher>(model);
            int id = _publisherRepository.AddPublisher(publisher);
            return id;
        }

        public ListOfAuthorForListVm GetAllAuthorsForList()
        {
            var authors = _authorRepository.GetAllAuthors().OrderBy(a => a.FirstName).ProjectTo<AuthorForListVm>(_mapper.ConfigurationProvider).ToList();
            var authorList = new ListOfAuthorForListVm() 
            { 
                Authors = authors 
            };
            return authorList;
        }

        public int AddAuthor(NewAuthorVm model)
        {
            var author = _mapper.Map<Author>(model);
            int id = _authorRepository.AddAuthor(author);
            return id;
        }

        public NewBookVm SetParametersToVm(NewBookVm model)
        {
            var genres = GetAllGenresForList();
            var publishers = GetAllPublishersForList();
            var authors = GetAllAuthorsForList();

            model.AllGenres = genres;
            model.AllPublishers = publishers;
            model.AllAuthors = authors;

            return model;
        }
    }
}
