using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IAddressRepository
    {
        int AddAddress(Address address);
        void DeleteAddress(int addressId);
        Address GetAddressById(int addressId);
        void UpdateAddress(Address address);
    }
}