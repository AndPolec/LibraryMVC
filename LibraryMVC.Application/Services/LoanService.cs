using AutoMapper;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly IMapper _mapper;
        private readonly IBorrowingCartRepository _borrowingCartRepository;
        private readonly ILoanRepository _loanRepository;

        //BorrowingCart
        public LoanService(IMapper mapper,IBorrowingCartRepository borrowingCartRepository, ILoanRepository loanRepository)
        {
            _borrowingCartRepository = borrowingCartRepository;
            _mapper = mapper;
            _loanRepository = loanRepository;
        }

        public bool AddToBorrowingCart(int bookId, string identityUserId)
        {
            if (isBorrowingCartFull(identityUserId))
                return false;

            _borrowingCartRepository.AddToBorrowingCart(bookId, identityUserId);
            return true;
        }

        public BorrowingCartDetailsVm GetBorrowingCartByIndentityUserId(string identityUserId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartByIndentityUserId(identityUserId);
            var borrowingCartVm = _mapper.Map<BorrowingCartDetailsVm>(borrowingCart);
            return borrowingCartVm;
        }
        public BorrowingCart GetBorrowingCartById(int borrowingCartId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartById(borrowingCartId);
            return borrowingCart;
        }

        public void RemoveFromBorrowingCart(int bookId, int borrowingCartId)
        {
            _borrowingCartRepository.RemoveFromBorrowingCart(bookId, borrowingCartId);
        }

        public int ClearBorrowingCart(int borrowingCartId)
        {
            _borrowingCartRepository.RemoveAllFromBorrowingCart(borrowingCartId);
        }

        private bool isBorrowingCartFull(string identityUserId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartByIndentityUserId(identityUserId);
            var count = borrowingCart.Books.Count();
            
            if (count < 5)
                return false;

            return true;
        }

        //Loan
        private ICollection<Book> FilterAndReturnAvailableBooks(ICollection<Book> books)
        {
            var availableBooks = new List<Book>();
            foreach (var book in books)
            {
                if (book.Quantity > 0)
                    availableBooks.Add(book);
            }
            return availableBooks;
        }
        public int AddNewLoan(int borrowingCartId, int userId)
        {
            var borrowingCart = GetBorrowingCartById(borrowingCartId);
            var availableBooks = FilterAndReturnAvailableBooks(borrowingCart.Books);
            

            var loan = new Loan()
            {
                LibraryUserId = userId,
                Books = availableBooks,
                CreationDate = DateTime.Now,
            };

            var loanId = _loanRepository.AddLoan(loan);

            ClearBorrowingCart(borrowingCartId);
            return loanId;
        }
    }
}
