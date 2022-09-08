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
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user.Id;
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User GetUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            return user;
        }

        public IQueryable<User> GetAllUsers()
        {
            return _context.Users;
        }

    }
}
