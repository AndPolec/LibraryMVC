using LibraryMVC.Application.ViewModels.Author;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Application.ViewModels.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Interfaces
{
    public interface IBookService
    {
        int AddBook(NewBookVm model);
        void DeleteBook(int id);
        bool IsBookInDatabase(int bookId);
        ListOfBookForListVm GetAllBooksForList(int pageSize, int pageNumber, string searchString);
        BookDetailsVm GetBookForDetails(int bookId);
        CreateBookVm GetInfoForAddNewBook();
        ListOfGenreForListVm GetAllGenresForList();
        int AddGenre(GenreForListVm model);
        int AddPublisher(PublisherForListVm model);
        int AddAuthor(NewAuthorVm model);
        ListOfAuthorForListVm GetAllAuthorsForList();
        ListOfPublisherForListVm GetAllPublishersForList();
        CreateBookVm GetInfoForBookEdit(int id);
        void UpdateBook(NewBookVm model);
        BookFormListsVm GetBookFormLists();
    }
}
