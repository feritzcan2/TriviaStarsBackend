using Api.Service;
using Api.Service.GameHub.ScheduledServices;

namespace Api.Game.BackgroundJobs
{
    public class OpenTriviaDbFetcherBackgroundService : AbstractBackgroundWorker
    {
        private readonly OpenTriviaDbService _finalizerService;

        public OpenTriviaDbFetcherBackgroundService(OpenTriviaDbService finalizerService)
        {
            _finalizerService = finalizerService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    await _finalizerService.SyncQuestions();
                    var random = new Random();

                    await Task.Delay(random.Next(60000, 400000), stoppingToken);

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
