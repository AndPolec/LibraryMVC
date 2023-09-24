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
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using LibraryMVC.Application.Exceptions;

namespace LibraryMVC.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly IMapper _mapper;
        private readonly IBorrowingCartRepository _borrowingCartRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IAdditionalLibrarianInfoRepository _additionalLibrarianInfoRepository;
        private readonly IReturnRecordRepository _returnRecordRepository;
        private readonly IGlobalLoanSettingsRepository _loanSettingsRepository;
        private enum LoanStatusId
        {
            New = 1,
            Borrowed = 2,
            Completed = 3,
            Overdue = 4,
            Cancelled = 5
        }


        public LoanService(IMapper mapper,IBorrowingCartRepository borrowingCartRepository, ILoanRepository loanRepository, IBookRepository bookRepository, IAdditionalLibrarianInfoRepository additionalLibrarianInfoRepository, IReturnRecordRepository returnRecordRepository, IGlobalLoanSettingsRepository loanSettingsRepository)
        {
            _borrowingCartRepository = borrowingCartRepository;
            _mapper = mapper;
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _additionalLibrarianInfoRepository = additionalLibrarianInfoRepository;
            _returnRecordRepository = returnRecordRepository;
            _loanSettingsRepository = loanSettingsRepository;
        }

        //BorrowingCart
        public void AddToBorrowingCart(int bookId, string identityUserId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartByIdentityUserId(identityUserId);
            var book = _bookRepository.GetBookById(bookId);

            if (borrowingCart == null)
            {
                throw new NotFoundException($"No borrowing cart found for user with ID: {identityUserId}");
            }

            if (book == null)
            {
                throw new NotFoundException($"No book found with ID: {bookId}");
            }

            borrowingCart.Books.Add(book);
            _borrowingCartRepository.UpdateBorrowingCart(borrowingCart);
        }


        public BorrowingCartDetailsVm? GetBorrowingCartForDetailsByIndentityUserId(string identityUserId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartByIdentityUserId(identityUserId);
            if (borrowingCart == null)
            {
                return null;
            }

            var borrowingCartVm = _mapper.Map<BorrowingCartDetailsVm>(borrowingCart);
            return borrowingCartVm;
        }

        private BorrowingCart GetBorrowingCartById(int borrowingCartId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartById(borrowingCartId);
            return borrowingCart;
        }

        public void RemoveFromBorrowingCart(int bookId, int borrowingCartId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartById(borrowingCartId);
            if (borrowingCart == null)
            {
                throw new NotFoundException($"No borrowing cart found for ID: {borrowingCartId}");
            }

            var bookToRemove = borrowingCart.Books.FirstOrDefault(b => b.Id == bookId);
            if (bookToRemove == null)
            {
                throw new NotFoundException($"No book found for ID: {bookId} in borrowing cart with ID: {borrowingCartId}");
            }

            borrowingCart.Books.Remove(bookToRemove);
            _borrowingCartRepository.UpdateBorrowingCart(borrowingCart);
        }

        public bool ClearBorrowingCart(int borrowingCartId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartById(borrowingCartId);
            if (borrowingCart != null)
            {
                _borrowingCartRepository.RemoveAllFromBorrowingCart(borrowingCart);
                return true;
            }
            return false;
        }

        public bool IsBookInBorrowingCart(int bookId, string identityUserId)
        {
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartByIdentityUserId(identityUserId);
            if (borrowingCart == null)
            {
                throw new NotFoundException($"No borrowing cart found for UserId: {identityUserId}");
            }
            return borrowingCart.Books.Any(b => b.Id == bookId);
        }

        public bool IsBorrowingCartFull(string identityUserId)
        {
            
            var borrowingCart = _borrowingCartRepository.GetBorrowingCartByIdentityUserId(identityUserId);
            if (borrowingCart == null)
            {
                throw new NotFoundException($"No borrowing cart found for UserId: {identityUserId}");
            }

            var count = borrowingCart.Books.Count();
            int maxBooksInOrder = _loanSettingsRepository.GetSettings().MaxBooksInOrder;

            if (count < maxBooksInOrder)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Loan
        private ICollection<Book> FilterAndReturnAvailableBooks(ICollection<Book> books)
        {
            var availableBooks = new List<Book>();
            foreach (var book in books)
            {
                if (book.Quantity > 0)
                {
                    availableBooks.Add(book);
                }
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

        private decimal CalculateOverduePenalty(Loan loan)
        {
            decimal overduePenaltyRatePerDayForOneBook = _loanSettingsRepository.GetSettings().OverduePenaltyRatePerDayForOneBook;
            int numberOfOverdueDays = (DateTime.Now - loan.ReturnDueDate).Days;
            int numberOfBooks = loan.Books.Count();
            decimal penalty = numberOfOverdueDays * numberOfBooks * overduePenaltyRatePerDayForOneBook;
            return penalty;
        }

        private void UpdatePenalty(Loan loan)
        {
            decimal calculatedPenalty = CalculateOverduePenalty(loan);
            loan.ReturnRecord.OverduePenalty = calculatedPenalty;
            loan.ReturnRecord.TotalPenalty = calculatedPenalty;
        }

        public void UpdateOverduePenaltyAndStatusForAllLoans()
        {
            var loans = _loanRepository.GetAllLoans().Where(l => l.StatusId == (int)LoanStatusId.Borrowed || l.StatusId == (int)LoanStatusId.Overdue).ToList(); 
            var loansToUpdate = new List<Loan>();

            foreach (var loan in loans)
            {
                if (loan.StatusId == (int)LoanStatusId.Overdue)
                {
                    UpdatePenalty(loan);
                    loansToUpdate.Add(loan);
                }
                else if (loan.StatusId == (int)LoanStatusId.Borrowed && (DateTime.Now.Date > loan.ReturnDueDate.Date))
                {
                    if (loan.ReturnRecord == null)
                    {
                        loan.ReturnRecord = new ReturnRecord() { Comments = "" };
                    }

                    UpdatePenalty(loan);
                    loan.isOverdue = true;
                    loan.StatusId = (int)LoanStatusId.Overdue;
                    loansToUpdate.Add(loan);
                }
            }

            if (loansToUpdate.Count != 0)
                _loanRepository.UpdatePenaltyAndStatusInLoans(loansToUpdate);
        }

        public int AddNewLoan(int borrowingCartId, int userId)
        {
            var borrowingCart = GetBorrowingCartById(borrowingCartId);
            var availableBooks = FilterAndReturnAvailableBooks(borrowingCart.Books);
            if (availableBooks.Any())
            {
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
            
            ClearBorrowingCart(borrowingCartId);
            return -1;
        }

        public ListOfLoanForListVm GetAllLoansForListByIndentityUserId(string userId, int pageSize, int pageNumber)
        {
            var loansVm = _loanRepository.GetAllLoans()
                .Where(l => l.LibraryUser.IdentityUserId == userId)
                .OrderByDescending(l => l.CreationDate)
                .ProjectTo<LoanForListVm>(_mapper.ConfigurationProvider)
                .ToList();

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
                .OrderBy(l => l.Id)
                .ToList();

            return loans;
        }

        public int ConfirmCheckOut(int loanId, string librarianIdentityUserId)
        {
            var librarianInfoId = _additionalLibrarianInfoRepository.GetInfoByIdentityUserId(librarianIdentityUserId).Id;
            var loan = _loanRepository.GetLoanById(loanId);

            if (loan is null)
                return -1;
            int durationOfFreeLoanInDays = _loanSettingsRepository.GetSettings().DurationOfFreeLoanInDays;
            loan.StatusId = 2; //Status = "Wypożyczone"
            loan.ReturnDueDate = DateTime.Now.AddDays(durationOfFreeLoanInDays);
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
            var loansVm = _loanRepository.GetAllLoans()
                .Where(l => l.StatusId == 2 || l.StatusId == 4) //Staus = "Wypożyczone" || "Zaległe"
                .OrderBy(l => l.Id)
                .ToList();
            var loansToReturn = _mapper.Map<List<Loan>,List<LoanForConfirmReturnListVm>>(loansVm);

            return loansToReturn;
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
            var loan = _loanRepository.GetLoanById(model.LoanId);
            if (loan is null)
            {
                return -1;
            }

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

            loan.StatusId = 3; //Status = "Zakończone"
            loan.ReturnRecord = returnRecord;
            _loanRepository.UpdateLoan(loan);

            IncrementQuantityOfAvailableBooks(returnRecord.ReturnedBooks);
            return returnRecord.Id;
        }

        public ReturnRecordDetailsVm GetReturnRecordForDetails(int returnRecordId)
        {
            var returnRecord = _returnRecordRepository.GetReturnRecordById(returnRecordId);
            var model = _mapper.Map<ReturnRecordDetailsVm>(returnRecord);
            return model;
        }

        public LoanSettingsVm GetGlobalLoanSettings()
        {
            var settings = _loanSettingsRepository.GetSettings();
            var settingsVm = new LoanSettingsVm()
            {
                penaltyRatePerDay = settings.OverduePenaltyRatePerDayForOneBook,
                durationOfFreeLoan = settings.DurationOfFreeLoanInDays,
                maxBooksInOrder = settings.MaxBooksInOrder
            };
            return settingsVm;
        }

        public void SetGlobalLoanSettings(LoanSettingsVm model)
        {
            var settings = _loanSettingsRepository.GetSettings();
            settings.OverduePenaltyRatePerDayForOneBook = model.penaltyRatePerDay;
            settings.DurationOfFreeLoanInDays = model.durationOfFreeLoan;
            settings.MaxBooksInOrder = model.maxBooksInOrder;
            _loanSettingsRepository.UpdateSettings(settings);
        }

    }
}
