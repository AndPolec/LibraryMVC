using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.ReturnRecord
{
    public class ReturnRecordDetailsVm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsPenaltyPaid { get; set; }
        public bool IsReturned { get; set; }

        [DisplayName("Dodatkowa opłata za zgubione/zniszczone")]
        public decimal AdditionalPenaltyForLostAndDestroyedBooks { get; set; }

        [DisplayName("Suma nałożonych kar")]
        public decimal TotalAmountOfPaidPenalty { get; set; }
        public string Comments { get; set; }
        public int LoanId { get; set; }
        public string FullNameOfConfirmingLibrarian { get; set; }

        [DisplayName("Zgubione/Zniszczone książki")]
        public List<BookForListVm> LostOrDestroyedBooks { get; set; }

        [DisplayName("Zwrócone książki")]
        public List<BookForListVm> ReturnedBooks { get; set; }
    }
}
