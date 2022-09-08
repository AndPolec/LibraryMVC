using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IPublisherRepository
    {
        int AddPublisher(Publisher publisher);
        void DeletePublisher(int publisherId);
        IQueryable<Publisher> GetAllPublishers();
        Publisher GetPublisherById(int publisherId);
        void UpdatePublisher(Publisher publisher);
    }
}