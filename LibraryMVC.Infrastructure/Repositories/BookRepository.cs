using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class BookRepository
    {
        private readonly Context _context;

        public BookRepository(Context context)
        {
            _context = context;
        }

        public int AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book.Id;
        }

        public void DeleteBook(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if(book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public Book GetBookById(int bookId)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            return book;
        }

        public IQueryable<Book> GetAllBooks()
        {
            var books = _context.Books;
            return books; 
        }

        public IQueryable<Book> GetBooksByGenreId(int genreId)
        {
            var books = _context.Books.Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId));
            return books;
        }

        public IQueryable<Book> GetBooksByAuthorId(int authorId)
        {
            var books = _context.Books.Where(b => b.AuthorId == authorId);
            return books;
        }

        public IQueryable<Book> GetBooksByPublisherId(int publisherId)
        {
            var books = _context.Books.Where(b => b.PublisherId == publisherId);
            return books;
        }
    }
}
