using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Services
{
    public class LoanService : ILoanService
    {
        public BorrowingCartVm GetBorrowingCart(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
