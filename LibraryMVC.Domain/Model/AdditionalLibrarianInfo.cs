using System.Security.Principal;

namespace LibraryMVC.Domain.Model
{
    public class AdditionalLibrarianInfo 
    {
        public int Id { get; set; }
        public int LibraryUserId { get; set; }
        public LibraryUser LibraryUser { get; set; }     
        public ICollection<CheckOutRecord> CheckOutRecords { get; set; }
        public ICollection<ReturnRecord> ReturnRecords { get; set; }
    }
}