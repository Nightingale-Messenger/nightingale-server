using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nightingale.Infrastructure.Data;

namespace Nightingale.API.Services
{
    public class RefreshTokenCleanService : IHostedService
    {
        private System.Threading.Timer _timer;
        private readonly IServiceScopeFactory scopeFactory;

        public RefreshTokenCleanService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Action, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void Action(object state)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appContext = scope.ServiceProvider.GetRequiredService<NightingaleContext>();
                var tokens = appContext.RefreshTokens
                    .Where(token => token.ExpirationDate <= DateTime.Now);
                appContext.RefreshTokens.RemoveRange(tokens);
                await appContext.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}