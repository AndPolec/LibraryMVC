using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }
        public DbSet<BorrowingCart> BorrowingCarts { get; set; }
        public DbSet<CheckOutRecord> CheckOutRecords { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<ReturnRecord> ReturnRecords { get; set; }
        public DbSet<LibraryUser> LibraryUsers { get; set; }


        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookGenre>()
                .HasKey(it => new { it.GenreId, it.BookId });

            builder.Entity<BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            builder.Entity<BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId);

            builder.Entity<Book>()
                .HasMany(b => b.Genres)
                .WithMany(g => g.Books)
                .UsingEntity<BookGenre>(
                    j => j.HasOne(g => g.Genre).WithMany(b => b.BookGenres),
                    j => j.HasOne(g => g.Book).WithMany(b => b.BookGenres));

            builder.Entity<Loan>()
                .Property(l => l.Penalty)
                .HasColumnType("decimal(8,2)");
        }

    }
}
