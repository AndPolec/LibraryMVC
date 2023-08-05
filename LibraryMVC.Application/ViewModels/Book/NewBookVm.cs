using FluentValidation;
using LibraryMVC.Application.ViewModels.Author;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Application.ViewModels.Publisher;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class NewBookVm
    {
        public int Id { get; set; }

        [DisplayName("Tytuł")]
        public string? Title { get; set; }

        public string? ISBN { get; set; }

        [DisplayName("Rok wydania")]
        public int? RelaseYear { get; set; }

        [DisplayName("Ilość")]
        public int? Quantity { get; set; }

        [DisplayName("Gatunek")]
        public List<int>? GenreIds { get; set; }

        [DisplayName("Autor")]
        public int? AuthorId { get; set; }

        [DisplayName("Wydawca")]
        public int? PublisherId { get; set; }
    }

    public class NewBookVmValidator : AbstractValidator<NewBookVm>
    {
        public NewBookVmValidator() 
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).NotEmpty().WithMessage("Tytuł nie może być pusty.");
            RuleFor(x => x.ISBN).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("ISBN nie może być pusty.")
                .Custom((i, context) =>
                {
                    if (!(i.Length == 10 || i.Length == 13))
                        context.AddFailure("ISBN powinien zawierać 10 lub 13 cyfr.");
                });
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Pole 'Ilość' nie może być puste.")
                .GreaterThan(-1).WithMessage("Ilość nie może być ujemna.");
            RuleFor(x => x.RelaseYear).NotNull().WithName("Data wydania")
                .Custom((i, context) =>
                {
                    if (i > DateTime.Today.Year)
                       context.AddFailure("Data wydania książki nie może wskazywać na przyszłość");
                });
            RuleFor(x => x.GenreIds).NotEmpty().WithMessage("Proszę wybrać gatunek.");
            RuleFor(x => x.AuthorId).NotEmpty().WithMessage("Proszę wybrać autora.");
            RuleFor(x => x.PublisherId).NotEmpty().WithMessage("Proszę wybrać wydawcę.");
        }
    }
}
