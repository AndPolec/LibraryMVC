using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Author
{
    public class NewAuthorVm
    {
        public int Id { get; set; }

        [DisplayName("Imię")]
        [Required]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Imię musi zaczynać się od dużej litery.")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        [Required]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Nazwisko musi zaczynać się od dużej litery.")]
        public string LastName { get; set; }
    }
}
