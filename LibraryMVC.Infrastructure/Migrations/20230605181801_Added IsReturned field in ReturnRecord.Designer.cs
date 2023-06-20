﻿// <auto-generated />
using System;
using LibraryMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20230605181801_Added IsReturned field in ReturnRecord")]
    partial class AddedIsReturnedfieldinReturnRecord
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BookBorrowingCart", b =>
                {
                    b.Property<int>("BooksId")
                        .HasColumnType("int");

                    b.Property<int>("BorrowingCartsId")
                        .HasColumnType("int");

                    b.HasKey("BooksId", "BorrowingCartsId");

                    b.HasIndex("BorrowingCartsId");

                    b.ToTable("BookBorrowingCart");
                });

            modelBuilder.Entity("BookLoan", b =>
                {
                    b.Property<int>("BooksId")
                        .HasColumnType("int");

                    b.Property<int>("LoansId")
                        .HasColumnType("int");

                    b.HasKey("BooksId", "LoansId");

                    b.HasIndex("LoansId");

                    b.ToTable("BookLoan");
                });

            modelBuilder.Entity("BookReturnRecord", b =>
                {
                    b.Property<int>("LostOrDestroyedBooksId")
                        .HasColumnType("int");

                    b.Property<int>("ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId")
                        .HasColumnType("int");

                    b.HasKey("LostOrDestroyedBooksId", "ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId");

                    b.HasIndex("ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId");

                    b.ToTable("ReturnRecordLostOrDestroyedBooks", (string)null);
                });

            modelBuilder.Entity("BookReturnRecord1", b =>
                {
                    b.Property<int>("ReturnRecordsWhereCopyOfBookWasReturnedId")
                        .HasColumnType("int");

                    b.Property<int>("ReturnedBooksId")
                        .HasColumnType("int");

                    b.HasKey("ReturnRecordsWhereCopyOfBookWasReturnedId", "ReturnedBooksId");

                    b.HasIndex("ReturnedBooksId");

                    b.ToTable("ReturnRecordReturnedBooks", (string)null);
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.AdditionalLibrarianInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("LibraryUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LibraryUserId")
                        .IsUnique();

                    b.ToTable("AdditionalLibrarianInfo");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BuildingNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PublisherId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("RelaseYear")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PublisherId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.BookGenre", b =>
                {
                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.HasKey("GenreId", "BookId");

                    b.HasIndex("BookId");

                    b.ToTable("BookGenre");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.BorrowingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("LibraryUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LibraryUserId")
                        .IsUnique();

                    b.ToTable("BorrowingCarts");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.CheckOutRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AdditionalLibrarianInfoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("LoanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdditionalLibrarianInfoId");

                    b.HasIndex("LoanId")
                        .IsUnique();

                    b.ToTable("CheckOutRecords");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.LibraryUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isBlocked")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("LibraryUsers");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LibraryUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReturnDueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<bool>("isOverdue")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("LibraryUserId");

                    b.HasIndex("StatusId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.ReturnRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AdditionalLibrarianInfoId")
                        .HasColumnType("int");

                    b.Property<decimal>("AdditionalPenaltyForLostAndDestroyedBooks")
                        .HasColumnType("decimal(8,2)");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPenaltyPaid")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReturned")
                        .HasColumnType("bit");

                    b.Property<int>("LoanId")
                        .HasColumnType("int");

                    b.Property<decimal>("OverduePenalty")
                        .HasColumnType("decimal(8,2)");

                    b.Property<decimal>("TotalPenalty")
                        .HasColumnType("decimal(8,2)");

                    b.HasKey("Id");

                    b.HasIndex("AdditionalLibrarianInfoId");

                    b.HasIndex("LoanId")
                        .IsUnique();

                    b.ToTable("ReturnRecords");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Statuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Nowe"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Wypożyczone"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Zakończone"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Zaległe"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Anulowane"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BookBorrowingCart", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryMVC.Domain.Model.BorrowingCart", null)
                        .WithMany()
                        .HasForeignKey("BorrowingCartsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookLoan", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryMVC.Domain.Model.Loan", null)
                        .WithMany()
                        .HasForeignKey("LoansId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookReturnRecord", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.Book", null)
                        .WithMany()
                        .HasForeignKey("LostOrDestroyedBooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryMVC.Domain.Model.ReturnRecord", null)
                        .WithMany()
                        .HasForeignKey("ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookReturnRecord1", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.ReturnRecord", null)
                        .WithMany()
                        .HasForeignKey("ReturnRecordsWhereCopyOfBookWasReturnedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryMVC.Domain.Model.Book", null)
                        .WithMany()
                        .HasForeignKey("ReturnedBooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.AdditionalLibrarianInfo", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.LibraryUser", "LibraryUser")
                        .WithOne("additionalLibrarianInfo")
                        .HasForeignKey("LibraryMVC.Domain.Model.AdditionalLibrarianInfo", "LibraryUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LibraryUser");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Address", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.LibraryUser", "User")
                        .WithOne("Address")
                        .HasForeignKey("LibraryMVC.Domain.Model.Address", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Book", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId");

                    b.HasOne("LibraryMVC.Domain.Model.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId");

                    b.Navigation("Author");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.BookGenre", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.Book", "Book")
                        .WithMany("BookGenres")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryMVC.Domain.Model.Genre", "Genre")
                        .WithMany("BookGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.BorrowingCart", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.LibraryUser", "LibraryUser")
                        .WithOne("BorrowingCart")
                        .HasForeignKey("LibraryMVC.Domain.Model.BorrowingCart", "LibraryUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LibraryUser");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.CheckOutRecord", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.AdditionalLibrarianInfo", "AdditionalLibrarianInfo")
                        .WithMany("CheckOutRecords")
                        .HasForeignKey("AdditionalLibrarianInfoId");

                    b.HasOne("LibraryMVC.Domain.Model.Loan", "Loan")
                        .WithOne("CheckOutRecord")
                        .HasForeignKey("LibraryMVC.Domain.Model.CheckOutRecord", "LoanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdditionalLibrarianInfo");

                    b.Navigation("Loan");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Loan", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.LibraryUser", "LibraryUser")
                        .WithMany("Loans")
                        .HasForeignKey("LibraryUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryMVC.Domain.Model.Status", "Status")
                        .WithMany("Loans")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LibraryUser");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.ReturnRecord", b =>
                {
                    b.HasOne("LibraryMVC.Domain.Model.AdditionalLibrarianInfo", "AdditionalLibrarianInfo")
                        .WithMany("ReturnRecords")
                        .HasForeignKey("AdditionalLibrarianInfoId");

                    b.HasOne("LibraryMVC.Domain.Model.Loan", "Loan")
                        .WithOne("ReturnRecord")
                        .HasForeignKey("LibraryMVC.Domain.Model.ReturnRecord", "LoanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdditionalLibrarianInfo");

                    b.Navigation("Loan");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.AdditionalLibrarianInfo", b =>
                {
                    b.Navigation("CheckOutRecords");

                    b.Navigation("ReturnRecords");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Book", b =>
                {
                    b.Navigation("BookGenres");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Genre", b =>
                {
                    b.Navigation("BookGenres");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.LibraryUser", b =>
                {
                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("BorrowingCart")
                        .IsRequired();

                    b.Navigation("Loans");

                    b.Navigation("additionalLibrarianInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Loan", b =>
                {
                    b.Navigation("CheckOutRecord")
                        .IsRequired();

                    b.Navigation("ReturnRecord")
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Publisher", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("LibraryMVC.Domain.Model.Status", b =>
                {
                    b.Navigation("Loans");
                });
#pragma warning restore 612, 618
        }
    }
}
