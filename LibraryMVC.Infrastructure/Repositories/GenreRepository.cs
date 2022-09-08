using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {

        private readonly Context _context;

        public GenreRepository(Context context)
        {
            context = _context;
        }

        public int AddGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return genre.Id;
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

        public void DeleteGenre(int genreId)
        {
            var genre = _context.Genres.Find(genreId);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
            }
        }
    }
}
