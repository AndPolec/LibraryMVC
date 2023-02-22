using LibraryMVC.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Interfaces
{
    public interface IUserService
    {
        int AddUser(NewUserVm model);
        bool isUserDataExists(string identityUserId);
    }
}
