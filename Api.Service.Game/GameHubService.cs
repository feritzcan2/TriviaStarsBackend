using _0_TriviaStars.Scripts.Shared.GameEngine.Entities.Data.Response;
using Api.Service.GameHub.Data.Game;
using Api.Service.GameHub.Data.Player;
using Api.Service.GameHub.DeckManagement;
using Api.Service.GameHub.GameEventHandler;
using Api.Service.GameHub.Utils;
using GameEngine.Entities.Data.GameEvent;

namespace Api.Service.GameHub
{
    public class GameHubService
    {
        private readonly GameRoomManager _roomManager;
        private readonly DeckManager _deckManager;
        private readonly QuestionManager _questionManager;
        private readonly Dictionary<Type, IGameEventHandler> _eventHandlers;

        public GameHubService(GameRoomManager roomManager, 
            DeckManager deckManager, QuestionManager questionManager,GameEventHandlerFactory gameEventHandlerFactory)
        {
            _questionManager = questionManager;
            _eventHandlers = gameEventHandlerFactory.Handlers;
            _deckManager = deckManager;
            _roomManager = roomManager;
        }
        
        public async Task<(GameRoom gameRoom, GamePlayer player)> JoinAsync(JoinEventData joinEventData,NetworkPlayer networkPlayer)
        {
           var resp  =  _eventHandlers[joinEventData.GetType()].HandleAsync(joinEventData,networkPlayer);
        }   
        
        public async Task StartGame(GameRoom gameRoom )
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
        
        public async ValueTask<OpenQuestionResponse> OpenCard(
            GameRoom gameRoom,Guid connectionId, string id)
        {
            var roundPlayData = gameRoom.Round.RoundPlayData[connectionId];
            if (roundPlayData.TurnEnded) return null;

            var player = gameRoom.GetPlayer(connectionId);
            var playerCard = player.Deck.Cards.FirstOrDefault(x => x.Id == id);
            if (playerCard == null) return null;
            var round = gameRoom.Round.Round;
            if (round == 1 || round == 2)
            {
                if (roundPlayData.PlayedCardCount >= 1)
                    return null;
            }
            else
            {
                if (roundPlayData.PlayedCardCount >= 2) return null;
            }

            roundPlayData.PlayedCardCount++;
            var dto = await _questionManager.GetQuestionDto(playerCard.QuestionId);
            return new OpenQuestionResponse
            {
                Question = dto
            };
        }
        
        public async Task EndTurn(GameRoom gameRoom, Guid connectionId )
        {
            var roundPlayData = gameRoom.Round.RoundPlayData[connectionId];
            roundPlayData.TurnEnded = true;
        }
        
        public async Task<QuestionAnsweredResponse> AnswerQuestion(GameRoom gameRoom,Guid connectionId,string cardId, string answer, int lane)
        {
            var response = new QuestionAnsweredResponse();
            var player = gameRoom.GetPlayer(connectionId);
            var playerCard = player.Deck.Cards.FirstOrDefault(x => x.Id == cardId);
            var question = await _questionManager.GetQuestionDto(playerCard.QuestionId);
            if (question.CorrectAnswer == answer)
            {
                response.Succeed = true;
            }

            player.Deck.Cards.Remove(playerCard);
            player.NetworkPlayer.BroadcasterExceptSelf.OnEnemyMove(new EnemyCardMove { Energy = question.Energy, LaneNumber = lane, Succeed = response.Succeed });

            return response;
        }
        
    }
}