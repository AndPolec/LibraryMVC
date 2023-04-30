using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.LibraryUser
{
    public class LibraryUserForListVm
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal UnpaidPenaltiesTotal { get; set; }
        public int OverdueLoansCount { get; set; }
    }
}
