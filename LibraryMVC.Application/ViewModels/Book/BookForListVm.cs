using AutoMapper;
using LibraryMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.ViewModels.Book
{
    public class BookForListVm : IMapFrom<LibraryMVC.Domain.Model.Book>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorFullName { get; set; }
        public int RelaseYear { get; set; }
        public List<string> Genres { get; set; }

        void Mapping(Profile profile)
        {
            profile.CreateMap<LibraryMVC.Domain.Model.Book, BookForListVm>()
                .ForMember(d => d.AuthorFullName, opt => opt.MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));
        }
    }
}
