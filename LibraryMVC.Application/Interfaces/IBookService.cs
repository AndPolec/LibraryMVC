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
        int AddBook(NewBookVm book);
        ListOfBookForListVm GetAllBooksForList(int pageSize, int pageNumber, string searchString);
        BookDetailsVm GetBook(int bookId);
        AddNewBookVm GetAllInfoForAddNewBook();
        ListOfGenreForListVm GetAllGenresForList();
        ListOfAuthorForListVm GetAllAuthorsForList();
        ListOfPublisherForListVm GetAllPublishersForList();
        

    }
}
