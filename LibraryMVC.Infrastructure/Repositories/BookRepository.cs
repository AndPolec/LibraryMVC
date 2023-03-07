using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
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
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public void UpdateBook(Book book)
        {
            var bookGenres = _context.BookGenre.Where(x => x.BookId == book.Id);
            _context.BookGenre.RemoveRange(bookGenres);

            _context.Attach(book);
            _context.Entry(book).Property("Title").IsModified = true;
            _context.Entry(book).Property("ISBN").IsModified = true;
            _context.Entry(book).Property("RelaseYear").IsModified = true;
            _context.Entry(book).Property("Quantity").IsModified = true;
            _context.Entry(book).Property("AuthorId").IsModified = true;
            _context.Entry(book).Property("PublisherId").IsModified = true;
            _context.Entry(book).Collection("BookGenres").IsModified = true;
            _context.SaveChanges();
        }

        public Book GetBookById(int bookId)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .FirstOrDefault(b => b.Id == bookId);

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

        public void UpdateBooksQuantity(ICollection<Book> books)
        {
            foreach (var book in books)
            {
                _context.Entry(book).Property("Quantity").IsModified = true;
            }
            _context.SaveChanges();
        }
    }
}
