using Api.Service.GameHub.Contracts;
using Api.Service.Repository;

namespace Api.ServiceExtensions
{
    public static class RepositoryServiceExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IQuestionRepository, QuestionsRepository>();
        }
    }
}
