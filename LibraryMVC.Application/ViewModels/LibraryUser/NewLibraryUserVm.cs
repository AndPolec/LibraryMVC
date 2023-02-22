using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.User
{
    public class NewLibraryUserVm
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDetailsVm Address { get; set; }
        public NewBorrowingCartVm BorrowingCart { get; set; }

    }
}
