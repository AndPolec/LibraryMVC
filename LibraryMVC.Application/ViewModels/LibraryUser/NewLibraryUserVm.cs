﻿using FluentValidation;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.User
{
    public class NewLibraryUserVm
    {
        public int Id { get; set; }
        public string? IdentityUserId { get; set; }

        [DisplayName("Imię")]
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Imię musi zaczynać się od dużej litery.")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Nazwisko musi zaczynać się od dużej litery.")]
        public string LastName { get; set; }

        [DisplayName("E-mail")]
        public string? Email { get; set; }

        [DisplayName("Numer telefonu")]
        [Required, StringLength(9, MinimumLength = 9, ErrorMessage = "Numer telefonu musi zawierać 9 cyfr.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi być w formacie XXXXXXXXX")]
        public string PhoneNumber { get; set; }
        public AddressDetailsVm Address { get; set; }
    }

    public class NewLibraryUserVmValidator : AbstractValidator<NewLibraryUserVm>
    {
        public NewLibraryUserVmValidator() 
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.FirstName).NotEmpty().Matches(@"^[A-Z]").WithMessage("Imię musi zaczynać się od dużej litery.");
            RuleFor(x => x.LastName).NotEmpty().Matches(@"^[A-Z]").WithMessage("Nazwisko musi zaczynać się od dużej litery.");
            RuleFor(x => x.PhoneNumber).Length(9,9).WithMessage("Numer telefonu musi zawierać 9 cyfr.").Matches(@"^\d{9}").WithMessage("Numer telefonu musi być w formacie XXXXXXXXX");
            RuleFor(x => x.Address).SetValidator(new AddressDetailsVmValidator());
        }
    }
}
