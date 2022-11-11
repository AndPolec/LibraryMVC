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
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public int? RelaseYear { get; set; }
        public int? Quantity { get; set; }
        public List<int>? GenreIds { get; set; }
        public int? AuthorId { get; set; }
        public int? PublisherId { get; set; }

        public ListOfAuthorForListVm AllAuthors { get; set; }
        public ListOfGenreForListVm AllGenres { get; set; }
        public ListOfPublisherForListVm AllPublishers { get; set; }
    }

    public class NewBookVmValidator : AbstractValidator<NewBookVm>
    {
        public NewBookVmValidator() 
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).NotEmpty().WithMessage("Tytuł nie może być pusty.");
            RuleFor(x => x.ISBN).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("ISBN nie może być pusty.")
                .Custom((i, context) =>
                {
                    if (!(i.Length == 10 || i.Length == 13))
                        context.AddFailure("ISBN powinien zawierać 10 lub 13 cyfr.");
                });
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Pole 'Ilość' nie może być puste.")
                .GreaterThan(-1).WithMessage("Ilość nie może być ujemna.");
            RuleFor(x => x.RelaseYear).NotNull()
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
