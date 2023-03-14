using LibraryMVC.Application.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class LoanDetailsVm
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReturnDueDate { get; set; }
        public bool IsCheckedOut { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool IsReturned { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal Penalty { get; set; }
        public string Status { get; set; }

        public List<BookForListVm> Books { get; set; }


    }
}
