using LibraryMVC.Application.ViewModels.LibraryUserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Interfaces
{
    public interface IIdentityUserRolesService
    {
        List<IdentityUsersForListVm> GetAllUsers();
    }
}
