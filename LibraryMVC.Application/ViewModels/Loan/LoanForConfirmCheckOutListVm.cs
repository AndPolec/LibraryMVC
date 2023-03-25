using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class LoanForConfirmCheckOutListVm
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserName { get; set; }     
        public List<string> BorrowedBooks { get; set; }
        public string Status { get; set; }
    }
}
