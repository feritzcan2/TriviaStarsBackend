using Api.Service.GameHub.Data.Player;
using Api.Service.GameHub.DeckManagement;

namespace Api.Service.GameHub.PlayerManagement
{
    public class PlayerCreator
    {
        private readonly DeckManager _deckManager;

        public PlayerCreator(DeckManager deckManager)
        {
            _deckManager = deckManager;
        }
        public async Task<GamePlayer> CreateStartingPlayer(string name, Guid connectionId)
        {
            var deck = await _deckManager.GenerateStartingDeck();

            return new GamePlayer
            {
                ConnectionId = connectionId,
                UserId = name,
                Deck = deck
            };

        }
    }
}
