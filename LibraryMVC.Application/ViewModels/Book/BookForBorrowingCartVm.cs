using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class BookForBorrowingCartVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorFullName { get; set; }
        public int RelaseYear { get; set; }
        public string Genre { get; set; }
        public bool IsAvailableForReservation { get; set; }
    }
}
