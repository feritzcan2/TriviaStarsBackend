using Api.Service.GameHub.DeckManagement;
using Api.Service.GameHub.GameEventHandler;
using Api.Service.GameHub.PlayerManagement;
using Api.Service.GameHub.ScheduledServices;
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
            services.AddSingleton<GameRoundFinalizerService>();
            services.AddSingleton<QuestionManager>();
            services.AddSingleton<GameHubService>();
            services.AddSingleton<GameEventHandlerFactory>();
            services.AddSingleton<JoinGameEventHandler>();
            
        }
    }
}
