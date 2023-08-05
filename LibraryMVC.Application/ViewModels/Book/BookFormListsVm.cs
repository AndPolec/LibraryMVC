using LibraryMVC.Application.ViewModels.Author;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Application.ViewModels.Publisher;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class BookFormListsVm
    {
        public ListOfAuthorForListVm AllAuthors { get; set; }
        public ListOfGenreForListVm AllGenres { get; set; }
        public ListOfPublisherForListVm AllPublishers { get; set; }
    }
}