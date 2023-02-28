using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.BorrowingCart
{
    public class NewBorrowingCartVm
    {
        public int Id { get; set; }
        public List<BookForListVm> Books { get; set; }
    }
}
