
namespace Homesplash.Logos;

internal class LogoWorker(ILogoQueue queue, IServiceProvider serviceProvider) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            if (queue.TryDequeue(out string? url) && url != null) {
                using var scope = serviceProvider.CreateScope();
                var logoService = scope.ServiceProvider.GetRequiredService<ILogoService>();
                await logoService.FetchAndSaveLogo(url);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}