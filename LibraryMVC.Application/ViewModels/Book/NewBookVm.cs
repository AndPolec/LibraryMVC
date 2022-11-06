using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class NewBookVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int RelaseYear { get; set; }
        public int Quantity { get; set; }
        public List<int> GenreId { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }

    }
}
