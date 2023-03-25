using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Application.ViewModels.Loan;
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
        private readonly IBookRepository _bookRepository;
        private readonly IAdditionalLibrarianInfoRepository _additionalLibrarianInfoRepository;

        
        public LoanService(IMapper mapper,IBorrowingCartRepository borrowingCartRepository, ILoanRepository loanRepository, IBookRepository bookRepository, IAdditionalLibrarianInfoRepository additionalLibrarianInfoRepository)
        {
            _borrowingCartRepository = borrowingCartRepository;
            _mapper = mapper;
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _additionalLibrarianInfoRepository = additionalLibrarianInfoRepository;
        }

        //BorrowingCart
        public bool AddToBorrowingCart(int bookId, string identityUserId)
        {
            if (isBorrowingCartFull(identityUserId))
                return false;

            _borrowingCartRepository.AddToBorrowingCart(bookId, identityUserId);
            return true;
        }

        public BorrowingCartDetailsVm GetBorrowingCartForDetailsByIndentityUserId(string identityUserId)
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

        public void ClearBorrowingCart(int borrowingCartId)
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
        private void DecrementQuantityOfAvailableBooks(ICollection<Book> books)
        {
            foreach(var book in books)
            {
                book.Quantity -= 1;
            }

            _bookRepository.UpdateBooksQuantity(books);
        }
        private void IncrementQuantityOfAvailableBooks(ICollection<Book> books)
        {
            foreach (var book in books)
            {
                book.Quantity += 1;
            }

            _bookRepository.UpdateBooksQuantity(books);
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
                StatusId = 1
            };
            var loanId = _loanRepository.AddLoan(loan);

            DecrementQuantityOfAvailableBooks(availableBooks);
            ClearBorrowingCart(borrowingCartId);

            return loanId;
        }
        public ListOfLoanForListVm GetAllLoansForListByIndentityUserId(string userId, int pageSize, int pageNumber)
        {
            var loans = _loanRepository.GetAllLoans()
                .Where(l => l.LibraryUser.IdentityUserId == userId)
                .ProjectTo<LoanForListVm>(_mapper.ConfigurationProvider)
                .ToList();

            var loansToShow = loans.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            var loanList = new ListOfLoanForListVm()
            {
                Loans = loansToShow,
                Count = loansToShow.Count(),
                PageSize = pageSize,
                CurrentPage = pageNumber
            };

            return loanList;
        }

        public LoanDetailsVm GetLoanForDetails(int loanId)
        {
            var loan = _loanRepository.GetLoanById(loanId);
            var loanVm = _mapper.Map<LoanDetailsVm>(loan);
            return loanVm;
        }

        public bool CancelLoan(int loanId)
        {
            var loan = _loanRepository.GetLoanById(loanId);

            if (loan.StatusId != 1)
                return false;

            loan.StatusId = 5;
            _loanRepository.UpdateLoan(loan);
            IncrementQuantityOfAvailableBooks(loan.Books);
            return true;
        }

        public List<LoanForConfirmCheckOutListVm> GetAllLoansForConfirmCheckOutList()
        {
            var loans = _loanRepository.GetAllLoans()
                .Where(l => l.Status.Id == 1)
                .ProjectTo<LoanForConfirmCheckOutListVm>(_mapper.ConfigurationProvider)
                .ToList();

            return loans;
        }

        public int ConfirmCheckOut(int loanId, string librarianIdentityUserId)
        {
            var librarianInfoId = _additionalLibrarianInfoRepository.GetInfoByIdentityUserId(librarianIdentityUserId).Id;
            var loan = _loanRepository.GetLoanById(loanId);
            loan.StatusId = 2;
            loan.CheckOutRecord = new CheckOutRecord()
            {
                Id = 0,
                Date = DateTime.Now,
                LoanId = loanId,
                AdditionalLibrarianInfoId = librarianInfoId
            };

            _loanRepository.UpdateLoan(loan);
            return loan.CheckOutRecord.Id;
        }
    }
}
