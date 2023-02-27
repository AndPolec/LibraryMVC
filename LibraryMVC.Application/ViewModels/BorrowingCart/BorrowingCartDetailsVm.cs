using LibraryMVC.Application.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.BorrowingCart
{
    public class BorrowingCartDetailsVm
    {
        public List<BookForListVm> Books { get; set; }
        public int Count { get; set; }
        public int LibraryUserId { get; set; }

    }
}
