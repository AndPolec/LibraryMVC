using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Model
{
    public class ReturnRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsPenaltyPaid { get; set; }
        public bool IsReturned { get; set; }
        public decimal OverduePenalty { get; set; }
        public decimal AdditionalPenaltyForLostAndDestroyedBooks  { get; set; }
        public decimal TotalPenalty { get; set; }
        public string Comments { get; set; }
        public int LoanId { get; set; }
        public Loan Loan { get; set; }
        public int? AdditionalLibrarianInfoId { get; set; }
        public AdditionalLibrarianInfo AdditionalLibrarianInfo { get; set; }
        public ICollection<Book> LostOrDestroyedBooks { get; set; }
        public ICollection<Book> ReturnedBooks { get; set; }
    }
}
