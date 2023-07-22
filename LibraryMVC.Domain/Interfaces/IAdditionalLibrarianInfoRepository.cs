using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IAdditionalLibrarianInfoRepository
    {
        Task AddNewLibrarianInfo(AdditionalLibrarianInfo librarianInfo);
        AdditionalLibrarianInfo GetInfoByIdentityUserId(string id);
        AdditionalLibrarianInfo? GetInfoByLibraryUserId(int id);

    }
}