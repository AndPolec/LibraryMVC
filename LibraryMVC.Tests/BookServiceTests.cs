using AutoMapper;
using LibraryMVC.Application.Mapping;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryMVC.Tests
{
    public class BookServiceTests
    {

        private List<Book> GetBooks()
        {
            var books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "The Wind in the Willows",
                    ISBN = "9780123456478",
                    RelaseYear = 1908,
                    Quantity = 10,
                    Genres = new List<Genre>
                    {
                        new Genre { Id = 1, Name = "Adventure" }
                    },
                    Author = new Author { Id = 1, FirstName = "Kenneth", LastName = "Grahame" },
                    Publisher = new Publisher { Id = 1, Name = "Wyndham Books" },
                    BookGenres = new List<BookGenre>
                    {
                        new BookGenre { Book = null, Genre = new Genre { Id = 1, Name = "Adventure" } }
                    }
                },
                new Book
                {
                    Id = 2,
                    Title = "Pride and Prejudice",
                    ISBN = "9780123456479",
                    RelaseYear = 1813,
                    Quantity = 8,
                    Genres = new List<Genre>
                    {
                        new Genre { Id = 2, Name = "Romance" }
                    },
                    Author = new Author { Id = 2, FirstName = "Jane", LastName = "Austen" },
                    Publisher = new Publisher { Id = 2, Name = "Classic Reads" },
                    BookGenres = new List<BookGenre>
                    {
                        new BookGenre { Book = null, Genre = new Genre { Id = 2, Name = "Romance" } }
                    }
                },
                new Book
                {
                    Id = 3,
                    Title = "Moby Dick",
                    ISBN = "1234567890",
                    RelaseYear = 1851,
                    Quantity = 5,
                    Genres = new List<Genre>
                    {
                        new Genre { Id = 3, Name = "Adventure" }
                    },
                    Author = new Author { Id = 3, FirstName = "Herman", LastName = "Melville" },
                    Publisher = new Publisher { Id = 3, Name = "Sea Tales" },
                    BookGenres = new List<BookGenre>
                    {
                        new BookGenre { Book = null, Genre = new Genre { Id = 3, Name = "Adventure" } }
                    }
                },
                new Book
                {
                    Id = 4,
                    Title = "The Lord of the Rings",
                    ISBN = "9780123456480",
                    RelaseYear = 1954,
                    Quantity = 12,
                    Genres = new List<Genre>
                    {
                        new Genre { Id = 4, Name = "Fantasy" }
                    },
                    Author = new Author { Id = 4, FirstName = "J.R.R.", LastName = "Tolkien" },
                    Publisher = new Publisher { Id = 4, Name = "Middle-Earth Publishing" },
                    BookGenres = new List<BookGenre>
                    {
                        new BookGenre { Book = null, Genre = new Genre { Id = 4, Name = "Fantasy" } }
                    }
                },
                new Book
                {
                    Id = 5,
                    Title = "Crime and Punishment",
                    ISBN = "9780123456481",
                    RelaseYear = 1866,
                    Quantity = 7,
                    Genres = new List<Genre>
                    {
                        new Genre { Id = 5, Name = "Drama" }
                    },
                    Author = new Author { Id = 5, FirstName = "Fyodor", LastName = "Dostoevsky" },
                    Publisher = new Publisher { Id = 5, Name = "Russian Classics" },
                    BookGenres = new List<BookGenre>
                    {
                        new BookGenre { Book = null, Genre = new Genre { Id = 5, Name = "Drama" } }
                    }
                }
            };
            return books;
        }

        private IMapper GetMapper()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new BookProfile());
            });
            return mapperConfig.CreateMapper();
        }

        [Fact]
        public void AddBook_ValidModel_ReturnsId()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            var testModel = new NewBookVm();
            var testBook = new Book();
            var testId = 1;

            mockMapper.Setup(m => m.Map<Book>(testModel)).Returns(testBook);
            mockBookRepo.Setup(r => r.AddBook(testBook)).Returns(testId);

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mockMapper.Object);

            // Act
            var result = service.AddBook(testModel);

            // Assert
            Assert.Equal(testId, result);
            mockMapper.Verify(m => m.Map<Book>(testModel), Times.Once);
            mockBookRepo.Verify(r => r.AddBook(testBook), Times.Once);
        }

        [Fact]
        public void GetBookForDetails_ValidId_ReturnsBookDetailsVm()
        {
            var mockMapper = new Mock<IMapper>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            int testId = 1;
            var testBook = new Book();
            var testBookVm = new BookDetailsVm();

            mockBookRepo.Setup(r => r.GetBookById(testId)).Returns(testBook);
            mockMapper.Setup(r => r.Map<BookDetailsVm>(testBook)).Returns(testBookVm);

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mockMapper.Object);

            var result = service.GetBookForDetails(testId);

            Assert.Equal(testBookVm, result);
        }

        [Fact]
        public void GetBookForDetails_InvalidId_ReturnsNull()
        {
            var mockMapper = new Mock<IMapper>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            int testId = 1;

            mockBookRepo.Setup(r => r.GetBookById(testId)).Returns((Book)null);

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mockMapper.Object);

            var result = service.GetBookForDetails(testId);

            Assert.Null(result);
        }

        [Fact]
        public void GetAllBooksForList_ValidArguments_ReturnsFilteredBooksVm()
        {
            var mapper = GetMapper();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            int testPageSize = 2;
            int testPageNumber = 1;
            string testSearchString = "Pride";

            var books = GetBooks();
            mockBookRepo.Setup(r => r.GetAllBooks()).Returns(books.AsQueryable());

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object,mockAuthorRepo.Object,mockPublisherRepo.Object, mapper);

            var result = service.GetAllBooksForList(testPageSize, testPageNumber, testSearchString);

            Assert.NotNull(result);
            Assert.Equal(testPageSize, result.PageSize);
            Assert.Equal(testPageNumber, result.CurrentPage);
            Assert.Equal(testSearchString, result.SearchString);
            Assert.Equal(1, result.Count);
            Assert.Single(result.Books);
            Assert.Contains(testSearchString, result.Books.First().Title);

        }

        [Fact]
        public void GetAllBooksForList_NullSearchString_ReturnsAllBooksForPageSize()
        {
            var mapper = GetMapper();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            int testPageSize = 2;
            int testPageNumber = 1;

            var books = GetBooks();
            mockBookRepo.Setup(r => r.GetAllBooks()).Returns(books.AsQueryable());

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mapper);

            var result = service.GetAllBooksForList(testPageSize, testPageNumber, searchString: null);

            Assert.NotNull(result);
            Assert.Equal(testPageSize, result.PageSize);
            Assert.Equal(testPageNumber, result.CurrentPage);
            Assert.Equal(string.Empty, result.SearchString);
            Assert.Equal(books.Count, result.Count);
            Assert.Equal(testPageSize, result.Books.Count);
        }

        [Fact]
        public void GetAllBooksForList_NegativePageSize_ReturnsEmptyList()
        {
            var mapper = GetMapper();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            int testPageSize = -2;
            int testPageNumber = 1;

            var books = GetBooks();
            mockBookRepo.Setup(r => r.GetAllBooks()).Returns(books.AsQueryable());

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mapper);

            var result = service.GetAllBooksForList(testPageSize, testPageNumber, string.Empty);

            Assert.NotNull(result);
            Assert.Empty(result.Books);
        }

        [Fact]
        public void GetAllBooksForList_NegativePageNumber_ReturnsEmptyList()
        {
            var mapper = GetMapper();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            int testPageSize = 2;
            int testPageNumber = -1;

            var books = GetBooks();
            mockBookRepo.Setup(r => r.GetAllBooks()).Returns(books.AsQueryable());

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mapper);

            var result = service.GetAllBooksForList(testPageSize, testPageNumber, string.Empty);

            Assert.NotNull(result);
            Assert.Empty(result.Books);
        }
    }
}
