using AutoMapper;
using LibraryMVC.Application.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<LibraryMVC.Domain.Model.Book, BookForListVm>()
                .ForMember(d => d.AuthorFullName, opt => opt.MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
                .ForMember(d => d.Genre, opt => opt.MapFrom(s => String.Join(", ", s.BookGenres.Select(g => g.Genre.Name))));
            
        }

        
    }
}
