using System.Net;

namespace LibraryMVC.Domain.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int RelaseYear { get; set; }
        public int Quantity { get; set; }

        public ICollection<Genre> Genres { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }


        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

    }
}