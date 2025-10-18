using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extentions
{
    public class ConnectionCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _connectionTimeout = TimeSpan.FromMinutes(10);

        public ConnectionCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupStaleConnections();
                }
                catch (Exception ex)
                {
                    // Log error (add your logging here)
                    Console.WriteLine($"Error cleaning connections: {ex.Message}");
                }

                await Task.Delay(_cleanupInterval, stoppingToken);
            }
        }

        private async Task CleanupStaleConnections()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var cutoffTime = DateTime.UtcNow.Subtract(_connectionTimeout);

            var staleConnections = context.userConnections
                .Where(uc => uc.ConnectedAt < cutoffTime)
                .ToList();

            if (staleConnections.Any())
            {
                context.userConnections.RemoveRange(staleConnections);
                await context.SaveChangesAsync();

                Console.WriteLine($"Cleaned up {staleConnections.Count} stale connections");
            }
        }
    }

}
