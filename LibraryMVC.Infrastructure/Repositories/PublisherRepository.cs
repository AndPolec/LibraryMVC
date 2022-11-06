using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {

        private readonly Context _context;

        public PublisherRepository(Context context)
        {
            _context = context;
        }

        public int AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
            return publisher.Id;
        }

        public void DeletePublisher(int publisherId)
        {
            var publisher = _context.Publishers.Find(publisherId);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
            }
        }

        public void UpdatePublisher(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            _context.SaveChanges();
        }

        public Publisher GetPublisherById(int publisherId)
        {
            var publisher = _context.Publishers.Find(publisherId);
            return publisher;
        }

        public IQueryable<Publisher> GetAllPublishers()
        {
            var publishers = _context.Publishers;
            return publishers;
        }

    }
}
