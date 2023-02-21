using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IUserRepository
    {
        int AddUser(User user);
        void DeleteUser(int userId);
        IQueryable<User> GetAllUsers();
        User GetUserById(int userId);
        void UpdateUser(User user);
        bool CheckIsUserExistsByIdentityUserId(string userId);
    }
}