using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Interfaces
{
    public interface IUserTypeRepository
    {
        IQueryable<UserType> GetAll();
    }
}
