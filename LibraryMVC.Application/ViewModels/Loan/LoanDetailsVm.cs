using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.ReturnRecord;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class LoanDetailsVm
    {
        [DisplayName("Nr zamówienia")]
        public int Id { get; set; }

        [DisplayName("Data utworzenia")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Termin zwrotu")]
        public DateTime ReturnDueDate { get; set; }

        [DisplayName("Odebrane")]
        public bool IsCheckedOut { get; set; }

        [DisplayName("Data odebrania")]
        public DateTime CheckOutDate { get; set; }

        [DisplayName("Zwrócone")]
        public bool IsReturned { get; set; }

        [DisplayName("Data zwrotu")]
        public DateTime ReturnDate { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Brak zwrotu w terminie")]
        public bool isOverdue { get; set; }

        public ReturnRecordDetailsVm ReturnRecord { get; set; }

        [DisplayName("Wypożyczone książki")]
        public List<BookForListVm> Books { get; set; }


    }
}
