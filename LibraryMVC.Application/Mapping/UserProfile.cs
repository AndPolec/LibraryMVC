using AutoMapper;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.IdentityUserRoles;
using LibraryMVC.Application.ViewModels.LibraryUser;
using LibraryMVC.Application.ViewModels.User;
using LibraryMVC.Domain.Model;
using Microsoft.AspNetCore.Identity;
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

            CreateMap<Address, AddressDetailsVm>();

            CreateMap<NewLibraryUserVm, LibraryUser>()
                .ForMember(d => d.Loans, opt => opt.MapFrom(s => new List<Loan>()))
                .ForMember(d => d.isBlocked, opt => opt.MapFrom(s => false));

            CreateMap<LibraryUser, NewLibraryUserVm>();

            CreateMap<LibraryUser, LibraryUserForListVm>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(d => d.OverdueLoansCount, opt => opt.MapFrom(s => s.Loans.Count(l => l.isOverdue == true)))
                .ForMember(d => d.UnpaidPenaltiesTotal, opt => opt.MapFrom(s => s.Loans
                    .Where(l => l.ReturnRecord != null && l.ReturnRecord.IsPenaltyPaid == false).Sum(l => l.ReturnRecord.TotalPenalty)));

            CreateMap<LibraryUser, LibraryUserDetailsVm>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(d => d.UnpaidPenaltiesTotal, opt => opt.MapFrom(s => s.Loans
                    .Where(l => l.ReturnRecord != null && l.ReturnRecord.IsPenaltyPaid == false).Sum(l => l.ReturnRecord.TotalPenalty)))
                .ForMember(d => d.OverdueLoansCount, opt => opt.MapFrom(s => s.Loans.Count(l => l.isOverdue == true)));

            CreateMap<LibraryUser, LibraryUserForPersonalDataVm>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(d => d.UnpaidPenaltiesTotal, opt => opt.MapFrom(s => s.Loans
                    .Where(l => l.ReturnRecord != null && l.ReturnRecord.IsPenaltyPaid == false).Sum(l => l.ReturnRecord.TotalPenalty)))
                .ForMember(d => d.OverdueLoansCount, opt => opt.MapFrom(s => s.Loans.Count(l => l.isOverdue == true)));

            CreateMap<IdentityUser, IdentityUsersForListVm>();
            CreateMap<IdentityRole, RoleVm>();
            CreateMap<IdentityUser, IdentityUserRolesDetailsVm>();

        }
    }
}
