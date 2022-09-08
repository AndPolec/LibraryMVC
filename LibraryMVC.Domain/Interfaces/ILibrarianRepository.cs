using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface ILibrarianRepository
    {
        int AddLibrarian(Librarian librarian);
        void DeleteLibrarian(int librarianId);
        IQueryable<Librarian> GetAllLibrarians();
        Librarian GetLibrarianById(int librarianId);
        void UpdateLibrarian(Librarian librarian);
    }
}