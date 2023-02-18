namespace LibraryMVC.Domain.Model
{
    public class ContactDetailType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ContactDetail> ContactDetails { get; set; }
    }
}