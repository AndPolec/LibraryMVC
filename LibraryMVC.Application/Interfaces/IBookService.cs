using LibraryMVC.Application.ViewModels.Book;
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
    }
}
