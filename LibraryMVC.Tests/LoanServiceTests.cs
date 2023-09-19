﻿using AutoMapper;
using FluentAssertions;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.BorrowingCart;
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

namespace LibraryMVC.Tests
{
    public class LoanServiceTests
    {
        [Fact]
        public void AddToBorrowingCart_ValidInput_ShouldUpdateBorrowingCart()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();

            var testBorrowingCart = new BorrowingCart { Books = new List<Book>() };
            var testBook = new Book();

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(It.IsAny<string>())).Returns(testBorrowingCart);
            mockBookRepo.Setup(r => r.GetBookById(It.IsAny<int>())).Returns(testBook);

            var service = new LoanService(mockMapper.Object,mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);
            
            service.AddToBorrowingCart(1, "userId");


            testBorrowingCart.Books.Should().Contain(testBook);
            mockBorrowingCartRepo.Verify(r => r.UpdateBorrowingCart(testBorrowingCart), Times.Once);
        }

        [Fact]
        public void AddToBorrowingCart_BorrowingCartNotFound_ShouldThrowKeyNotFoundException()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();
            string testUserId = "id";

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(It.IsAny<string>())).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);

            Action result = () => service.AddToBorrowingCart(1, testUserId);

            result.Should().Throw<KeyNotFoundException>().WithMessage($"No borrowing cart found for user with ID: {testUserId}");
        }

        [Fact]
        public void AddToBorrowingCart_BookNotFound_ShouldThrowKeyNotFoundException()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();
            int testBookId = 1;

            var testBorrowingCart = new BorrowingCart { Books = new List<Book>() };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(It.IsAny<string>())).Returns(testBorrowingCart);
            mockBookRepo.Setup(r => r.GetBookById(testBookId)).Returns((Book)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);

            Action result = () => service.AddToBorrowingCart(testBookId, "userId");

            result.Should().Throw<KeyNotFoundException>().WithMessage($"No book found with ID: {testBookId}");
        }

        [Fact]
        public void GetBorrowingCartForDetailsByIndentityUserId_ValidUserId_ShouldReturnBorrowingCartModel()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();
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

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);

            var result = service.GetBorrowingCartForDetailsByIndentityUserId(testUserId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedBorrowingCartDetailsVm);
        }

        [Fact]
        public void GetBorrowingCartForDetailsByIndentityUserId_BorrowingCartNotFound_ShouldReturnNull()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();
            string testUserId = "id";

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartByIdentityUserId(testUserId)).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);

            var result = service.GetBorrowingCartForDetailsByIndentityUserId(testUserId);

            result.Should().BeNull();
        }

        [Fact]
        public void RemoveFromBorrowingCart_BorrowingCartNotFound_ShouldThrowKeyNotFoundException()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();
            int testBorrowingCartId = 1;
            int testBookId = 2;

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);

            Action result = () => service.RemoveFromBorrowingCart(testBookId,testBorrowingCartId);

            result.Should().Throw<KeyNotFoundException>().WithMessage($"No borrowing cart found for ID: {testBorrowingCartId}");
        }

        [Fact]
        public void RemoveFromBorrowingCart_BookNotFoundInBorrowingCart_ShouldThrowKeyNotFoundException()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();
            int testBorrowingCartId = 1;
            int testBookId = 2;

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns(new BorrowingCart() { Id = testBorrowingCartId, Books = new List<Book>() });

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);

            Action result = () => service.RemoveFromBorrowingCart(testBookId, testBorrowingCartId);

            result.Should().Throw<KeyNotFoundException>().WithMessage($"No book found for ID: {testBookId} in borrowing cart with ID: {testBorrowingCartId}");
        }

        [Fact]
        public void RemoveFromBorrowingCart_ValidInput_ShouldRemoveBookAndCallUpdate()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();
            int testBorrowingCartId = 1;
            int testBookId = 2;
            var testBorrowingCart = new BorrowingCart() { Id = testBorrowingCartId, Books = new List<Book>() { new Book() { Id = testBookId }, new Book() { Id = 3 } } };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns(testBorrowingCart);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);

            service.RemoveFromBorrowingCart(testBookId,testBorrowingCartId);

            testBorrowingCart.Books.Should().HaveCount(1);
            testBorrowingCart.Books.Should().NotContain(b => b.Id == testBookId);
            mockBorrowingCartRepo.Verify(r => r.UpdateBorrowingCart(testBorrowingCart), Times.Once());
        }

        [Fact]
        public void ClearBorrowingCart_BorrowingCartNotFound_ShouldReturnFalse()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();
            int testBorrowingCartId = 1;

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns((BorrowingCart)null);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);

            var result = service.ClearBorrowingCart(testBorrowingCartId);

            result.Should().BeFalse();
        }

        [Fact]
        public void ClearBorrowingCart_BorrowingCartFound_ShouldCallRemoveMethodAndReturnTrue()
        {
            var mockBorrowingCartRepo = new Mock<IBorrowingCartRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockBookRepo = new Mock<IBookRepository>();
            var mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
            var mockReturnRecordRepo = new Mock<IReturnRecordRepository>();
            var mockMapper = new Mock<IMapper>();
            int testBorrowingCartId = 1;
            var testBorrowingCart = new BorrowingCart() { Id = testBorrowingCartId, Books = new List<Book>() { new Book() { Id = 2 }, new Book() { Id = 3 } } };

            mockBorrowingCartRepo.Setup(r => r.GetBorrowingCartById(testBorrowingCartId)).Returns(testBorrowingCart);

            var service = new LoanService(mockMapper.Object, mockBorrowingCartRepo.Object, mockLoanRepo.Object, mockBookRepo.Object, mockAdditionalLibrarianInfoRepo.Object, mockReturnRecordRepo.Object);

            var result = service.ClearBorrowingCart(testBorrowingCartId);

            result.Should().BeTrue();
            mockBorrowingCartRepo.Verify(r => r.RemoveAllFromBorrowingCart(testBorrowingCart),Times.Once());
        }

    }
}
