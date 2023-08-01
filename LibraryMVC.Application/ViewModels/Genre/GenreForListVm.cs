using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Genre
{
    public class GenreForListVm
    {
        public int Id { get; set; }

        [DisplayName("Nazwa")]
        [Required]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Nazwa gatunku musi zaczynać się od dużej litery.")]
        public string Name { get; set; }
    }
}
