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
        ListOfBookForListVm GetAllBooksForList(int pageSize, int pageNumber, string searchString);
        BookDetailsVm GetBookForDetails(int bookId);
        NewBookVm GetInfoForAddNewBook();
        ListOfGenreForListVm GetAllGenresForList();
        ListOfAuthorForListVm GetAllAuthorsForList();
        ListOfPublisherForListVm GetAllPublishersForList();
        NewBookVm GetInfoForBookEdit(int id);
        void UpdateBook(NewBookVm model);
        NewBookVm SetParametersToVm(NewBookVm model);
    }
}
