﻿using LibraryMVC.Domain.Interfaces;
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

        public void AddToBorrowingCart(int bookId, string identityUserId)
        {
            var borrowingCart = _context.BorrowingCarts.Include(bc => bc.Books).FirstOrDefault(bc => bc.LibraryUser.IdentityUserId == identityUserId);
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (borrowingCart != null)
            {
                borrowingCart.Books.Add(book);
                _context.SaveChanges();
            }
        }

        public BorrowingCart GetBorrowingCartByIndentityUserId(string id)
        {
            var user = _context.LibraryUsers
                .Include(u => u.BorrowingCart).ThenInclude(bc => bc.Books).ThenInclude(b => b.Author)
                .Include(u => u.BorrowingCart).ThenInclude(bc => bc.Books).ThenInclude(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .FirstOrDefault(u => u.IdentityUserId == id);

            var borrowingCart = user.BorrowingCart;
            return borrowingCart;
        }

        public void RemoveFromBorrowingCart(int bookId, int borrowingCartId)
        {
            var borrowingCart = _context.BorrowingCarts.Find(borrowingCartId);
            if (borrowingCart != null)
            {
               var book = borrowingCart.Books.FirstOrDefault(b => b.Id == bookId);
               borrowingCart.Books.Remove(book);
                _context.SaveChanges(); 
            }
        }
    }
}
