using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class LoanForConfirmReturnListVm
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReturnDueDate { get; set; }
        public int NumberOfBorrowedBooks { get; set; }
        public decimal Penalty { get; set; }
        public string Status { get; set; }
    }
}
