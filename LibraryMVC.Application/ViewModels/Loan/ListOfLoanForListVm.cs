using LibraryMVC.Application.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class ListOfLoanForListVm
    {
        public List<LoanForListVm> Loans { get; set; }
        public int CurrentPage { get; set; }
        public string SearchString { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
    }
}
