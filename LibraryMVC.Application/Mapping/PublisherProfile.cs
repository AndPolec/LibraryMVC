using AutoMapper;
using LibraryMVC.Application.ViewModels.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Mapping
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<LibraryMVC.Domain.Model.Publisher, PublisherForListVm>();
        }
    }
}
