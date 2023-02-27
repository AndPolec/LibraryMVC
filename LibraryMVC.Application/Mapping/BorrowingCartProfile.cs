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
    public class BorrowingCartProfile : Profile
    {
        public BorrowingCartProfile()
        {
            CreateMap<BorrowingCart, BorrowingCartDetailsVm>()
                .ForMember(d => d.Count, opt => opt.MapFrom(s => s.Books.Count));

        }
    }
}
