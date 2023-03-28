using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.ReturnRecord
{
    public class NewReturnRecordVm
    {
        public int Id { get; set; }
        public string FullLibraryUserName { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime ReturnDueDate { get; set; }
        public bool IsPenaltyPaid { get; set; }
        public decimal PenaltyForHoldingBooks { get; set; }
        public decimal AdditionalPenaltyForLostAndDestroyedBooks { get; set; }
        public decimal TotalAmountOfPaidPenalty { get; set; }
        public string Comments { get; set; }
        public List<int> LostOrDestroyedBooksId { get; set; }
        public List<int> ReturnedBooksId { get; set; }
        public List<BookForListVm> BorrowedBooks { get; set; }
    }
}