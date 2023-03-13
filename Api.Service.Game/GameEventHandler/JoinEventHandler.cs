using _0_TriviaStars.Scripts.Shared.GameEngine.Entities.Data.Response;
using Api.Service.GameHub.Data.Game;
using Api.Service.GameHub.Data.Player;
using Api.Service.GameHub.DeckManagement;
using Api.Service.GameHub.Utils;
using GameEngine.Entities.Data.GameEvent;

namespace Api.Service.GameHub.GameEventHandler;

public class JoinGameEventHandler:AbstractGameEventHandler<JoinEventData,JoinEventResponseData>
{
    private readonly GameRoomManager _roomManager;
    private readonly DeckManager _deckManager;
    private readonly QuestionManager _questionManager;

    public JoinGameEventHandler()
    {
        
    }
    public JoinGameEventHandler(GameRoomManager roomManager, 
        DeckManager deckManager, QuestionManager questionManager)
    {
        _questionManager = questionManager;
        _deckManager = deckManager;
        _roomManager = roomManager;
    }
    public async Task<(GameRoom gameRoom, GamePlayer player)> JoinAsync(GameEventData gameEventData,NetworkPlayer networkPlayer)
    {
        var joinEventData = (JoinEventData)gameEventData;
        var (gameRoom, player) = await _roomManager.EnterRoom(joinEventData,networkPlayer);
        if (gameRoom.Players.Count == 2 || (gameRoom.Players.Count == 1 && joinEventData.IsSinglePlayer))
        {
            await StartGame(gameRoom);
        }
        return (gameRoom, player);
    }

    private async Task StartGame(GameRoom gameRoom )
    {
        var resp = new StartGameResponse();
        gameRoom.Round.SetNewRound();
        foreach (var currPlayer in gameRoom.Players)
        {
            var deckDto = await _deckManager.ToDeckDto(currPlayer.Deck);
            resp.Deck = deckDto;
            resp.Energy = 7;
            Console.WriteLine("Will start game ");
            currPlayer.NetworkPlayer.Broadcaster.OnGameStart(resp);
        }
    }


    public override JoinEventResponseData HandleAsync(JoinEventData joinEventData, NetworkPlayer networkPlayer)
    {
        throw new NotImplementedException();
    }
}