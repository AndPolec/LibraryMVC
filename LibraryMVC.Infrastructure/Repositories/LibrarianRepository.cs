using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class LibrarianRepository
    {
        private readonly Context _context;
        public LibrarianRepository(Context context)
        {
            _context = context;
        }

        public int AddLibrarian(Librarian librarian)
        {
            _context.Librarians.Add(librarian);
            _context.SaveChanges();
            return librarian.Id;
        }

        public Librarian GetLibrarianById(int librarianId)
        {
            var librarian = _context.Librarians.FirstOrDefault(l => l.Id == librarianId);
            return librarian;
        }

        public IQueryable<Librarian> GetAllLibrarians()
        {
            var librarians = _context.Librarians;
            return librarians;
        }

        public void UpdateLibrarian(Librarian librarian)
        {
            _context.Librarians.Update(librarian);
            _context.SaveChanges();
        }

        public void DeleteLibrarian(int librarianId)
        {
            var librarian = _context.Librarians.Find(librarianId);
            if (librarian != null)
            {
                _context.Librarians.Remove(librarian);
                _context.SaveChanges();
            }
        }
    }
}
