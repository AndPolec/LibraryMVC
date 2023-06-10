using LibraryMVC.Application.ViewModels.LibraryUser;
using LibraryMVC.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Interfaces
{
    public interface ILibraryUserService
    {
        int AddUser(NewLibraryUserVm model);
        bool IsUserDataExists(string identityUserId);
        List<LibraryUserForListVm> GetAllLibraryUsersForList();
        LibraryUserDetailsVm GetLibraryUserForDetails(int id);
        bool IsBlocked(int userId);
        bool IsBlocked(string identityUserId);
        void BlockUser(int userId);
        void UnblockUser(int userId);
    }
}
