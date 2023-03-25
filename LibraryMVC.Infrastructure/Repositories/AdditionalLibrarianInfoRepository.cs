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

        public int AddNew(AdditionalLibrarianInfo librarianInfo)
        {
            throw new NotImplementedException();
        }

        public AdditionalLibrarianInfo GetInfoByIdentityUserId(string id)
        {
            var info = _context.AdditionalLibrarianInfo.FirstOrDefault(i => i.LibraryUser.IdentityUserId == id);
            return info;
        }
    }
}
