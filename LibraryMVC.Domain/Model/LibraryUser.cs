using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryMVC.Domain.Model
{
    public class LibraryUser
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Address Address { get; set; }
        public BorrowingCart BorrowingCart { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
