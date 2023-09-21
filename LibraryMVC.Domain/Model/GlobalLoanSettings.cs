using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Model
{
    public class GlobalLoanSettings
    {
        public int Id { get; set; }
        public decimal OverduePenaltyRatePerDayForOneBook { get; set; }
        public int DurationOfFreeLoanInDays { get; set; }
        public int MaxBooksInOrder { get; set; }
    }
}
