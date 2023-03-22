using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class AdditionalLibrarianInfoRepository : IAdditionalLibrarianInfoRepository
    {
        private readonly Context _context;
        public AdditionalLibrarianInfoRepository(Context context)
        {
            _context = context;
        }

        public int AddLibrarian(AdditionalLibrarianInfo librarian)
        {
            throw new NotImplementedException();
        }
    }
}
