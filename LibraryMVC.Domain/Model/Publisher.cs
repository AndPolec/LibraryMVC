namespace LibraryMVC.Domain.Model
{
    public class Publisher
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}