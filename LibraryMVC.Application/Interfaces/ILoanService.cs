using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Interfaces
{
    public interface ILoanService
    {
        BorrowingCartDetailsVm GetBorrowingCartByIndentityUserId(string identityUserId);
        BorrowingCart GetBorrowingCartById(int borrowingCartId);
        bool AddToBorrowingCart(int bookId,string identityUserId);
        void RemoveFromBorrowingCart(int bookId, int borrowingCartId);
        void ClearBorrowingCart(int borrowingCartId);
        int AddNewLoan(int borrowingCartId, int userId);

    }
}
