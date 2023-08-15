using AutoMapper;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryMVC.Tests
{
    public class BookServiceTests
    {
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
    }
}
