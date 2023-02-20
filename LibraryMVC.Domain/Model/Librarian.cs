using System.Security.Principal;

namespace LibraryMVC.Domain.Model
{
    public class Librarian : User
    {
        
        public ICollection<CheckOutRecord> CheckOutRecords { get; set; }
        public ICollection<ReturnRecord> ReturnRecords { get; set; }
    }
}