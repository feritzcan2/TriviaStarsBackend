namespace Api.Game.BackgroundJobs
{
    public abstract class AbstractBackgroundWorker : BackgroundService
    {

        public AbstractBackgroundWorker()
        {
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }

    }
}