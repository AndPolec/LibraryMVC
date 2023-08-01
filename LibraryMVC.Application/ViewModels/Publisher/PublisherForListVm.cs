using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Publisher
{
    public class PublisherForListVm
    {
        public int Id { get; set; }

        [DisplayName("Nazwa")]
        [Required]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Nazwa wydawcy musi zaczynać się od dużej litery.")]
        public string Name { get; set; }
    }
}
