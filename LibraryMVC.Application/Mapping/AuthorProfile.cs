using AutoMapper;
using LibraryMVC.Application.ViewModels.Author;
using LibraryMVC.Application.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<LibraryMVC.Domain.Model.Author, AuthorForListVm>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.LastName + " " + s.FirstName));

        }
    }
}
