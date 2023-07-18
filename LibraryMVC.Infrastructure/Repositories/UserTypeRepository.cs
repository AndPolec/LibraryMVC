using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    internal class UserTypeRepository : IUserTypeRepository
    {
        private readonly Context _context;

        public UserTypeRepository(Context context)
        {
            _context = context;
        }

        public IQueryable<UserType> GetAll()
        {
            return _context.UserTypes;
        }
    }
}
