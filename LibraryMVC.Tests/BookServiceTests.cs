using AutoMapper;
using FluentAssertions;
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
                config.AddProfile(new GenreProfile());
                config.AddProfile(new AuthorProfile());
                config.AddProfile(new PublisherProfile());
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
            result.Should().Be(testId);
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

            result.Should().Be(testBookVm);
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

            result.Should().BeNull();
        }

        [Fact]
        public void GetAllBooksForList_ValidArguments_ReturnsFilteredBooksVm()
        {
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            var mapper = GetMapper();
            int testPageSize = 2;
            int testPageNumber = 1;
            string testSearchString = "Pride";
            var expectedBook = new BookForListVm()
            {
                Id = 2,
                Title = "Pride and Prejudice",
                AuthorFullName = "Jane Austen",
                Genre = "Romance",
                RelaseYear = 1813
            };

            var books = GetBooks();
            mockBookRepo.Setup(r => r.GetAllBooks()).Returns(books.AsQueryable());

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mapper);

            var result = service.GetAllBooksForList(testPageSize, testPageNumber, testSearchString);

            result.Should().NotBeNull();
            result.PageSize.Should().Be(testPageSize);
            result.CurrentPage.Should().Be(testPageNumber);
            result.SearchString.Should().Be(testSearchString);
            result.Count.Should().Be(1);
            result.Books.Should().HaveCount(1);
            result.Books.FirstOrDefault().Should().BeEquivalentTo(expectedBook);

        }

        [Fact]
        public void GetAllBooksForList_NullSearchString_ReturnsAllBooksForPageSize()
        {
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            var mapper = GetMapper();
            int testPageSize = 2;
            int testPageNumber = 1;

            var books = GetBooks();
            mockBookRepo.Setup(r => r.GetAllBooks()).Returns(books.AsQueryable());

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mapper);

            var result = service.GetAllBooksForList(testPageSize, testPageNumber, searchString: null);

            result.Should().NotBeNull();
            result.PageSize.Should().Be(testPageSize);
            result.CurrentPage.Should().Be(testPageNumber);
            result.SearchString.Should().BeEmpty();
            result.Count.Should().Be(books.Count);
            result.Books.Should().HaveCount(testPageSize);
        }

        [Fact]
        public void GetAllBooksForList_NegativePageSize_ThrowsArgumentException()
        {
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            var mockMapper = new Mock<IMapper>();
            int testPageSize = -2;
            int testPageNumber = 1;

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mockMapper.Object);

            Action result = () => service.GetAllBooksForList(testPageSize, testPageNumber, string.Empty);

            result.Should().Throw<ArgumentException>().WithMessage("Values for pageSize and pageNumber must be greater than zero.");
        }

        [Fact]
        public void GetAllBooksForList_NegativePageNumber_ThrowsArgumentException()
        {
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            var mockMapper = new Mock<IMapper>();
            int testPageSize = 2;
            int testPageNumber = -1;

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mockMapper.Object);

            Action result = () => service.GetAllBooksForList(testPageSize, testPageNumber, string.Empty);

            result.Should().Throw<ArgumentException>().WithMessage("Values for pageSize and pageNumber must be greater than zero.");
        }

        [Fact]
        public void GetInfoForAddNewBook_WhenCalled_ReturnsProperlyInitializedModel()
        {
            var mockBookRepo = new Mock<IBookRepository>();
            var mockGenreRepo = new Mock<IGenreRepository>();
            var mockAuthorRepo = new Mock<IAuthorRepository>();
            var mockPublisherRepo = new Mock<IPublisherRepository>();
            var mapper = GetMapper();

            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Fantasy" },
                new Genre { Id = 2, Name = "Science Fiction" }
            };

            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "George", LastName = "Martin" },
                new Author { Id = 2, FirstName = "Isaac", LastName = "Asimov" }
            };

            var publishers = new List<Publisher>
            {
                new Publisher { Id = 1, Name = "Penguin Books" },
                new Publisher { Id = 2, Name = "HarperCollins" }
            };

            mockGenreRepo.Setup(r => r.GetAllGenres()).Returns(genres.AsQueryable());
            mockPublisherRepo.Setup(r => r.GetAllPublishers()).Returns(publishers.AsQueryable());
            mockAuthorRepo.Setup(r => r.GetAllAuthors()).Returns(authors.AsQueryable());

            var service = new BookService(mockBookRepo.Object, mockGenreRepo.Object, mockAuthorRepo.Object, mockPublisherRepo.Object, mapper);

            var result = service.GetInfoForAddNewBook();

            result.Should().NotBeNull();
            result.NewBook.Should().NotBeNull();
            result.BookFormLists.Should().NotBeNull();
            result.BookFormLists.AllAuthors.Authors.Count.Should().Be(authors.Count);
            result.BookFormLists.AllGenres.Genres.Count.Should().Be(genres.Count);
            result.BookFormLists.AllPublishers.Publishers.Count.Should().Be(publishers.Count);
        }
    }
}
