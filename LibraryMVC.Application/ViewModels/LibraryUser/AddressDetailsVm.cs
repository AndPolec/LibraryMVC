using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.User
{
    public class AddressDetailsVm
    {
        public int Id { get; set; }

        [DisplayName("Ulica")]
        [Required]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Nazwa ulicy musi zaczynać się od dużej litery.")]
        public string Street { get; set; }

        [DisplayName("Nr domu")]
        public string BuildingNumber { get; set; }

        [DisplayName("Nr mieszkania")]
        public string? FlatNumber { get; set; }

        [DisplayName("Kod pocztowy")]
        [Required]
        [RegularExpression(@"\d{2}-\d{3}", ErrorMessage = "Kod pocztowy musi mieć format: XX-XXX")]
        public string ZipCode { get; set; }

        [DisplayName("Miasto")]
        [Required]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Nazwa miasta musi zaczynać się od dużej litery.")]
        public string City { get; set; }
    }

    public class AddressDetailsVmValidator : AbstractValidator<AddressDetailsVm>
    {
        public AddressDetailsVmValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Street).NotEmpty().Matches(@"^[A-Z]").WithMessage("Nazwa ulicy musi zaczynać się od dużej litery."); 
            RuleFor(x => x.BuildingNumber).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty().Matches(@"\d{2}-\d{3}").WithMessage("Kod pocztowy musi mieć format: XX-XXX");
            RuleFor(x => x.City).NotEmpty().Matches(@"^[A-Z]").WithMessage("Nazwa miasta musi zaczynać się od dużej litery.");
        }
    }
}
