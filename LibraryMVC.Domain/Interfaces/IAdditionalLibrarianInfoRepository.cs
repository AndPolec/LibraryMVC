using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IAdditionalLibrarianInfoRepository
    {
        int AddNew(AdditionalLibrarianInfo librarianInfo);
        AdditionalLibrarianInfo GetInfoByIdentityUserId(string id);
        
    }
}