using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public Address Address { get; set; }
        public ICollection<ContactDetail> ContactDetails { get; set; }
        public BorrowCart BorrowCart { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
