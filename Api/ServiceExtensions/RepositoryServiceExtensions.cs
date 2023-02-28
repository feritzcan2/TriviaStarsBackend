using Api.Game.BackgroundJobs;
using Api.Service.GameHub.Contracts;
using Api.Service.Repository;

namespace Api.ServiceExtensions
{
    public static class RepositoryServiceExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {

            services.AddHostedService<GameRoundCheckerBackgroundService>();
            services.AddSingleton<IQuestionRepository, QuestionsRepository>();
        }
    }
}
