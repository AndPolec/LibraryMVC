using LibraryMVC.Application.ViewModels.IdentityUserRoles;
using Microsoft.AspNetCore.Identity;
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
        IQueryable<RoleVm> GetAllRoles();
        Task<List<string>>  GetRolesByUserIdAsync(string id);
        Task<IdentityUserRolesDetailsVm> GetUserRolesDetailsAsync(string id);
        Task<IdentityResult> ChangeUserRolesAsync(string userId, List<string> newRoles);
        Task<IdentityResult> SetStandardReaderRoleAsync(IdentityUser user);
    }
}
