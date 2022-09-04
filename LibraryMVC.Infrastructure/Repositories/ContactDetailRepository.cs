using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class ContactDetailRepository
    {
        
        private readonly Context _context;

        public ContactDetailRepository(Context context)
        {
            _context = context;
        }

        //ContactDetail object

        public int AddContactDetail(ContactDetail contactDetail)
        {
            _context.ContactDetails.Add(contactDetail);
            _context.SaveChanges();

            return contactDetail.Id;
        }

        public ContactDetail GetContactDetailById(int contactDetailId)
        {
            var contactDetail = _context.ContactDetails.FirstOrDefault(cd => cd.Id == contactDetailId);
            return contactDetail;
        }

        public void DeleteContactDetail(int contactDetailId)
        {
            var contactDetail = _context.ContactDetails.Find(contactDetailId);
            if (contactDetail != null)
            {
                _context.ContactDetails.Remove(contactDetail);
                _context.SaveChanges();
            }
        }

        public void UpdateContactDetail(ContactDetail contactDetail)
        {
            _context.ContactDetails.Update(contactDetail);
            _context.SaveChanges();
        }

        //ContactDetailType object
        public int AddContactDetailType(ContactDetailType contactDetailType)
        {
            _context.ContactDetailTypes.Add(contactDetailType);
            _context.SaveChanges();

            return contactDetailType.Id;
        }

        public ContactDetailType GetContactDetailTypeById(int contactDetailTypeId)
        {
            var contactDetailType = _context.ContactDetailTypes.FirstOrDefault(cd => cd.Id == contactDetailTypeId);
            return contactDetailType;
        }

        public void DeleteContactDetailType(int contactDetailTypeId)
        {
            var contactDetailType = _context.ContactDetailTypes.Find(contactDetailTypeId);
            if (contactDetailType != null)
            {
                _context.ContactDetailTypes.Remove(contactDetailType);
                _context.SaveChanges();
            }
        }

        public void UpdateContactDetailType(ContactDetailType contactDetailType)
        {
            _context.ContactDetailTypes.Update(contactDetailType);
            _context.SaveChanges();
        }
    }
}
