using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.IdentityUserRoles
{
    public class IdentityUserRolesDetailsVm
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Użytkownik")]
        public string UserName { get; set; }

        [DisplayName("Uprawnienia")]
        public List<string> UserRoles { get; set; }

        [DisplayName("Wybierz nowe:")]
        public List<RoleVm> AllRoles { get; set; }
    }
}
