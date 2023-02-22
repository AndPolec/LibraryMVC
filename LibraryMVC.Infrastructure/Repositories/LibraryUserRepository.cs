using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class LibraryUserRepository : ILibraryUserRepository
    {
        private readonly Context _context;

        public LibraryUserRepository(Context context)
        {
            _context = context;
        }

        public int AddUser(LibraryUser user)
        {
            _context.LibraryUsers.Add(user);
            _context.SaveChanges();

            return user.Id;
        }

        public void DeleteUser(int userId)
        {
            var user = _context.LibraryUsers.Find(userId);
            if (user != null)
            {
                _context.LibraryUsers.Remove(user);
                _context.SaveChanges();
            }
        }

        public void UpdateUser(LibraryUser user)
        {
            _context.LibraryUsers.Update(user);
            _context.SaveChanges();
        }

        public LibraryUser GetUserById(int userId)
        {
            var user = _context.LibraryUsers.FirstOrDefault(u => u.Id == userId);
            return user;
        }
        public LibraryUser GetUserByIdentityUserId(string userId)
        {
            var user = _context.LibraryUsers.FirstOrDefault(u => u.IdentityUserId == userId);
            return user;
        }

        public IQueryable<LibraryUser> GetAllUsers()
        {
            return _context.LibraryUsers;
        }
        
        public bool CheckIsUserExistsByIdentityUserId(string userId)
        {
            return _context.LibraryUsers.Any(u => u.IdentityUserId == userId);
        }

    }
}
