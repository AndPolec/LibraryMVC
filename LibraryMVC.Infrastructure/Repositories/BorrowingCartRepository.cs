using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class BorrowingCartRepository : IBorrowingCartRepository
    {
        private readonly Context _context;
        public BorrowingCartRepository(Context context)
        {
            _context = context;
        }

        public BorrowingCart GetBorrowingCartByIndentityUserId(string id)
        {
            var user = _context.LibraryUsers
                .Include(u => u.BorrowingCart)
                .FirstOrDefault(u => u.IdentityUserId == id);

            var borrowingCart = user.BorrowingCart;
            return borrowingCart;
        }
    }
}
