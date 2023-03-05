using LibraryMVC.Application.ViewModels.BorrowingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Interfaces
{
    public interface ILoanService
    {
        BorrowingCartDetailsVm GetBorrowingCart(string identityUserId);
        bool AddToBorrowingCart(int bookId,string identityUserId);
        void RemoveFromBorrowingCart(int bookId, int borrowingCartId);
        int AddNewLoan(int borrowingCartId, int userId);

    }
}
