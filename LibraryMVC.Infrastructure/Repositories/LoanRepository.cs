using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly Context _context;

        public LoanRepository(Context context)
        {
            _context = context;
        }

        public int AddLoan(Loan loan)
        {
            _context.Loans.Add(loan);
            _context.SaveChanges();
            return loan.Id;
        }

        public Loan GetLoanById(int loanId)
        {
            var loan = _context.Loans
                .Include(l => l.Status)
                .Include(l => l.CheckOutRecord)
                .Include(l => l.ReturnRecord)
                .Include(l => l.Books).ThenInclude(b => b.Author)
                .Include(l => l.Books).ThenInclude(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Include(l => l.LibraryUser).ThenInclude(lu => lu.additionalLibrarianInfo)
                .FirstOrDefault(l => l.Id == loanId);
            return loan;
        }

        public IQueryable<Loan> GetAllLoans()
        {
            var loan = _context.Loans
                .Include(l => l.Books)
                .Include(l => l.Status)
                .Include(l => l.ReturnRecord)
                .Include(l => l.LibraryUser)
                .AsNoTracking();
            return loan;
        }

        public void UpdateLoan(Loan loan)
        {
            _context.Loans.Update(loan);
            _context.SaveChanges();
        }

        public void UpdatePenaltyAndStatusInLoans(ICollection<Loan> loans)
        {
            foreach (var loan in loans)
            {
                _context.Entry(loan).Property(l => l.StatusId).IsModified = true;
                _context.Entry(loan.ReturnRecord).Property(rr => rr.OverduePenalty).IsModified = true;
                _context.Entry(loan.ReturnRecord).Property(rr => rr.TotalPenalty).IsModified = true;
            }
            _context.SaveChanges();
        }

        public void DeleteLoan(int loanId)
        {
            var loan = _context.Loans.Find(loanId);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
                _context.SaveChanges();
            }
        }
    }
}
