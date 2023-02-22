using AutoMapper;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Mapping
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<NewBorrowingCartVm, BorrowingCart>();
        }
    }
}
