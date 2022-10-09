using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class BookDetailsVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorFullName { get; set; }
        public int RelaseYear { get; set; }
        public List<string> Genres { get; set; }
        public string PublisherName { get; set; }
        public string ISBN { get; set; }
        public bool IsAvailableForReservation { get; set; }
    }
}
