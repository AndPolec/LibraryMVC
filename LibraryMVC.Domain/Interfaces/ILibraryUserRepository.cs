using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface ILibraryUserRepository
    {
        int AddUser(LibraryUser user);
        void DeleteUser(int userId);
        IQueryable<LibraryUser> GetAllUsers();
        LibraryUser GetUserById(int userId);
        LibraryUser GetUserByIdentityUserId(string userId);
        void UpdateUser(LibraryUser user);
    }
}