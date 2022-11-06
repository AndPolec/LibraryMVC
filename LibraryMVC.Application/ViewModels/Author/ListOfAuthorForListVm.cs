using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Author
{
    public class ListOfAuthorForListVm
    {
        public List<AuthorForListVm> Authors { get; set; }
        public int Count { get; set; }
    }
}
