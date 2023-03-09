using AutoMapper;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Application.ViewModels.Loan;
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
            CreateMap<Loan, LoanForListVm>()
                .ForMember(d => d.NumberOfBorrowedBooks, opt => opt.MapFrom(s => s.Books.Count))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.Name));
        }
    }
}
