using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Infrastructure.Repositories
{
    public class LoanRepository
    {
        private readonly Context _context;

        public LoanRepository(Context context)
        {
            context = _context;
        }

        public int AddLoan(Loan loan)
        {
            _context.Loans.Add(loan);
            _context.SaveChanges();
            return loan.Id;
        }

        public Loan GetLoanById(int loanId)
        {
            var loan = _context.Loans.FirstOrDefault(l => l.Id == loanId);
            return loan;
        }

        public IQueryable<Loan> GetAllLoans()
        {
            var loan = _context.Loans;
            return loan;
        }

        public void UpdateLoan(Loan loan)
        {
            _context.Loans.Update(loan);
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
