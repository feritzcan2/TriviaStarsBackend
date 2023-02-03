using Api.Service.GameHub.DeckManagement;
using Api.Service.GameHub.PlayerManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Service.GameHub.Utils
{
    public static class GameServiceExtensions
    {
        public static void AddGameEngine(this IServiceCollection services)
        {
            services.AddSingleton<GameRoomManager>();
            services.AddSingleton<DeckManager>();
            services.AddSingleton<PlayerCreator>();
            services.AddSingleton<QuestionManager>();
        }
    }
}
