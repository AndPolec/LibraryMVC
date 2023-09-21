using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class GlobalLoanSettingsRepository : IGlobalLoanSettingsRepository
    {
        private readonly Context _context;

        public GlobalLoanSettingsRepository(Context context)
        {
            _context = context;
        }

        public GlobalLoanSettings GetSettings()
        {
            var settings = _context.GlobalLoanSettings.FirstOrDefault();
            return settings;
        }

        public void UpdateSettings(GlobalLoanSettings settings)
        {
            _context.GlobalLoanSettings.Update(settings);
            _context.SaveChanges();
        }
    }
}
