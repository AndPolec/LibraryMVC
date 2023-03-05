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

        //BorrowingCart
        public LoanService(IMapper mapper,IBorrowingCartRepository borrowingCartRepository)
        {
            _borrowingCartRepository = borrowingCartRepository;
            _mapper = mapper;
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

        private bool isBorrowingCartFull(string identityUserId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartByIndentityUserId(identityUserId);
            var count = borrowingCart.Books.Count();
            
            if (count < 5)
                return false;

            return true;
        }

        //Loan
        public int AddNewLoan(int borrowingCartId, int userId)
        {
            var borrowingCart = GetBorrowingCartById(borrowingCartId);
            var loan = new Loan()
            {
                LibraryUserId = userId,
                Books = borrowingCart.Books,
                LoanCreationDate = DateTime.Now,
                CheckOutDueDate = DateTime.Now.AddDays(7)
            }; 


            return 0;
        }
    }
}
