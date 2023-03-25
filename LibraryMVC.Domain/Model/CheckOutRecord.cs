using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Model
{
    public class CheckOutRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int LoanId { get; set; }
        public Loan Loan { get; set; }
        public int AdditionalLibrarianInfoId { get; set; }
        public AdditionalLibrarianInfo AdditionalLibrarianInfo { get; set; }
    }
}
