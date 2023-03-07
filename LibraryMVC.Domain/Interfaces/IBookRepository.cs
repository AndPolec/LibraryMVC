using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IBookRepository
    {
        int AddBook(Book book);
        void DeleteBook(int bookId);
        IQueryable<Book> GetAllBooks();
        Book GetBookById(int bookId);
        IQueryable<Book> GetBooksByAuthorId(int authorId);
        IQueryable<Book> GetBooksByGenreId(int genreId);
        IQueryable<Book> GetBooksByPublisherId(int publisherId);
        void UpdateBook(Book book);
        void UpdateBooksQuantity(ICollection<Book> books);
    }
}