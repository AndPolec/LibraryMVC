using LibraryMVC.Domain.Model;

namespace LibraryMVC.Domain.Interfaces
{
    public interface ILoanRepository
    {
        int AddLoan(Loan loan);
        void DeleteLoan(int loanId);
        IQueryable<Loan> GetAllLoans();
        Loan GetLoanById(int loanId);
        void UpdateLoan(Loan loan);
    }
}