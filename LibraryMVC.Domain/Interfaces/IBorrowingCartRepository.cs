﻿using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IBorrowingCartRepository
    {
        BorrowingCart GetBorrowingCartByIndentityUserId(string id);
        BorrowingCart GetBorrowingCartById(int borrowingCartId);
        void AddToBorrowingCart(int bookId, string identityUserId);
        void RemoveFromBorrowingCart(int bookId, int borrowingCartId);
        void RemoveAllFromBorrowingCart(int borrowingCartId);

    }
}