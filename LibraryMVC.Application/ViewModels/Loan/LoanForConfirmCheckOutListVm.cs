using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class LoanForConfirmCheckOutListVm
    {
        [DisplayName("Nr zamówienia")]
        public int Id { get; set; }

        [DisplayName("Data utworzenia")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Czytelnik")]
        public string UserName { get; set; }

        [DisplayName("Książki do wydania")]
        public List<string> BorrowedBooks { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}
