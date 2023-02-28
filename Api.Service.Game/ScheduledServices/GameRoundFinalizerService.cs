using _0_TriviaStars.Scripts.Shared.GameEngine.Entities.Data.Response;
using Api.Service.GameHub.Data.Game;
using Api.Service.GameHub.DeckManagement;

namespace Api.Service.GameHub.ScheduledServices
{
    public class GameRoundFinalizerService
    {
        private IList<GameRoom> _rooms;
        private readonly DeckManager _deckManager;

        public GameRoundFinalizerService(DeckManager deckManager)
        {
            _rooms = new List<GameRoom>();
            _deckManager = deckManager;
        }

        public async Task ProcessGames()
        {
            foreach (var room in _rooms)
            {
                if (room.Round.RoundStartDate.HasValue)
                {
                    if (room.Round.CheckTurnsEnded() || DateTime.UtcNow.Subtract(room.Round.RoundStartDate.Value).TotalSeconds >= 40)
                    {
                        await NextTurn(room);
                    }
                }
            }
        }
        private async Task NextTurn(GameRoom gameRoom)
        {
            var response = new NextTurnResponse();
            gameRoom.Round.SetNewRound();
            response.Round = gameRoom.Round.Round;
            foreach (var player in gameRoom.Players)
            {
                var cards = await _deckManager.FillCards(player);
                response.Cards = cards;
                player.Broadcaster.OnNextRound(response);
            }
        }

        public void AddRoom(GameRoom value)
        {
            _rooms.Add(value);
        }
    }
}
