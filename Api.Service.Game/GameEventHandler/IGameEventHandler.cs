using Api.Service.GameHub.Data.Player;
using GameEngine.Entities.Data.GameEvent;

namespace Api.Service.GameHub.GameEventHandler;

public interface IGameEventHandler<in TIn, out TOut> where TIn:GameEventData where TOut:GameEventResponseData
{
    TOut HandleAsync(TIn joinEventData, NetworkPlayer networkPlayer);
}
public interface IGameEventHandler:IGameEventHandler<GameEventData,GameEventResponseData>
{
}