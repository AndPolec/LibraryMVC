using AutoMapper;
using FluentValidation;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.Mapping;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.Book;
using LibraryMVC.Application.ViewModels.ReturnRecord;
using LibraryMVC.Application.ViewModels.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IBookService, BookService>();
            services.AddTransient<ILibraryUserService, LibraryUserService>();
            services.AddTransient<ILoanService, LoanService>();

            services.AddTransient<IValidator<NewBookVm>, NewBookVmValidator>();
            services.AddTransient<IValidator<NewLibraryUserVm>, NewLibraryUserVmValidator>();
            services.AddTransient<IValidator<NewReturnRecordVm>, NewReturnRecordVmValidator>();

            return services;
        }
    }
}
