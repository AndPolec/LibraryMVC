﻿using AutoMapper;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Domain.Model;
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
            CreateMap<Book, BookForListVm>()
                .ForMember(d => d.AuthorFullName, opt => opt.MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
                .ForMember(d => d.Genre, opt => opt.MapFrom(s => String.Join(", ", s.BookGenres.Select(g => g.Genre.Name))));

            CreateMap<int, BookGenre>()
                .ForMember(d => d.GenreId, opt => opt.MapFrom(s => s));

            CreateMap<Book, NewBookVm>()
                .ForMember(d => d.GenreIds, opt => opt.MapFrom(s => s.BookGenres.Select(bg => bg.GenreId)))
                .ReverseMap()
                .ForMember(d => d.BookGenres, opt => opt.MapFrom(s => s.GenreIds));
            
        }

        
    }
}
