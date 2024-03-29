﻿using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public DbSet<AdditionalLibrarianInfo> AdditionalLibrarianInfo { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<ReturnRecord> ReturnRecords { get; set; }
        public DbSet<LibraryUser> LibraryUsers { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<GlobalLoanSettings> GlobalLoanSettings { get; set; }


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

            builder.Entity<ReturnRecord>()
                .Property(r => r.OverduePenalty)
                .HasColumnType("decimal(8,2)");

            builder.Entity<ReturnRecord>()
                .Property(r => r.AdditionalPenaltyForLostAndDestroyedBooks)
                .HasColumnType("decimal(8,2)");

            builder.Entity<ReturnRecord>()
                .Property(r => r.TotalPenalty)
                .HasColumnType("decimal(8,2)");

            builder.Entity<ReturnRecord>()
                .HasMany(r => r.LostOrDestroyedBooks)
                .WithMany(b => b.ReturnRecordsWhereCopyOfBookWasLostOrDestroyed)
                .UsingEntity(j => j.ToTable("ReturnRecordLostOrDestroyedBooks"));

            builder.Entity<ReturnRecord>()
                .HasMany(r => r.ReturnedBooks)
                .WithMany(b => b.ReturnRecordsWhereCopyOfBookWasReturned)
                .UsingEntity(j => j.ToTable("ReturnRecordReturnedBooks"));

            builder.Entity<GlobalLoanSettings>()
                .Property(g => g.OverduePenaltyRatePerDayForOneBook)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Status>()
                .HasData(new Status { Id = 1, Name = "Nowe" },
                         new Status { Id = 2, Name = "Wypożyczone" },
                         new Status { Id = 3, Name = "Zakończone" },
                         new Status { Id = 4, Name = "Zaległe" },
                         new Status { Id = 5, Name = "Anulowane" });

            builder.Entity<UserType>()
                .HasData(new UserType { Id = 1, Name = "Czytelnik" },
                         new UserType { Id = 2, Name = "Bibliotekarz" },
                         new UserType { Id = 3, Name = "Administrator" });

            builder.Entity<IdentityRole>()
               .HasData(new IdentityRole { Id = "Reader", Name = "Czytelnik", NormalizedName = "Czytelnik" },
                        new IdentityRole { Id = "Librarian", Name = "Bibliotekarz", NormalizedName = "Bibliotekarz" },
                        new IdentityRole { Id = "Administrator", Name = "Administrator", NormalizedName = "Administrator" });

            builder.Entity<GlobalLoanSettings>()
                .HasData(new GlobalLoanSettings {Id = 1, DurationOfFreeLoanInDays = 21, MaxBooksInOrder = 5, OverduePenaltyRatePerDayForOneBook = 0.2M });
        }

    }
}
