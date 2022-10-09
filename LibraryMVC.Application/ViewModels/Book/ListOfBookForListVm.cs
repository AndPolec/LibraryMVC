using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class ListOfBookForListVm
    {
        public List<BookForListVm> Books { get; set; }
        public int Count { get; set; }

    }
}
