using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.LibraryUser
{
    public class LibraryUserForListVm
    {
        [DisplayName("Nr użytkownika")]
        public int Id { get; set; }

        [DisplayName("Imię i nazwisko")]
        public string FullName { get; set; }

        [DisplayName("Adres email")]
        public string Email { get; set; }

        [DisplayName("Suma zaległości")]
        public decimal UnpaidPenaltiesTotal { get; set; }

        [DisplayName("Ilość zaległych zamówień")]
        public int OverdueLoansCount { get; set; }
    }
}
