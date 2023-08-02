using AutoMapper;
using LibraryMVC.Application.ViewModels.Genre;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Mapping
{
    
 public class GenreProfile : Profile
    { 
        public GenreProfile()
        {
            CreateMap<Genre, GenreForListVm>();
            CreateMap<GenreForListVm, Genre>();

            
        }
    }
    
}
