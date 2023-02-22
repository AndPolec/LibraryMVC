﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Model
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime LoanCreationDate { get; set; }
        public DateTime CheckOutDueDate { get; set; }
        public DateTime ReturnDueDate { get; set; }
        public decimal Penalty { get; set; }

        public int LibraryUserId { get; set; }
        public LibraryUser LibraryUser { get; set; }
        public CheckOutRecord CheckOutRecord { get; set; }
        public ReturnRecord ReturnRecord { get; set; }

    }
}
