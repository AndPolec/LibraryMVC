using FluentValidation;
using LibraryMVC.Application.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Loan
{
    public class LoanSettingsVm
    {
        [DisplayName("Opłata za jeden dzień przetrzymania książki")]
        public decimal penaltyRatePerDay { get; set; }

        [DisplayName("Ilość dni darmowego wypożyczenia")]
        public int durationOfFreeLoan { get; set; }

        [DisplayName("Maksymalna ilość książek w jednym zamówieniu")]
        public int maxBooksInOrder { get; set; }
    }

    public class LoanSettingsVmValidator : AbstractValidator<LoanSettingsVm>
    {
        public LoanSettingsVmValidator()
        {
            RuleFor(x => x.penaltyRatePerDay).NotEmpty().WithMessage("Pole nie może być puste lub równe 0").GreaterThan(-1).WithMessage("Pole nie może być mniejsze niż 0.");
            RuleFor(x => x.durationOfFreeLoan).NotEmpty().WithMessage("Pole nie może być puste lub równe 0").GreaterThan(-1).WithMessage("Pole nie może być mniejsze niż 0.");
            RuleFor(x => x.maxBooksInOrder).NotEmpty().WithMessage("Pole nie może być puste lub równe 0").GreaterThan(-1).WithMessage("Pole nie może być mniejsze niż 0.");
        }
    }
}
