using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IGenreRepository, GenreRepository>();
            services.AddTransient<IPublisherRepository, PublisherRepository>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<ILibraryUserRepository, LibraryUserRepository>();
            services.AddTransient<IBorrowingCartRepository, BorrowingCartRepository>();
            services.AddTransient<ILoanRepository, LoanRepository>();
            services.AddTransient<IAdditionalLibrarianInfoRepository, AdditionalLibrarianInfoRepository>();
            services.AddTransient<IReturnRecordRepository, ReturnRecordRepository>();
            services.AddTransient<IUserTypeRepository, UserTypeRepository>();
            return services;
        }
    }
}
