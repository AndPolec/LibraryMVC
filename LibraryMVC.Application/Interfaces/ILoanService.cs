using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Application.ViewModels.Loan;
using LibraryMVC.Application.ViewModels.ReturnRecord;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Interfaces
{
    public interface ILoanService
    {
        BorrowingCartDetailsVm GetBorrowingCartForDetailsByIndentityUserId(string identityUserId);
        BorrowingCart GetBorrowingCartById(int borrowingCartId);
        bool AddToBorrowingCart(int bookId,string identityUserId);
        void RemoveFromBorrowingCart(int bookId, int borrowingCartId);
        void ClearBorrowingCart(int borrowingCartId);
        int AddNewLoan(int borrowingCartId, int userId);
        ListOfLoanForListVm GetAllLoansForListByIndentityUserId(string userId, int pageSize, int pageNumber);
        List<LoanForConfirmCheckOutListVm> GetAllLoansForConfirmCheckOutList();
        List<LoanForConfirmReturnListVm> GetAllLoansForConfirmReturnList();
        LoanDetailsVm GetLoanForDetails(int loanId);
        bool CancelLoan(int loanId);
        int ConfirmCheckOut(int loanId, string librarianIdentityUserId);
        NewReturnRecordVm GetInfoForConfirmReturn(int loanId);
        NewReturnRecordVm SetParametersToVm(NewReturnRecordVm model);
        int ConfirmReturn(NewReturnRecordVm model, string librarianIdentityUserId);
        ReturnRecordDetailsVm GetReturnRecordForDetails(int returnRecordId);


    }
}
