using Api.Service.GameHub.Data.Player;
using Api.Service.GameHub.DeckManagement;
using GameEngine.Entities.Hubs.Receiver;

namespace Api.Service.GameHub.PlayerManagement
{
    public class PlayerCreator
    {
        private readonly DeckManager _deckManager;

        public PlayerCreator(DeckManager deckManager)
        {
            _deckManager = deckManager;
        }
        public async Task<GamePlayer> CreateStartingPlayer(string name, Guid connectionId,
            IGamingHubReceiver gamingHubReceiver)
        {
            var deck = await _deckManager.GenerateStartingDeck();

            return new GamePlayer
            {
                Broadcaster = gamingHubReceiver,
                ConnectionId = connectionId,
                UserId = name,
                Deck = deck
            };

        }
    }
}
