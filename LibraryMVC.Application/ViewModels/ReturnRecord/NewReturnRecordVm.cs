﻿using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.ComponentModel;

namespace LibraryMVC.Application.ViewModels.ReturnRecord
{
    public class NewReturnRecordVm
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public bool IsReturned { get; set; }

        [DisplayName("Użytkownik")]
        public string FullLibraryUserName { get; set; }

        [DisplayName("Data odebrania")]
        public DateTime CheckOutDate { get; set; }

        [DisplayName("Termin zwrotu")]
        public DateTime ReturnDueDate { get; set; }

        [DisplayName("Kara została zapłacona")]
        public bool IsPenaltyPaid { get; set; }

        [DisplayName("Kara za przetrzymanie książek")]
        public decimal OverduePenalty { get; set; }

        [DisplayName("Kara za zgubione/uszkodzone książki")]
        public decimal AdditionalPenaltyForLostAndDestroyedBooks { get; set; }

        [DisplayName("Kara do zapłaty")]
        public decimal TotalPenalty { get; set; }

        [DisplayName("Komentarz")]
        public string Comments { get; set; }
        public int NumberOfBorrowedBooks { get; set; }
        public List<int>? LostOrDestroyedBooksId { get; set; }
        public List<int> ReturnedBooksId { get; set; }
        public List<BookForListVm>? BorrowedBooks { get; set; }
    }

    public class NewReturnRecordVmValidator : AbstractValidator<NewReturnRecordVm>
    {
        public NewReturnRecordVmValidator() 
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.LostOrDestroyedBooksId).NotNull();
            RuleFor(x => x.ReturnedBooksId).NotNull();
            RuleFor(x => x.Comments).NotEmpty().WithMessage("Komentarz jest wymagany.");
            RuleFor(x => x.TotalPenalty).ScalePrecision(2, 8, false);
            RuleFor(x => x.IsPenaltyPaid).Must(x => x.Equals(true)).WithMessage("Klient musi zapłacić karę podczas zwrotu zamówienia.");
            RuleFor(x => x).Must(x => x.LostOrDestroyedBooksId.Count + x.ReturnedBooksId.Count == x.NumberOfBorrowedBooks)
                .WithName("BorrowedBooks")
                .WithMessage("Każda wypożyczona książka musi zostać oznaczona jako zwrócona lub oddana.");
            RuleFor(x => x).Must(x => !x.ReturnedBooksId.Intersect(x.LostOrDestroyedBooksId).Any())
                .WithName("BorrowedBooks")
                .WithMessage("Książka nie może być oznaczona jednocześnie jako zgubiona i oddana.");

        }
    }
    
}