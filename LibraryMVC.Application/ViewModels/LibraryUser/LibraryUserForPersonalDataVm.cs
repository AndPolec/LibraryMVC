using LibraryMVC.Application.ViewModels.Loan;
using LibraryMVC.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.LibraryUser
{
    public class LibraryUserForPersonalDataVm
    {
        public int Id { get; set; }

        [DisplayName("Imię i nazwisko")]
        public string FullName { get; set; }

        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DisplayName("Numer telefonu")]
        public string PhoneNumber { get; set; }

        public AddressDetailsVm Address { get; set; }

        public bool IsBlocked { get; set; }

        [DisplayName("Suma zaległości")]
        public decimal UnpaidPenaltiesTotal { get; set; }

        [DisplayName("Liczba zaległych zamówień")]
        public int OverdueLoansCount { get; set; }
    }
}
