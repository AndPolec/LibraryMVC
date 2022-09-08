using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        int AddAuthor(Author author);
        void DeleteAuthor(int authorId);
        IQueryable<Author> GetAllAuthors();
        void UpdateAuthor(Author author);
    }
}