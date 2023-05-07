using LibraryMVC.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.LibraryUser
{
    public class LibraryUserDetailsVm
    {
        public int Id { get; set; }
        [DisplayName("Imię")]
        public string FirstName { get; set; }
        [DisplayName("Nazwisko")]
        public string LastName { get; set; }
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [DisplayName("Numer telefonu")]
        public string PhoneNumber { get; set; }
        public AddressDetailsVm Address { get; set; }
        [DisplayName("Suma zaległości")]
        public decimal UnpaidPenaltiesTotal { get; set; }
        [DisplayName("Liczba zaległych zamówień")]
        public int OverdueLoansCount { get; set; }
    }
}
