using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly Context _context;

        public AuthorRepository(Context context)
        {
            _context = context;
        }

        public int AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
            return author.Id;
        }

        public IQueryable<Author> GetAllAuthors()
        {
            var authors = _context.Authors;
            return authors;
        }

        public void UpdateAuthor(Author author)
        {
            _context.Authors.Update(author);
            _context.SaveChanges();
        }

        public void DeleteAuthor(int authorId)
        {
            var author = _context.Authors.Find(authorId);
            if (author != null)
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
            }
        }

    }
}
