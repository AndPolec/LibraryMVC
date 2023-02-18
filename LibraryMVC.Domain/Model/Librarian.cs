using System.Security.Principal;

namespace LibraryMVC.Domain.Model
{
    public class Librarian
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<CheckOutRecord> CheckOutRecords { get; set; }
        public ICollection<ReturnRecord> ReturnRecords { get; set; }


    }
}