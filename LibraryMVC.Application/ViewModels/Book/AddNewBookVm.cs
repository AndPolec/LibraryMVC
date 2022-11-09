using LibraryMVC.Application.ViewModels.Author;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Application.ViewModels.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class AddNewBookVm
    {
        public NewBookVm NewBookModel { get; set; }
        public ListOfAuthorForListVm ListOfAuthors { get; set; }
        public ListOfGenreForListVm ListOfGenres { get; set; }
        public ListOfPublisherForListVm ListOfPublishers { get; set; }
        
    }
}
