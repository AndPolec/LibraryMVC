using AutoMapper;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.LibraryUser;
using LibraryMVC.Application.ViewModels.User;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AddressDetailsVm, Address>();

            CreateMap<NewLibraryUserVm, LibraryUser>()
                .ForMember(d => d.Loans, opt => opt.MapFrom(s => new List<Loan>()))
                .ForMember(d => d.isBlocked, opt => opt.MapFrom(s => false))
                .AfterMap((s, d) => { d.BorrowingCart.Books = new List<Book>(); });

            CreateMap<LibraryUser, LibraryUserForListVm>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(d => d.OverdueLoansCount, opt => opt.MapFrom(s => s.Loans.Count(l => l.ReturnDueDate.Date < DateTime.Now.Date)))
                .ForMember(d => d.UnpaidPenaltiesTotal, opt => opt.MapFrom(s => s.Loans
                    .Where(l => l.ReturnRecord != null && l.ReturnRecord.IsPenaltyPaid == false).Sum(l => l.ReturnRecord.TotalPenalty))); 




        }
    }
}
