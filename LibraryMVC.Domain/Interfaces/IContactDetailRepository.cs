using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IContactDetailRepository
    {
        int AddContactDetail(ContactDetail contactDetail);
        int AddContactDetailType(ContactDetailType contactDetailType);
        void DeleteContactDetail(int contactDetailId);
        void DeleteContactDetailType(int contactDetailTypeId);
        ContactDetail GetContactDetailById(int contactDetailId);
        ContactDetailType GetContactDetailTypeById(int contactDetailTypeId);
        void UpdateContactDetail(ContactDetail contactDetail);
        void UpdateContactDetailType(ContactDetailType contactDetailType);
    }
}