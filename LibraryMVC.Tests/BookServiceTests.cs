using AutoMapper;
using FluentAssertions;
using FluentAssertions.Equivalency;
using LibraryMVC.Application.Exceptions;
using LibraryMVC.Application.Mapping;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.Author;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Application.ViewModels.Publisher;
using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Http;
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
        private Mock<IBookRepository> _mockBookRepo;
        private Mock<IGenreRepository> _mockGenreRepo;
        private Mock<IAuthorRepository> _mockAuthorRepo;
        private Mock<IPublisherRepository> _mockPublisherRepo;
        private Mock<IMapper> _mockMapper;

        public BookServiceTests()
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            _mockBookRepo = new Mock<IBookRepository>();
            _mockGenreRepo = new Mock<IGenreRepository>();
            _mockAuthorRepo = new Mock<IAuthorRepository>();
            _mockPublisherRepo = new Mock<IPublisherRepository>();
            _mockMapper = new Mock<IMapper>();
        }

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
                        new BookGenre { Book = new Book{ Id = 1 }, Genre = new Genre { Id = 1, Name = "Adventure" }, GenreId = 1 }
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
                        new BookGenre { Book = new Book { Id = 2 }, Genre = new Genre { Id = 2, Name = "Romance" },GenreId = 2 }
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
                        new BookGenre { Book = new Book { Id = 3 }, Genre = new Genre { Id = 3, Name = "Adventure" }, GenreId = 3 }
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
                        new BookGenre { Book = new Book { Id = 4 }, Genre = new Genre { Id = 4, Name = "Fantasy" }, GenreId = 4 }
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
                        new BookGenre { Book = new Book { Id = 5 }, Genre = new Genre { Id = 5, Name = "Drama" }, GenreId = 5 }
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
            var testModel = new NewBookVm();
            var testBook = new Book();
            var testId = 1;

            _mockMapper.Setup(m => m.Map<Book>(testModel)).Returns(testBook);
            _mockBookRepo.Setup(r => r.AddBook(testBook)).Returns(testId);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            // Act
            var result = service.AddBook(testModel);

            // Assert
            result.Should().Be(testId);
            _mockMapper.Verify(m => m.Map<Book>(testModel), Times.Once);
            _mockBookRepo.Verify(r => r.AddBook(testBook), Times.Once);
        }

        [Fact]
        public void GetBookForDetails_ValidId_ReturnsBookDetailsVm()
        {
            int testId = 1;
            var testBook = new Book();
            var testBookVm = new BookDetailsVm();

            _mockBookRepo.Setup(r => r.GetBookById(testId)).Returns(testBook);
            _mockMapper.Setup(r => r.Map<BookDetailsVm>(testBook)).Returns(testBookVm);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            var result = service.GetBookForDetails(testId);

            result.Should().Be(testBookVm);
        }

        [Fact]
        public void GetBookForDetails_InvalidId_ReturnsNull()
        {
            int testId = 1;

            _mockBookRepo.Setup(r => r.GetBookById(testId)).Returns((Book?)null);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            var result = service.GetBookForDetails(testId);

            result.Should().BeNull();
        }

        [Fact]
        public void GetAllBooksForList_ValidArguments_ReturnsFilteredBooksVm()
        {
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
            _mockBookRepo.Setup(r => r.GetAllBooks()).Returns(books.AsQueryable());

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, mapper);

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
            var mapper = GetMapper();
            int testPageSize = 2;
            int testPageNumber = 1;

            var books = GetBooks();
            _mockBookRepo.Setup(r => r.GetAllBooks()).Returns(books.AsQueryable());

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, mapper);

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
            int testPageSize = -2;
            int testPageNumber = 1;

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            Action result = () => service.GetAllBooksForList(testPageSize, testPageNumber, string.Empty);

            result.Should().Throw<ArgumentException>().WithMessage("Values for pageSize and pageNumber must be greater than zero.");
        }

        [Fact]
        public void GetAllBooksForList_NegativePageNumber_ThrowsArgumentException()
        {
            int testPageSize = 2;
            int testPageNumber = -1;

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            Action result = () => service.GetAllBooksForList(testPageSize, testPageNumber, string.Empty);

            result.Should().Throw<ArgumentException>().WithMessage("Values for pageSize and pageNumber must be greater than zero.");
        }

        [Fact]
        public void GetInfoForAddNewBook_WhenCalled_ReturnsProperlyInitializedModel()
        {
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

            _mockGenreRepo.Setup(r => r.GetAllGenres()).Returns(genres.AsQueryable());
            _mockPublisherRepo.Setup(r => r.GetAllPublishers()).Returns(publishers.AsQueryable());
            _mockAuthorRepo.Setup(r => r.GetAllAuthors()).Returns(authors.AsQueryable());

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, mapper);

            var result = service.GetInfoForAddNewBook();

            result.Should().NotBeNull();
            result.NewBook.Should().NotBeNull();
            result.BookFormLists.Should().NotBeNull();
            result.BookFormLists.AllAuthors.Authors.Count.Should().Be(authors.Count);
            result.BookFormLists.AllGenres.Genres.Count.Should().Be(genres.Count);
            result.BookFormLists.AllPublishers.Publishers.Count.Should().Be(publishers.Count);
        }

        [Fact]
        public void GetInfoForBookEdit_ValidId_ReturnsInfoForBookEdit()
        {
            var mapper = GetMapper();
            var testBook = GetBooks().First();
            int testBookId = testBook.Id;
            var expectedBook = new NewBookVm()
            {
                Id = testBook.Id,
                Title = testBook.Title,
                ISBN = testBook.ISBN,
                AuthorId = testBook.AuthorId,
                GenreIds = testBook.BookGenres.Select(g => g.Genre.Id).ToList(),
                PublisherId = testBook.PublisherId,
                Quantity = testBook.Quantity,
                RelaseYear = testBook.RelaseYear
            };

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

            _mockBookRepo.Setup(r => r.GetBookById(testBookId)).Returns(testBook);
            _mockGenreRepo.Setup(r => r.GetAllGenres()).Returns(genres.AsQueryable());
            _mockPublisherRepo.Setup(r => r.GetAllPublishers()).Returns(publishers.AsQueryable());
            _mockAuthorRepo.Setup(r => r.GetAllAuthors()).Returns(authors.AsQueryable());

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, mapper);

            var result = service.GetInfoForBookEdit(testBookId);

            result.Should().NotBeNull();
            result.NewBook.Should().NotBeNull();
            result.NewBook.Should().BeEquivalentTo(expectedBook);
            result.BookFormLists.Should().NotBeNull();
            result.BookFormLists.AllAuthors.Authors.Count.Should().Be(authors.Count);
            result.BookFormLists.AllGenres.Genres.Count.Should().Be(genres.Count);
            result.BookFormLists.AllPublishers.Publishers.Count.Should().Be(publishers.Count);
        }

        [Fact]
        public void GetInfoForBookEdit_InvalidIdBookIsNotFound_ReturnsNull()
        {
            int testBookId = -1;

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            var result = service.GetInfoForBookEdit(testBookId);

            result.Should().BeNull();
        }

        [Fact]
        public void UpdateBook_BookExistInDatabase_UpdatesBook()
        {
            var testBookId = 123;
            var testBookVm = new NewBookVm { Id = testBookId };
            var testBook = new Book { Id = testBookId };

            _mockBookRepo.Setup(r => r.GetBookById(testBookId)).Returns(testBook);
            _mockMapper.Setup(m => m.Map<Book>(testBookVm)).Returns(testBook);
            _mockBookRepo.Setup(r => r.UpdateBook(testBook));

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            service.UpdateBook(testBookVm);

            _mockBookRepo.Verify(r => r.UpdateBook(testBook), Times.Once);
        }

        [Fact]
        public void UpdateBook_BookNotExistInDatabase_ThrowsNotFoundException()
        {
            var testBookVm = new NewBookVm { Id = 123 };

            _mockBookRepo.Setup(r => r.GetBookById(testBookVm.Id)).Returns((Book?)null);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            Action result = () => service.UpdateBook(testBookVm);

            result.Should().Throw<NotFoundException>().WithMessage($"Book with ID {testBookVm.Id} was not found.");
        }

        [Fact]
        public void DeleteBook_BookExistInDatabase_DeletesBook()
        {
            var testBookId = 123;
            var testBook = new Book { Id = testBookId };

            _mockBookRepo.Setup(r => r.GetBookById(testBookId)).Returns(testBook);
            _mockBookRepo.Setup(r => r.DeleteBook(testBookId));

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            service.DeleteBook(testBookId);

            _mockBookRepo.Verify(r => r.DeleteBook(testBookId), Times.Once);
        }

        [Fact]
        public void DeleteBook_BookNotExistInDatabase_ThrowsNotFoundException()
        {
            var testBookId = 123;

            _mockBookRepo.Setup(r => r.GetBookById(testBookId)).Returns((Book?)null);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            Action result = () => service.DeleteBook(testBookId);

            result.Should().Throw<NotFoundException>().WithMessage($"Book with ID {testBookId} was not found.");
        }

        [Fact]
        public void IsBookInDatabase_BookExistsInDatabase_ReturnsTrue()
        {
            int testBookId = 1;
            var testBook = new Book { Id = testBookId };

            _mockBookRepo.Setup(r => r.GetBookById(testBookId)).Returns(testBook);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            var result = service.IsBookInDatabase(testBookId);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsBookInDatabase_BookDoesNotExistInDatabase_ReturnsFalse()
        {
            int testBookId = 1;
            var testBook = new Book { Id = testBookId };

            _mockBookRepo.Setup(r => r.GetBookById(testBookId)).Returns((Book?)null);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            var result = service.IsBookInDatabase(testBookId);

            result.Should().BeFalse();
        }

        [Fact]
        public void GetAllGenresForList_WhenCalled_ReturnsList()
        {
            var mapper = GetMapper();

            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Fantasy" },
                new Genre { Id = 2, Name = "Science Fiction" }
            };

            var expectedGenres = new List<GenreForListVm>
            {
                new GenreForListVm { Id = 1, Name = "Fantasy" },
                new GenreForListVm { Id = 2, Name = "Science Fiction" }
            };

            _mockGenreRepo.Setup(r => r.GetAllGenres()).Returns(genres.AsQueryable);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, mapper);

            var result = service.GetAllGenresForList();

            result.Genres.Should().NotBeNull();
            result.Genres.Should().HaveCount(2);
            result.Genres.Should().BeEquivalentTo(expectedGenres);
        }

        [Fact]
        public void AddGenre_ValidModel_AddsGenreAndReturnsId()
        {
            var genreVm = new GenreForListVm
            {
                Id = 0,
                Name = "Fantasy"
            };

            var genre = new Genre
            {
                Id = 0,
                Name = "Fantasy"
            };

            _mockMapper.Setup(m => m.Map<Genre>(genreVm)).Returns(genre);
            _mockGenreRepo.Setup(r => r.AddGenre(genre)).Returns(1);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            var result = service.AddGenre(genreVm);

            result.Should().Be(1);
            _mockGenreRepo.Verify(r => r.AddGenre(genre), Times.Once());
        }


        [Fact]
        public void GetAllAuthorsForList_WhenCalled_ReturnsList()
        {
            var mapper = GetMapper();

            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "George", LastName = "Martin" },
                new Author { Id = 2, FirstName = "Isaac", LastName = "Asimov" }
            };

            var expectedAuthors = new List<AuthorForListVm>
            {
                new AuthorForListVm { Id = 1, FullName = "Martin George" },
                new AuthorForListVm { Id = 2, FullName = "Asimov Isaac" }
            };

            _mockAuthorRepo.Setup(r => r.GetAllAuthors()).Returns(authors.AsQueryable);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, mapper);

            var result = service.GetAllAuthorsForList();

            result.Authors.Should().NotBeNull();
            result.Authors.Should().HaveCount(2);
            result.Authors.Should().BeEquivalentTo(expectedAuthors);
        }

        [Fact]
        public void AddAuthor_ValidModel_AddsAuthorAndReturnsId()
        {
            var authorVm = new NewAuthorVm
            {
                Id = 0,
                FirstName = "George",
                LastName = "Martin"
            };

            var author = new Author
            {
                Id = 0,
                FirstName = "George",
                LastName = "Martin"
            };

            _mockMapper.Setup(m => m.Map<Author>(authorVm)).Returns(author);
            _mockAuthorRepo.Setup(r => r.AddAuthor(author)).Returns(1);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            var result = service.AddAuthor(authorVm);

            result.Should().Be(1);
            _mockAuthorRepo.Verify(r => r.AddAuthor(author), Times.Once());
        }


        [Fact]
        public void GetAllPublishersForList_WhenCalled_ReturnsList()
        {
            var mapper = GetMapper();

            var publishers = new List<Publisher>
            {
                new Publisher { Id = 1, Name = "Publisher 1" },
                new Publisher { Id = 2, Name = "Publisher 2" }
            };

            var expectedPublishers = new List<PublisherForListVm>
            {
                new PublisherForListVm { Id = 1, Name = "Publisher 1" },
                new PublisherForListVm { Id = 2, Name = "Publisher 2" }
            };

            _mockPublisherRepo.Setup(r => r.GetAllPublishers()).Returns(publishers.AsQueryable);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, mapper);

            var result = service.GetAllPublishersForList();

            result.Publishers.Should().NotBeNull();
            result.Publishers.Should().HaveCount(2);
            result.Publishers.Should().BeEquivalentTo(expectedPublishers);
        }

        [Fact]
        public void AddPublisher_ValidModel_AddsPublisherAndReturnsId()
        {
            var publisherVm = new PublisherForListVm
            {
                Id = 0,
                Name = "Publisher"
            };

            var publisher = new Publisher
            {
                Id = 0,
                Name = "Publisher"
            };

            _mockMapper.Setup(m => m.Map<Publisher>(publisherVm)).Returns(publisher);
            _mockPublisherRepo.Setup(r => r.AddPublisher(publisher)).Returns(1);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, _mockMapper.Object);

            var result = service.AddPublisher(publisherVm);

            result.Should().Be(1);
            _mockPublisherRepo.Verify(r => r.AddPublisher(publisher), Times.Once());
        }

        [Fact]
        public void GetBookFormLists_WhenCalled_ReturnsModelWithCorrectData()
        {
            var mapper = GetMapper();

            var publishers = new List<Publisher>
            {
                new Publisher { Id = 1, Name = "Publisher 1" },
                new Publisher { Id = 2, Name = "Publisher 2" }
            };

            var expectedPublishers = new List<PublisherForListVm>
            {
                new PublisherForListVm { Id = 1, Name = "Publisher 1" },
                new PublisherForListVm { Id = 2, Name = "Publisher 2" }
            };

            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "George", LastName = "Martin" },
                new Author { Id = 2, FirstName = "Isaac", LastName = "Asimov" }
            };

            var expectedAuthors = new List<AuthorForListVm>
            {
                new AuthorForListVm { Id = 1, FullName = "Martin George" },
                new AuthorForListVm { Id = 2, FullName = "Asimov Isaac" }
            };
            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Fantasy" },
                new Genre { Id = 2, Name = "Science Fiction" }
            };

            var expectedGenres = new List<GenreForListVm>
            {
                new GenreForListVm { Id = 1, Name = "Fantasy" },
                new GenreForListVm { Id = 2, Name = "Science Fiction" }
            };

            _mockGenreRepo.Setup(r => r.GetAllGenres()).Returns(genres.AsQueryable);
            _mockAuthorRepo.Setup(r => r.GetAllAuthors()).Returns(authors.AsQueryable);
            _mockPublisherRepo.Setup(r => r.GetAllPublishers()).Returns(publishers.AsQueryable);

            var service = new BookService(_mockBookRepo.Object, _mockGenreRepo.Object, _mockAuthorRepo.Object, _mockPublisherRepo.Object, mapper);
            
            var result = service.GetBookFormLists();

            result.AllAuthors.Authors.Should().BeEquivalentTo(expectedAuthors);
            result.AllGenres.Genres.Should().BeEquivalentTo(expectedGenres);
            result.AllPublishers.Publishers.Should().BeEquivalentTo(expectedPublishers);
        }
    }
}
