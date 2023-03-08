using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class LoanForListVm
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReturnDueDate { get; set; }
        public int NumberOfBorrowedBooks { get; set; }
        public string Status { get; set; }
    }
}
