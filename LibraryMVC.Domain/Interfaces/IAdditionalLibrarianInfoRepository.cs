using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IAdditionalLibrarianInfoRepository
    {
        int AddLibrarian(AdditionalLibrarianInfo librarian);
        
    }
}