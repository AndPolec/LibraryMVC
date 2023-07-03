using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Model
{
    public class UserType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<LibraryUser> LibraryUsers { get; set; }
    }
}
