using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Application.ViewModels.Loan;
using LibraryMVC.Application.ViewModels.ReturnRecord;
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
        private decimal _penaltyRatePerDayForOneBook = 0.2M;
        private int _durationOfFreeLoanInDays = 21;
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

        private decimal CalculatePenalty(Loan loan)
        {
            int numberOfBookHoldingDays = (DateTime.Now - loan.ReturnDueDate).Days;
            int numberOfBooks = loan.Books.Count();
            decimal penalty = numberOfBookHoldingDays * numberOfBooks * _penaltyRatePerDayForOneBook;
            return penalty;
        } 

        private void UpdatePenaltyForHoldingBooks(List<Loan> loans)
        {
            var loansToUpdate = new List<Loan>();

            foreach (var loan in loans)
            {
                if (loan.StatusId == 2 && (DateTime.Now.Date > loan.ReturnDueDate.Date)) // if Status == "Wypożyczone"
                {
                    decimal calculatedPenalty = CalculatePenalty(loan);
                    loan.Penalty = calculatedPenalty;
                    loan.StatusId = 4;
                    loansToUpdate.Add(loan);
                }
                else if (loan.StatusId == 4) // if Status == "Zaległe"
                {
                    decimal calculatedPenalty = CalculatePenalty(loan);
                    if (loan.Penalty != calculatedPenalty)
                    {
                        loan.Penalty = calculatedPenalty;
                        loansToUpdate.Add(loan);
                    }
                }
            }

            if (loansToUpdate.Count != 0)
                _loanRepository.UpdatePenaltyAndStatusInLoans(loansToUpdate);
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
                .ToList();

            UpdatePenaltyForHoldingBooks(loans);

            var loansVm = _mapper.Map<List<LoanForListVm>>(loans);
            var loansToDisplay = loansVm.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            var loanList = new ListOfLoanForListVm()
            {
                Loans = loansToDisplay,
                Count = loansToDisplay.Count(),
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
            loan.StatusId = 2; //Status = "Wypożyczone"
            loan.ReturnDueDate = DateTime.Now.AddDays(_durationOfFreeLoanInDays);
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

        public List<LoanForConfirmReturnListVm> GetAllLoansForConfirmReturnList()
        {
            var loans = _loanRepository.GetAllLoans()
                .Where(l => l.Status.Id == 2 || l.Status.Id == 4).ToList(); //Staus = "Wypożyczone" || "Zaległe"

            UpdatePenaltyForHoldingBooks(loans);

            var loansVm = _mapper.Map<List<LoanForConfirmReturnListVm>>(loans);
            return loansVm;
        }

        public NewReturnRecordVm GetInfoForConfirmReturn(int loanId)
        {
            var loan = _loanRepository.GetLoanById(loanId);
            var returnRecordVm = _mapper.Map<NewReturnRecordVm>(loan);
            return returnRecordVm;
        }

        public NewReturnRecordVm SetParametersToVm(NewReturnRecordVm model)
        {
            var books = _bookRepository.GetAllBooks()
                .Where(b => b.Loans.Any(l => l.Id == model.LoanId))
                .ProjectTo<BookForListVm>(_mapper.ConfigurationProvider)
                .ToList();
            model.BorrowedBooks = books;
            return model;
        }

        public int ConfirmReturn(NewReturnRecordVm model, string librarianIdentityUserId)
        {
            var returnRecord = _mapper.Map<ReturnRecord>(model);
            returnRecord.AdditionalLibrarianInfoId = _additionalLibrarianInfoRepository.GetInfoByIdentityUserId(librarianIdentityUserId).Id;
            returnRecord.ReturnedBooks = new List<Book>();
            returnRecord.LostOrDestroyedBooks = new List<Book>();
            foreach (var i in model.ReturnedBooksId)
            {
                returnRecord.ReturnedBooks.Add(_bookRepository.GetBookById(i));
            }
            foreach (var i in model.LostOrDestroyedBooksId)
            {
                returnRecord.LostOrDestroyedBooks.Add(_bookRepository.GetBookById(i));
            }

            var loan = _loanRepository.GetLoanById(model.LoanId);
            loan.StatusId = 3; //Status = "Zakończone"
            loan.ReturnRecord = returnRecord;
            _loanRepository.UpdateLoan(loan);

            IncrementQuantityOfAvailableBooks(returnRecord.ReturnedBooks);
            return returnRecord.Id;
        }

    }
}
