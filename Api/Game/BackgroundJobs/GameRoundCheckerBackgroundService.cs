using Api.Service.GameHub.ScheduledServices;

namespace Api.Game.BackgroundJobs
{
    public class GameRoundCheckerBackgroundService : AbstractBackgroundWorker
    {
        private readonly GameRoundFinalizerService _finalizerService;

        public GameRoundCheckerBackgroundService(GameRoundFinalizerService finalizerService)
        {
            _finalizerService = finalizerService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await _finalizerService.ProcessGames();
                await Task.Delay(250, stoppingToken);

            }
        }
    }
}
