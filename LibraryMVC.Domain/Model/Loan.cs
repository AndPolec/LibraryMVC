using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Model
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime ReturnDueDate { get; set; }
        public decimal Penalty { get; set; }

        public Reservation Reservation { get; set; }
        public int CheckOutLibrarianId { get; set; }
        public virtual Librarian CheckOutLibrarian { get; set; }
        public int CheckInLibrarianId { get; set; }
        public virtual Librarian CheckInLibrarian { get; set; }

    }
}
