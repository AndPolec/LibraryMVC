using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IGenreRepository
    {
        int AddGenre(Genre genre);
        void DeleteGenre(int genreId);
        IQueryable<Genre> GetAllGenres();
        Genre GetGenreById(int genreId);
        void UpdateGenre(Genre genre);
    }
}