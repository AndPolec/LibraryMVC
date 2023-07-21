using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IAdditionalLibrarianInfoRepository
    {
        AdditionalLibrarianInfo GetInfoByIdentityUserId(string id);
        AdditionalLibrarianInfo? GetInfoByLibraryUserId(int id);

    }
}