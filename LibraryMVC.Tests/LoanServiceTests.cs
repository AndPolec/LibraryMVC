using AutoMapper;
using FluentAssertions;
using FluentAssertions.Common;
using LibraryMVC.Application.Helpers;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.Mapping;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Application.ViewModels.Loan;
using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static LibraryMVC.Application.Services.LoanService;

namespace LibraryMVC.Tests
{
    public class LoanServiceTests
    {
        private Mock<IBorrowingCartRepository> mockBorrowingCartRepo;
        private Mock<ILoanRepository> mockLoanRepo;
        private Mock<IBookRepository> mockBookRepo;
        private Mock<IAdditionalLibrarianInfoRepository> mockAdditionalLibrarianInfoRepo;
        private Mock<IReturnRecordRepository> mockReturnRecordRepo;
        private Mock<IGlobalLoanSettingsRepository> mockGlobalLoanSettingsRepo;
        private Mock<IMapper> mockMapper;

        public LoanServiceTests()
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            mockLoanRepo = new Mock<ILoanRepository>();
            mockBookRepo = new Mock<IBookRepository>();
            mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            mockGlobalLoanSettingsRepo = new Mock<IGlobalLoanSettingsRepository>();
            mockMapper = new Mock<IMapper>();
        }

        private IMapper GetMapper()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new BookProfile());
                config.AddProfile(new GenreProfile());
                config.AddProfile(new AuthorProfile());
                config.AddProfile(new PublisherProfile());
                config.AddProfile(new LoanProfile());
                config.AddProfile(new UserProfile());
            });
            return mapperConfig.CreateMapper();
        }

        [Fact]
        public void AddToBorrowingCart_ValidInput_ShouldUpdateBorrowingCart()
        {
            var testBorrowingCart = new BorrowingCart { Books = new List<Book>() };
            var testBook = new Book();

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(It.IsAny<string>())).Returns(testBorrowingCart);
            mockBookRepo.Setup(r => r.GetBookById(It.IsAny<int>())).Returns(testBook);

            var service = new LoanService(mockMapper.Object,mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            
            service.AddToBorrowingCart(1, "userId");


            testBorrowingCart.Books.Should().Contain(testBook);
            mockBorrowingCartRepo.Verify(r => r.UpdateBorrowingCart(testBorrowingCart), Times.Once);
        }

        [Fact]
        public void AddToBorrowingCart_BorrowingCartNotFound_ShouldThrowNotFoundException()
        {
            string testUserId = "id";

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(It.IsAny<string>())).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            Action result = () => service.AddToBorrowingCart(1, testUserId);

            result.Should().Throw<NotFoundException>().WithMessage($"No borrowing cart found for user with ID: {testUserId}");
        }

        [Fact]
        public void AddToBorrowingCart_BookNotFound_ShouldThrowNotFoundException()
        {
            int testBookId = 1;

            var testBorrowingCart = new BorrowingCart { Books = new List<Book>() };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(It.IsAny<string>())).Returns(testBorrowingCart);
            mockBookRepo.Setup(r => r.GetBookById(testBookId)).Returns((Book)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            Action result = () => service.AddToBorrowingCart(testBookId, "userId");

            result.Should().Throw<NotFoundException>().WithMessage($"No book found with ID: {testBookId}");
        }

        [Fact]
        public void GetBorrowingCartForDetailsByIndentityUserId_ValidUserId_ShouldReturnBorrowingCartModel()
        {
            string testUserId = "id";
            var testBorrowingCart = new BorrowingCart
            {
                Id = 1,
                LibraryUserId = 123,
                Books = new List<Book>() 
            };

            var expectedBorrowingCartDetailsVm = new BorrowingCartDetailsVm
            {
                Id = 1,
                LibraryUserId = 123,
                Count = 0, 
                Books = new List<BookForBorrowingCartVm>() 
            };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(testUserId)).Returns(testBorrowingCart);
            mockMapper.Setup(m => m.Map<BorrowingCartDetailsVm>(testBorrowingCart)).Returns(expectedBorrowingCartDetailsVm);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            var result = service.GetBorrowingCartForDetailsByIndentityUserId(testUserId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedBorrowingCartDetailsVm);
        }

        [Fact]
        public void GetBorrowingCartForDetailsByIndentityUserId_BorrowingCartNotFound_ShouldReturnNull()
        {
            string testUserId = "id";

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(testUserId)).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            var result = service.GetBorrowingCartForDetailsByIndentityUserId(testUserId);

            result.Should().BeNull();
        }

        [Fact]
        public void RemoveFromBorrowingCart_BorrowingCartNotFound_ShouldThrowNotFoundException()
        {
            int testBorrowingCartId = 1;
            int testBookId = 2;

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            Action result = () => service.RemoveFromBorrowingCart(testBookId,testBorrowingCartId);

            result.Should().Throw<NotFoundException>().WithMessage($"No borrowing cart found for ID: {testBorrowingCartId}");
        }

        [Fact]
        public void RemoveFromBorrowingCart_BookNotFoundInBorrowingCart_ShouldThrowNotFoundException()
        {
            int testBorrowingCartId = 1;
            int testBookId = 2;

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns(new BorrowingCart() { Id = testBorrowingCartId, Books = new List<Book>() });

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            Action result = () => service.RemoveFromBorrowingCart(testBookId, testBorrowingCartId);

            result.Should().Throw<NotFoundException>().WithMessage($"No book found for ID: {testBookId} in borrowing cart with ID: {testBorrowingCartId}");
        }

        [Fact]
        public void RemoveFromBorrowingCart_ValidInput_ShouldRemoveBookAndCallUpdate()
        {
            int testBorrowingCartId = 1;
            int testBookId = 2;
            var testBorrowingCart = new BorrowingCart() { Id = testBorrowingCartId, Books = new List<Book>() { new Book() { Id = testBookId }, new Book() { Id = 3 } } };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns(testBorrowingCart);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            service.RemoveFromBorrowingCart(testBookId,testBorrowingCartId);

            testBorrowingCart.Books.Should().HaveCount(1);
            testBorrowingCart.Books.Should().NotContain(b => b.Id == testBookId);
            mockBorrowingCartRepo.Verify(r => r.UpdateBorrowingCart(testBorrowingCart), Times.Once());
        }

        [Fact]
        public void ClearBorrowingCart_BorrowingCartNotFound_ShouldReturnFalse()
        {
            int testBorrowingCartId = 1;

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            var result = service.ClearBorrowingCart(testBorrowingCartId);

            result.Should().BeFalse();
        }

        [Fact]
        public void ClearBorrowingCart_BorrowingCartFound_ShouldCallRemoveMethodAndReturnTrue()
        {
            int testBorrowingCartId = 1;
            var testBorrowingCart = new BorrowingCart() { Id = testBorrowingCartId, Books = new List<Book>() { new Book() { Id = 2 }, new Book() { Id = 3 } } };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns(testBorrowingCart);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            var result = service.ClearBorrowingCart(testBorrowingCartId);

            result.Should().BeTrue();
            mockBorrowingCartRepo.Verify(r => r.RemoveAllFromBorrowingCart(testBorrowingCart),Times.Once());
        }

        [Fact]
        public void IsBookInBorrowingCart_BorrowingCartNotFound_ShouldThrowNotFoundException()
        {
            string testIdentityUserId = "testId";
            int testBookId = 1;

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(testIdentityUserId)).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            Action result = () => service.IsBookInBorrowingCart(testBookId,testIdentityUserId);

            result.Should().Throw<NotFoundException>().WithMessage($"No borrowing cart found for UserId: {testIdentityUserId}");
        }

        [Fact]
        public void IsBookInBorrowingCart_BookFoundInBorrowingCart_ShouldReturnTrue()
        {
            string testIdentityUserId = "testId";
            int testBookId = 2;
            var testBorrowingCart = new BorrowingCart() { Books = new List<Book>() { new Book() { Id = 2 }, new Book() { Id = 3 } } };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(testIdentityUserId)).Returns(testBorrowingCart);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            var result = service.IsBookInBorrowingCart(testBookId, testIdentityUserId);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsBookInBorrowingCart_BookNotFoundInBorrowingCart_ShouldReturnFalse()
        {
            string testIdentityUserId = "testId";
            int testBookId = 1;
            var testBorrowingCart = new BorrowingCart() { Books = new List<Book>() { new Book() { Id = 2 }, new Book() { Id = 3 } } };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(testIdentityUserId)).Returns(testBorrowingCart);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            var result = service.IsBookInBorrowingCart(testBookId, testIdentityUserId);

            result.Should().BeFalse();
        }


        [Fact]
        public void IsBorrowingCartFull_BorrowingCartNotFound_ShouldThrowNotFoundException()
        {
            string testIdentityUserId = "testId";

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(testIdentityUserId)).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            Action result = () => service.IsBorrowingCartFull(testIdentityUserId);

            result.Should().Throw<NotFoundException>().WithMessage($"No borrowing cart found for UserId: {testIdentityUserId}");
        }

        [Fact]
        public void IsBorrowingCartFull_BorrowingCartIsFull_ShouldReturnTrue()
        {
            string testIdentityUserId = "testId";
            var testBorrowingCart = new BorrowingCart() { Books = new List<Book>() { new Book() { Id = 2 }, new Book() { Id = 3 } } };
            var testLoanSettings = new GlobalLoanSettings() { DurationOfFreeLoanInDays = 21, MaxBooksInOrder = 2, OverduePenaltyRatePerDayForOneBook = 0.2M };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(testIdentityUserId)).Returns(testBorrowingCart);
            mockGlobalLoanSettingsRepo.Setup(r => r.GetSettings()).Returns(testLoanSettings);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            var result = service.IsBorrowingCartFull(testIdentityUserId);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsBorrowingCartFull_BorrowingCartIsNotFull_ShouldReturnFalse()
        {
            string testIdentityUserId = "testId";
            var testBorrowingCart = new BorrowingCart() { Books = new List<Book>() { new Book() { Id = 2 }, new Book() { Id = 3 } } };
            var testLoanSettings = new GlobalLoanSettings() { DurationOfFreeLoanInDays = 21, MaxBooksInOrder = 5, OverduePenaltyRatePerDayForOneBook = 0.2M };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(testIdentityUserId)).Returns(testBorrowingCart);
            mockGlobalLoanSettingsRepo.Setup(r => r.GetSettings()).Returns(testLoanSettings);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            var result = service.IsBorrowingCartFull(testIdentityUserId);


            result.Should().BeFalse();
        }

        [Fact]
        public void UpdateOverduePenaltyAndStatusForAllLoans_OverdueLoanWithBorrowedStatus_ShouldUpdateStatusAndPenalty()
        {
            var testLoan = new Loan
            {
                ReturnDueDate = DateTime.Now.AddDays(-1),
                StatusId = (int)LoanStatusId.Borrowed,
                Books = new List<Book> { new Book(), new Book() }
            };

            var settings = new GlobalLoanSettings
            {
                DurationOfFreeLoanInDays = 21,
                MaxBooksInOrder = 5,
                OverduePenaltyRatePerDayForOneBook = 0.2M
            };

            decimal expectedPenalty = 0.4M;

            mockLoanRepo.Setup(x => x.GetAllLoans()).Returns(new List<Loan> { testLoan }.AsQueryable());
            mockGlobalLoanSettingsRepo.Setup(x => x.GetSettings()).Returns(settings);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            service.UpdateOverduePenaltyAndStatusForAllLoans();

            testLoan.isOverdue.Should().BeTrue();
            testLoan.StatusId.Should().Be((int)LoanStatusId.Overdue);
            testLoan.ReturnRecord.OverduePenalty.Should().Be(expectedPenalty);
            testLoan.ReturnRecord.TotalPenalty.Should().Be(expectedPenalty);
            mockLoanRepo.Verify(x => x.UpdatePenaltyAndStatusInLoans(It.IsAny<List<Loan>>()), Times.Once);
        }

        [Fact]
        public void UpdateOverduePenaltyAndStatusForAllLoans_OverdueLoanWithOverdueStatus_ShouldUpdatePenalty()
        {
            var testLoan = new Loan
            {
                ReturnDueDate = DateTime.Now.AddDays(-2),
                StatusId = (int)LoanStatusId.Overdue,
                Books = new List<Book> { new Book(), new Book() },
                isOverdue = true,
                ReturnRecord = new ReturnRecord {OverduePenalty = 0.4M, TotalPenalty = 0.4M }
            };

            var settings = new GlobalLoanSettings
            {
                DurationOfFreeLoanInDays = 21,
                MaxBooksInOrder = 5,
                OverduePenaltyRatePerDayForOneBook = 0.2M
            };

            decimal expectedPenalty = 0.8M;

            mockLoanRepo.Setup(x => x.GetAllLoans()).Returns(new List<Loan> { testLoan }.AsQueryable());
            mockGlobalLoanSettingsRepo.Setup(x => x.GetSettings()).Returns(settings);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            service.UpdateOverduePenaltyAndStatusForAllLoans();

            testLoan.isOverdue.Should().BeTrue();
            testLoan.StatusId.Should().Be((int)LoanStatusId.Overdue);
            testLoan.ReturnRecord.OverduePenalty.Should().Be(expectedPenalty);
            testLoan.ReturnRecord.TotalPenalty.Should().Be(expectedPenalty);
            mockLoanRepo.Verify(x => x.UpdatePenaltyAndStatusInLoans(It.IsAny<List<Loan>>()), Times.Once);
        }

        [Fact]
        public void UpdateOverduePenaltyAndStatusForAllLoans_BorrowedLoanWithNoOverdue_ShouldDoNothing()
        {
            var testLoan = new Loan
            {
                ReturnDueDate = DateTime.Now.AddDays(7),
                StatusId = (int)LoanStatusId.Borrowed,
                Books = new List<Book> { new Book(), new Book() },
                isOverdue = false,
            };

            var settings = new GlobalLoanSettings
            {
                DurationOfFreeLoanInDays = 21,
                MaxBooksInOrder = 5,
                OverduePenaltyRatePerDayForOneBook = 0.2M
            };

            mockLoanRepo.Setup(x => x.GetAllLoans()).Returns(new List<Loan> { testLoan }.AsQueryable());
            mockGlobalLoanSettingsRepo.Setup(x => x.GetSettings()).Returns(settings);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            service.UpdateOverduePenaltyAndStatusForAllLoans();

            testLoan.isOverdue.Should().BeFalse();
            testLoan.StatusId.Should().Be((int)LoanStatusId.Borrowed);
            testLoan.ReturnRecord.Should().BeNull();
            mockLoanRepo.Verify(x => x.UpdatePenaltyAndStatusInLoans(It.IsAny<List<Loan>>()), Times.Never);
        }

        [Fact]
        public void AddNewLoan_BorrowingCartNotFound_ShouldThrowNotFoundException()
        {
            int testBorrowingCartId = 1;
            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(It.IsAny<int>())).Returns((BorrowingCart)null);

            Action result = () => service.AddNewLoan(testBorrowingCartId, 1);

            result.Should().Throw<NotFoundException>().WithMessage($"No borrowing cart found for Id: {testBorrowingCartId}");
        }

        [Fact]
        public void AddNewLoan_NoAvailableBooksForBorrow_ShouldReturnMinusOne()
        {
            int testBorrowingCartId = 1;
            var testBorrowingCart = new BorrowingCart
            {
                Id = testBorrowingCartId,
                Books = new List<Book> { new Book { Quantity = 0 } }
            };

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns(testBorrowingCart);

            var result = service.AddNewLoan(testBorrowingCartId, 1);

            result.Should().Be(-1);
        }

        [Fact]
        public void AddNewLoan_AvailableBooks_ShouldReturnLoanIdAndClearBorrowingCartAndDecrementQuantityOfBorrowedBook()
        {
            int expectedLoanId = 2;
            var testBook = new Book { Quantity = 1 };
            int testBorrowingCartId = 1;
            var testBorrowingCart = new BorrowingCart
            {
                Id = testBorrowingCartId,
                Books = new List<Book> { new Book { Quantity = 1 } }
            };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns(testBorrowingCart);
            mockLoanRepo.Setup(r => r.AddLoan(It.IsAny<Loan>())).Returns(expectedLoanId);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            var result = service.AddNewLoan(testBorrowingCartId, 1);

            result.Should().Be(expectedLoanId);
            mockLoanRepo.Verify(r => r.AddLoan(It.IsAny<Loan>()), Times.Once);
            mockBookRepo.Verify(r => r.UpdateBooksQuantity(It.IsAny<ICollection<Book>>()), Times.Once);
            mockBorrowingCartRepo.Verify(r => r.RemoveAllFromBorrowingCart(testBorrowingCart), Times.Once);
        }

        [Fact]
        public void GetAllLoansForListByIndentityUserId_ValidArguments_ShouldReturnLoanList()
        {
            var mapper = GetMapper();
            string testUserId = "userId";
            int pageSize = 2;
            int pageNumber = 1;

            var loans = new List<Loan>
            {
                new Loan { Id = 1, CreationDate = DateTime.Now.AddDays(-1), LibraryUser = new LibraryUser { IdentityUserId = testUserId }, Status = new Status { Name = "Borrowed"}, Books = new List<Book>() },
                new Loan { Id = 2, CreationDate = DateTime.Now.AddDays(-2), LibraryUser = new LibraryUser { IdentityUserId = testUserId }, Status = new Status { Name = "Borrowed"}, Books = new List<Book>() },
                new Loan { Id = 3, CreationDate = DateTime.Now.AddDays(-3), LibraryUser = new LibraryUser { IdentityUserId = testUserId }, Status = new Status { Name = "Borrowed"}, Books = new List<Book>() },
                new Loan { Id = 4, CreationDate = DateTime.Now.AddDays(-3), LibraryUser = new LibraryUser { IdentityUserId = "test" }, Status = new Status { Name = "Borrowed"}, Books = new List<Book>() },
            };

            mockLoanRepo.Setup(repo => repo.GetAllLoans()).Returns(loans.AsQueryable());

            var service = new LoanService(mapper, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);
            
            var result = service.GetAllLoansForListByIndentityUserId(testUserId, pageSize, pageNumber);

            result.PageSize.Should().Be(pageSize);
            result.CurrentPage.Should().Be(pageNumber);
            result.Count.Should().Be(3);
            result.Loans.Count.Should().Be(pageSize);
            result.Loans.Should().Contain(l => l.Id == 1);
            result.Loans.Should().Contain(l => l.Id == 2);
        }

        [Fact]
        public void GetAllLoansForListByIndentityUserId_NegativePageSize_ShouldThrowException()
        {
            string testUserId = "userId";
            int pageSize = -2;
            int pageNumber = 1;

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            Action result = () => service.GetAllLoansForListByIndentityUserId(testUserId, pageSize, pageNumber);

            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetAllLoansForListByIndentityUserId_NegativePageNumber_ShouldThrowExcepction()
        {
            string testUserId = "userId";
            int pageSize = 2;
            int pageNumber = -1;

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            Action result = () => service.GetAllLoansForListByIndentityUserId(testUserId, pageSize, pageNumber);

            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetLoanForDetails_LoanFound_ShouldReturnLoanVm()
        {
            int loanId = 1;
            var expectedVm = new LoanDetailsVm() { Id = loanId };

            mockLoanRepo.Setup(r => r.GetLoanById(loanId)).Returns(new Loan() { Id = loanId});
            mockMapper.Setup(m => m.Map<LoanDetailsVm>(It.IsAny<Loan>())).Returns(expectedVm);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            var result = service.GetLoanForDetails(loanId);

            result.Should().BeEquivalentTo(expectedVm);
        }

        [Fact]
        public void GetLoanForDetails_LoanNotFound_ShouldReturnNull()
        {
            int loanId = 1;
            var expectedVm = new LoanDetailsVm() { Id = loanId };

            mockLoanRepo.Setup(r => r.GetLoanById(loanId)).Returns((Loan)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            var result = service.GetLoanForDetails(loanId);

            result.Should().BeNull();
        }

        [Fact]
        public void CancelLoan_LoanNotFound_ShouldThrowNotFoundException()
        {
            int loanId = 1;

            mockLoanRepo.Setup(r => r.GetLoanById(loanId)).Returns((Loan)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            Action result = () => service.CancelLoan(loanId);

            result.Should().Throw<NotFoundException>().WithMessage($"No loan found for Id: {loanId}");
        }

        [Theory]
        [InlineData((int)LoanStatusId.Borrowed)]
        [InlineData((int)LoanStatusId.Completed)]
        [InlineData((int)LoanStatusId.Overdue)]
        [InlineData((int)LoanStatusId.Cancelled)]
        public void CancelLoan_LoanCannotBeCancelled_ShouldReturnFalse(int loanStatus)
        {
            int loanId = 1;
            Loan testLoan = new Loan() {Id = loanId, StatusId = loanStatus };

            mockLoanRepo.Setup(r => r.GetLoanById(loanId)).Returns(testLoan);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            var result = service.CancelLoan(loanId);

            result.Should().Be(false);
        }

        [Fact]
        public void CancelLoan_LoanCanBeCancelled_ShouldCancelLoanAndUpdateBooksQuantityAndReturnTrue()
        {
            int loanId = 1;
            Loan testLoan = new Loan() { Id = loanId, StatusId = (int)LoanStatusId.New, Books = new List<Book>() };

            mockLoanRepo.Setup(r => r.GetLoanById(loanId)).Returns(testLoan);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            var result = service.CancelLoan(loanId);

            result.Should().Be(true);
            testLoan.StatusId = (int)LoanStatusId.Cancelled;
            mockLoanRepo.Verify(r => r.UpdateLoan(testLoan), Times.Once);
            mockBookRepo.Verify(r => r.UpdateBooksQuantity(It.IsAny<ICollection<Book>>()), Times.Once);
        }

        [Fact]
        public void GetAllLoansForConfirmCheckOutList_WhenCalled_ShouldReturnLoanListVm()
        {
            var mapper = GetMapper();
            var testLoanList = new List<Loan>()
            {
                new Loan { Id = 1, CreationDate = new DateTime(2023, 9, 1), Status = new Status {Id = (int)LoanStatusId.New, Name = "New"}, Books = new List<Book>(), LibraryUser = new LibraryUser() },
                new Loan { Id = 4, CreationDate = new DateTime(2023, 9, 2), Status = new Status {Id = (int)LoanStatusId.New, Name = "New"}, Books = new List<Book>(), LibraryUser = new LibraryUser() },
            };

            mockLoanRepo.Setup(r => r.GetAllLoans()).Returns(testLoanList.AsQueryable());

            var service = new LoanService(mapper, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            var result = service.GetAllLoansForConfirmCheckOutList();

            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(2);

            var firstLoanVm = result.FirstOrDefault(l => l.Id == 1);
            var secondLoanVm = result.FirstOrDefault(l => l.Id == 4);

            firstLoanVm.Should().NotBeNull();
            firstLoanVm.Status.Should().Be("New");
            firstLoanVm.CreationDate.Should().Be(new DateTime(2023, 9, 1));

            secondLoanVm.Should().NotBeNull();
            secondLoanVm.Status.Should().Be("New");
            secondLoanVm.CreationDate.Should().Be(new DateTime(2023, 9, 2));
        }

        [Fact]
        public void GetAllLoansForConfirmCheckOutList_NoLoans_ShouldReturnEmptyList()
        {
            var mapper = GetMapper();
            mockLoanRepo.Setup(r => r.GetAllLoans()).Returns(new List<Loan>().AsQueryable());

            var service = new LoanService(mapper, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object, mockGlobalLoanSettingsRepo.Object);

            var result = service.GetAllLoansForConfirmCheckOutList();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
