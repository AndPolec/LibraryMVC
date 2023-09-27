using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Helpers
{
    public enum LoanStatusId
    {
        New = 1,
        Borrowed = 2,
        Completed = 3,
        Overdue = 4,
        Cancelled = 5
    }
}
