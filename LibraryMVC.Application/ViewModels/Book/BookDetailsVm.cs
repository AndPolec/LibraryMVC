using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class BookDetailsVm
    {
        public int Id { get; set; }

        [DisplayName("Tytuł")]
        public string Title { get; set; }

        [DisplayName("Autor")]
        public string AuthorFullName { get; set; }

        [DisplayName("Data wydania")]
        public int RelaseYear { get; set; }

        [DisplayName("Gatunek")]
        public string Genres { get; set; }

        [DisplayName("Wydawca")]
        public string PublisherName { get; set; }

        public string ISBN { get; set; }

        [DisplayName("Dostępna do wypożyczenia")]
        public bool IsAvailableForReservation { get; set; }
    }
}
