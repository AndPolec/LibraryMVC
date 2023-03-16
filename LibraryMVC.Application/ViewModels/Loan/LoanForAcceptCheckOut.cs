using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class LoanForAcceptCheckOutVm
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReturnDueDate { get; set; }
        public int NumberOfBorrowedBooks { get; set; }
        public string Status { get; set; }
    }
}
