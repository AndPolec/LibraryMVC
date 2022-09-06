using LibraryMVC.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class AddressRepository
    {
        private readonly Context _context;
        public AddressRepository(Context context)
        {
            _context = context; 
        }

        public int AddAddress(Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();

            return address.Id;
        }

        public Address GetAddressById(int addressId)
        {
            var address = _context.Addresses.FirstOrDefault(u => u.Id == addressId);
            return address;
        }

        public void DeleteAddress(int addressId)
        {
            var address = _context.Addresses.Find(addressId);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                _context.SaveChanges();
            }
        }

        public void UpdateAddress(Address address)
        {
            _context.Addresses.Update(address);
            _context.SaveChanges();
        }
    }
}
