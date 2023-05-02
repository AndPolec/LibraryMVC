using LibraryMVC.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Services
{
    public class OverduePenaltyUpdateBackgroundService : BackgroundService
    {
        private readonly ILoanService _loanService;

        public OverduePenaltyUpdateBackgroundService(ILoanService loanService)
        {
            _loanService = loanService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _loanService.UpdateOverduePenaltyAndStatusForAllLoans();

                var now = DateTime.Now;
                var nextUpdateDate = new DateTime(now.Year,now.Month,now.Day, 0, 0, 1).AddDays(1);
                TimeSpan timeToNextUpdate = nextUpdateDate - now;
                await Task.Delay(timeToNextUpdate, stoppingToken);
            }
        }
    }
}
