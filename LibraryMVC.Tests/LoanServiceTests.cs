using AutoMapper;
using FluentAssertions;
using FluentAssertions.Common;
using LibraryMVC.Application.Helpers;
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
        public void AddNewLoan_AvailableBooks_ShouldReturnLoanId()
        {
            
        }
    }
}
