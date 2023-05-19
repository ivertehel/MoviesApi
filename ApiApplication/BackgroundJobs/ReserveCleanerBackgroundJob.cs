using ApiApplication.Database;
using ApiApplication.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.BackgroundJobs
{
    public class ReserveCleanerBackgroundJob : AbstractBackgroundJob
    {
        private readonly IServiceScopeFactory _scopeFactory;

        protected override TimeSpan Delay => TimeSpan.FromSeconds(1);

        public ReserveCleanerBackgroundJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public override async Task RunRecurringJob(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CinemaContext>();
                var reservationsRepository = new ReservationsRepository(context);
                await reservationsRepository.RemoveOutdatedReservationsAsync(cancellationToken);
            }
        }
    }
}
