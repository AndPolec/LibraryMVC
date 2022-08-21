using System.Security.Principal;

namespace LibraryMVC.Domain.Model
{
    public class Librarian
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Loan> AcceptedCheckOuts { get; set; }
        public virtual ICollection<Loan> AcceptedCheckIns { get; set; }


    }
}