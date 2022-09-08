using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IBookGenreRepository
    {
        void AddBookGenre(BookGenre bookGenre);
        void Delete(BookGenre bookGenre);
    }
}