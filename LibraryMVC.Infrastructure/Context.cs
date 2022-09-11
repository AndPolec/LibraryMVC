using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure
{
    public class Context : IdentityDbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }
        public DbSet<ContactDetail> ContactDetails { get; set; }
        public DbSet<ContactDetailType> ContactDetailTypes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }


        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Loan>()
                .HasOne(a => a.Reservation)
                .WithOne(b => b.Loan)
                .HasForeignKey<Reservation>(c => c.LoanId);

            builder.Entity<BookGenre>()
                .HasKey(it => new { it.GenreId, it.BookId });

            builder.Entity<BookGenre>()
                .HasOne<Book>(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            builder.Entity<BookGenre>()
                .HasOne<Genre>(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId);

            builder.Entity<Loan>()
                .HasOne(l => l.CheckInLibrarian)
                .WithMany(l => l.AcceptedCheckIns)
                .HasForeignKey(l => l.CheckInLibrarianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Loan>()
                .HasOne(l => l.CheckOutLibrarian)
                .WithMany(l => l.AcceptedCheckOuts)
                .HasForeignKey(l => l.CheckOutLibrarianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Loan>()
                .Property(l => l.Penalty)
                .HasColumnType("decimal(8,2)");
        }

    }
}
