using GameEngine.Entities.Data.GameEvent;

namespace Api.Service.GameHub.GameEventHandler;


public class GameEventHandlerFactory
{
    public Dictionary<Type, IGameEventHandler> Handlers;
    public GameEventHandlerFactory(JoinGameEventHandler handler)
    {
        Handlers = new Dictionary<Type, IGameEventHandler>();
        Handlers.Add(typeof(JoinEventData), handler);
    }
}