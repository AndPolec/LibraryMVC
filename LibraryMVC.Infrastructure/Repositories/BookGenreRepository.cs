using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class BookGenreRepository : IBookGenreRepository
    {
        private readonly Context _context;

        public BookGenreRepository(Context context)
        {
            _context = context;
        }

        public void AddBookGenre(BookGenre bookGenre)
        {
            _context.BookGenre.Add(bookGenre);
            _context.SaveChanges();
        }

        public void Delete(BookGenre bookGenre)
        {
            _context.BookGenre.Remove(bookGenre);
            _context.SaveChanges();
        }

    }
}
