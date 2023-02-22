using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Model
{
    public class BorrowingCart
    {
        public int Id { get; set; }
        public int LibraryUserId { get; set; }
        public LibraryUser LibraryUser { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
