using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Nightingale.Infrastructure.Data;

namespace Nightingale.API.Services
{
    public class RefreshTokenCleanService : IHostedService
    {
        private System.Threading.Timer _timer;
        private readonly NightingaleContext _appContext;

        public RefreshTokenCleanService(NightingaleContext appContext)
        {
            _appContext = appContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Action, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void Action(object state)
        {
            var tokens = _appContext.RefreshTokens
                .Where(token => token.ExpirationDate <= DateTime.Now);
            _appContext.RefreshTokens.RemoveRange(tokens);
            await _appContext.SaveChangesAsync();
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