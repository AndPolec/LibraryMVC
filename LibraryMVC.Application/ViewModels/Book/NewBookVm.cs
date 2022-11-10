using FluentValidation;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class NewBookVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int RelaseYear { get; set; }
        public int Quantity { get; set; }
        public List<int> GenreIds { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }

    }

    public class NewBookVmValidator : AbstractValidator<NewBookVm>
    {
        public NewBookVmValidator() 
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.ISBN).Custom((i, context) =>
            {
                if (!(i.Length == 10 || i.Length == 13))
                    context.AddFailure("ISBN powinien zawierać 10 lub 13 cyfr.");
            });
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .GreaterThan(-1);
            RuleFor(x => x.RelaseYear).InclusiveBetween(1900, DateTime.Now.Year);
                
        }
    }
}
