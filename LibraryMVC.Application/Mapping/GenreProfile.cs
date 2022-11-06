using AutoMapper;
using LibraryMVC.Application.ViewModels.Genre;
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
            CreateMap<LibraryMVC.Domain.Model.Genre, GenreForListVm>();
        }
    }
    
}
