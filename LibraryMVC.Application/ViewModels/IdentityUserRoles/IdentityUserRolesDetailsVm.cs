using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.IdentityUserRoles
{
    public class IdentityUserRolesDetailsVm
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<string> UserRoles { get; set; }
        public List<RoleVm> AllRoles { get; set; }
    }
}
