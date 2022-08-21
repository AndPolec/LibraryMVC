namespace LibraryMVC.Domain.Model
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ReservationDueDate { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Book> Books { get; set; }

        public int LoanId { get; set; }
        public Loan Loan { get; set; }
    }
}