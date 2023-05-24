using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class LoanForListVm
    {
        [DisplayName("Nr zamówienia")]
        public int Id { get; set; }

        [DisplayName("Data utworzenia")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Termin zwrotu")]
        public DateTime ReturnDueDate { get; set; }

        [DisplayName("Liczba książek")]
        public int NumberOfBorrowedBooks { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}
