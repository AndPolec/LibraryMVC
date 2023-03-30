using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class ReturnRecordRepository : IReturnRecordRepository
    {
        private readonly Context _context;

        public ReturnRecordRepository(Context context)
        {
            _context = context;
        }

        public ReturnRecord GetReturnRecordById(int id)
        {
            var returnRecord = _context.ReturnRecords
                .Include(r => r.AdditionalLibrarianInfo).ThenInclude(l => l.LibraryUser)
                .FirstOrDefault(r => r.Id == id);
            return returnRecord;
        }
    }
}
