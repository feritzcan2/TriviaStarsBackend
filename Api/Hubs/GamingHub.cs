using _0_TriviaStars.Scripts.Shared.GameEngine.Entities.Data.Response;
using Api.Service.GameHub.Data;
using Api.Service.GameHub.Data.Player;
using Api.Service.GameHub.DeckManagement;
using Api.Service.GameHub.Utils;
using GameEngine.Entities.Data.GameEvent;
using GameEngine.Entities.Hubs.Receiver;
using MagicOnion.Server.Hubs;

namespace Api.Hubs
{
    public class GamingHub : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        // this class is instantiated per connected so fields are cache area of connection.
        IGroup room;
        IInMemoryStorage<GamePlayer> storage;
        private GameRoom gameRoom;
        private GamePlayer player;
        private readonly GameRoomManager _roomManager;
        private readonly DeckManager _deckManager;
        private readonly QuestionManager _questionManager;

        public GamingHub(GameRoomManager roomManager, DeckManager deckManager, QuestionManager questionManager)
        {

            _questionManager = questionManager;
            _deckManager = deckManager;
            _roomManager = roomManager;
        }

        protected override ValueTask OnDisconnected()
        {
            if (room == null) return ValueTask.CompletedTask;
            _roomManager.RemoveRoom(room.GroupName);
            Broadcast(room).OnGameDisconnected();
            return base.OnDisconnected();
        }

        public async Task JoinAsync(string roomName, string userName)
        {
            (gameRoom, player) = await _roomManager.EnterRoom(roomName, userName, ConnectionId);
            (room, storage) = await Group.AddAsync(roomName, player);

            if (gameRoom.Players.Count == 2)
            {
                await StartGame();
            }
            //  return storage.AllValues.ToArray();
        }

        private async Task StartGame()
        {
            var rresp = new StartGameResponse();
            gameRoom.Round.SetNewRound();
            foreach (var currPlayer in gameRoom.Players)
            {
                var deckDto = await _deckManager.ToDeckDto(currPlayer.Deck);
                rresp.Deck = deckDto;
                rresp.Energy = 7;
                BroadcastTo(room, currPlayer.ConnectionId).OnGameStart(rresp);
            }
        }

        public async Task<OpenQuestionResponse> OpenCard(string id)
        {
            var playerCard = player.Deck.Cards.FirstOrDefault(x => x.Id == id);
            if (playerCard == null) return null;
            var question = await _questionManager.GetQuestion(playerCard.QuestionId);
            if (player.Energy < question.Energy) return null;

            player.Energy -= question.Energy;

            var dto = await _questionManager.GetQuestionDto(playerCard.QuestionId);
            return new OpenQuestionResponse
            {
                Question = dto,
                Energy = player.Energy
            };
        }

        public async Task EndTurn()
        {

            gameRoom.Round.PlayedPlayerGuids.Add(ConnectionId);
            if (gameRoom.Round.PlayedPlayerGuids.Count == 2)
            {
                await NextTurn();
            }
        }

        private async Task NextTurn()
        {
            var response = new NextTurnResponse();
            gameRoom.Round.SetNewRound();
            response.Round = gameRoom.Round.Round;
            foreach (var player in gameRoom.Players)
            {
                var cards = await _deckManager.FillCards(player);
                response.Cards = cards;
                BroadcastTo(room, player.ConnectionId).OnNextRound(response);
            }
        }

        public async Task<QuestionAnsweredResponse> AnswerQuestion(string cardId, string answer, int lane)
        {
            var response = new QuestionAnsweredResponse();
            Console.WriteLine("PlayerId " + player.ConnectionId.ToString());
            var playerCard = player.Deck.Cards.FirstOrDefault(x => x.Id == cardId);
            var question = await _questionManager.GetQuestionDto(playerCard.QuestionId);
            if (question.CorrectAnswer == answer)
            {
                response.Succeed = true;
            }

            player.Deck.Cards.Remove(playerCard);
            BroadcastExceptSelf(room).OnEnemyMove(new EnemyCardMove { Energy = question.Energy, LaneNumber = lane, Succeed = response.Succeed ? true : null });

            return response;
        }

        public async Task LeaveAsync()
        {
        }


        // You can hook OnConnecting/OnDisconnected by override.

    }

}
