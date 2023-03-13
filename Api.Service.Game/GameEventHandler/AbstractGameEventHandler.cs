using Api.Service.GameHub.Data.Player;
using GameEngine.Entities.Data.GameEvent;

namespace Api.Service.GameHub.GameEventHandler;

public abstract class AbstractGameEventHandler<Tin, Tout> :IGameEventHandler,IGameEventHandler<Tin, Tout>  
    where Tin:GameEventData where Tout:GameEventResponseData
{
    public GameEventResponseData HandleAsync(GameEventData joinEventData, NetworkPlayer networkPlayer)
    {
        return  (Tout) this.HandleAsync((Tin)joinEventData, networkPlayer);
    }

    public abstract Tout HandleAsync(Tin joinEventData, NetworkPlayer networkPlayer);
}