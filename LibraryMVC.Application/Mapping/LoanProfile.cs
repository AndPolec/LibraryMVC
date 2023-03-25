using AutoMapper;
using LibraryMVC.Application.ViewModels.BorrowingCart;
using LibraryMVC.Application.ViewModels.Loan;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Mapping
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<Loan, LoanForListVm>()
                .ForMember(d => d.NumberOfBorrowedBooks, opt => opt.MapFrom(s => s.Books.Count))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.Name));

            CreateMap<Loan, LoanDetailsVm>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.Name))
                .ForMember(d => d.IsCheckedOut, opt => opt.MapFrom(s => s.CheckOutRecord != null))
                .ForMember(d => d.CheckOutDate, opt => opt.MapFrom(s => s.CheckOutRecord.Date))
                .ForMember(d => d.IsReturned, opt => opt.MapFrom(s => s.ReturnRecord != null))
                .ForMember(d => d.ReturnDate, opt => opt.MapFrom(s => s.ReturnRecord.Date));

            CreateMap<Loan, LoanForConfirmCheckOutListVm>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.Name))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.LibraryUser.FirstName + " " + s.LibraryUser.LastName))
                .ForMember(d => d.BorrowedBooks, opt => opt.MapFrom(s => s.Books.Select(
                    b => b.Title + " | " + b.Author.FirstName + " " + b.Author.LastName + " | ISBN: " + b.ISBN)));
;

        }
    }
}
