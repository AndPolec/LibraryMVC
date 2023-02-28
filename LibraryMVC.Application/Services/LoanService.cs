using AutoMapper;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Domain.Interfaces;
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

        public BorrowingCartDetailsVm GetBorrowingCart(string identityUserId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartByIndentityUserId(identityUserId);
            var borrowingCartVm = _mapper.Map<BorrowingCartDetailsVm>(borrowingCart);
            return borrowingCartVm;
        }

        private bool isBorrowingCartFull(string identityUserId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartByIndentityUserId(identityUserId);
            var count = borrowingCart.Books.Count();
            
            if (count < 5)
                return false;

            return true;
        }
    }
}
