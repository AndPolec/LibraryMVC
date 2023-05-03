using LibraryMVC.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceScopeFactory _scopeFactory;

        public OverduePenaltyUpdateBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var loanService = scope.ServiceProvider.GetService<ILoanService>();
                    loanService.UpdateOverduePenaltyAndStatusForAllLoans();
                }

                var now = DateTime.Now;
                var nextUpdateDate = new DateTime(now.Year,now.Month,now.Day, 0, 0, 1).AddDays(1);
                TimeSpan timeToNextUpdate = nextUpdateDate - now;
                await Task.Delay(timeToNextUpdate, stoppingToken);
            }
        }
    }
}
