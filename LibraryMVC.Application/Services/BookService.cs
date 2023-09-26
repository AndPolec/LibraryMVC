using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryMVC.Application.Helpers;
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
using System.Reflection;
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

        public BookDetailsVm? GetBookForDetails(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book is null)
            {
                return null;
            }
            var bookVm = _mapper.Map<BookDetailsVm>(book);
            return bookVm;
        }

        public ListOfBookForListVm GetAllBooksForList(int pageSize, int pageNumber, string searchString)
        {
            if (searchString is null)
            {
                searchString = string.Empty;
            }

            if (pageSize < 1 || pageNumber < 1)
            {
                throw new ArgumentException("Values for pageSize and pageNumber must be greater than zero.");
            }

            var booksQuery = _bookRepository.GetAllBooks().Where(b => b.Title.StartsWith(searchString));
            var totalBooksCount = booksQuery.Count();
            var booksToReturn = booksQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ProjectTo<BookForListVm>(_mapper.ConfigurationProvider)
                .ToList();

            var bookList = new ListOfBookForListVm()
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                SearchString = searchString,
                Books = booksToReturn,
                Count = totalBooksCount
            };
            return bookList;
        }

        public CreateBookVm GetInfoForAddNewBook()
        {
            var model = new CreateBookVm() {
                NewBook = new NewBookVm(),
                BookFormLists = GetBookFormLists()
            };

            return model;
        }

        public CreateBookVm? GetInfoForBookEdit(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book is null)
            {
                return null;
            }

            var bookModel = _mapper.Map<NewBookVm>(book);
            var editBookModel = new CreateBookVm()
            {
                NewBook = bookModel,
                BookFormLists = GetBookFormLists()
            };

            return editBookModel;
        }

        public void UpdateBook(NewBookVm model)
        {
            if (!IsBookInDatabase(model.Id))
            {
                throw new NotFoundException($"Book with ID {model.Id} was not found.");
            }

            var book = _mapper.Map<Book>(model);
            _bookRepository.UpdateBook(book);
        }

        public void DeleteBook(int id)
        {
            if (!IsBookInDatabase(id))
            {
                throw new NotFoundException($"Book with ID {id} was not found.");
            }

            _bookRepository.DeleteBook(id);
        }

        public bool IsBookInDatabase(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);
            return book != null;
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
                Authors = authors,
                Count = authors.Count               
            };
            return authorList;
        }

        public int AddAuthor(NewAuthorVm model)
        {
            var author = _mapper.Map<Author>(model);
            int id = _authorRepository.AddAuthor(author);
            return id;
        }

        public BookFormListsVm GetBookFormLists()
        {
            var genres = GetAllGenresForList();
            var publishers = GetAllPublishersForList();
            var authors = GetAllAuthorsForList();
            var model = new BookFormListsVm
            {
                AllAuthors = authors,
                AllGenres = genres,
                AllPublishers = publishers
            };
            return model;
        }
    }
}
