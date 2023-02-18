using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Model
{
    public class ContactDetail
    {
        public int Id { get; set; }
        public string ContactDetailInformation { get; set; }
        public  int ContactDetailTypeId { get; set; }
        public virtual ContactDetailType ContactDetailType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
