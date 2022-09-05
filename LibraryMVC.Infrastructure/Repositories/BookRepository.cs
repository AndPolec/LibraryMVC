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

        //Book
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

        //Genre
        public int AddGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return genre.Id;
        }

        public void DeleteGenre(int genreId)
        {
            var genre = _context.Genres.Find(genreId);
            if(genre != null)
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
            }
        }

        public Genre GetGenreById(int genreId)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.Id == genreId);
            return genre;
        }

        public IQueryable<Genre> GetAllGenres()
        {
            var genres = _context.Genres;
            return genres;
        }

        public void UpdateGenre(Genre genre)
        {
            _context.Genres.Update(genre);
            _context.SaveChanges();
        }


        //Publisher
        public int AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
            return publisher.Id;
        }

        public void DeletePublisher(int publisherId)
        {
            var publisher = _context.Publishers.Find(publisherId);
            if(publisher != null)
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
            }
        }

        public void UpdatePublisher(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            _context.SaveChanges();
        }

        public Publisher GetPublisherById(int publisherId)
        {
            var publisher = _context.Publishers.Find(publisherId);
            return publisher;
        }

        public IQueryable<Publisher> GetAllPublishers()
        {
            var publishers = _context.Publishers;
            return publishers;
        }
    }
}
