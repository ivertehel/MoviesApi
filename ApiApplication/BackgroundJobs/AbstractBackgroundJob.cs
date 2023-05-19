using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.BackgroundJobs
{
    public abstract class AbstractBackgroundJob : BackgroundService
    {
        protected abstract TimeSpan Delay { get; }

        protected AbstractBackgroundJob()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await RunRecurringJob(cancellationToken);

                await Task.Delay(Delay, cancellationToken);
            }
        }

        public abstract Task RunRecurringJob(CancellationToken cancellationToken);
    }
}
