using AutoMapper;
using LibraryMVC.Application.ViewModels.Book;
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
                .ForMember(d => d.Loans, opt => opt.Ignore());




        }
    }
}
