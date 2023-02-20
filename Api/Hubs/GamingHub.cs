using _0_TriviaStars.Scripts.Shared.GameEngine.Entities.Data.Response;
using Api.Service.GameHub.Data.Game;
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

        public async ValueTask JoinAsync(string roomName, string userName, bool singlePlayer)
        {
            try
            {
                Console.WriteLine("Join asynccc");
                (gameRoom, player) = await _roomManager.EnterRoom(roomName, userName, ConnectionId, singlePlayer);
                (room, storage) = await Group.AddAsync(roomName, player);

                if (gameRoom.Players.Count == 2 || (gameRoom.Players.Count == 1 && singlePlayer))
                {
                    await StartGame();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in join " + e.ToString());
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
                Console.WriteLine("Will start game ");
                BroadcastTo(room, currPlayer.ConnectionId).OnGameStart(rresp);
            }
        }

        public async ValueTask<OpenQuestionResponse> OpenCard(string id)
        {
            var roundPlayData = gameRoom.Round.RoundPlayData[player.UserId];
            if (roundPlayData.TurnEnded) return null;

            var playerCard = player.Deck.Cards.FirstOrDefault(x => x.Id == id);
            if (playerCard == null) return null;
            var question = await _questionManager.GetQuestion(playerCard.QuestionId);
            var round = gameRoom.Round.Round;
            if (round == 1 || round == 2)
            {
                if (question.Difficulty != "easy" || roundPlayData.GetCardCount() >= 1)
                    return null;
            }
            else if (round == 3 || round == 4)
            {
                if (question.Difficulty == "hard" || roundPlayData.GetCardCount() >= 2) return null;
            }
            else if (round == 5 || round == 6)
            {
                if (roundPlayData.GetCardCount() >= 2) return null;
            }

            var dto = await _questionManager.GetQuestionDto(playerCard.QuestionId);
            return new OpenQuestionResponse
            {
                Question = dto
            };
        }

        public async ValueTask EndTurn()
        {
            var roundPlayData = gameRoom.Round.RoundPlayData[player.UserId];
            roundPlayData.TurnEnded = true;
            if (gameRoom.Round.CheckTurnsEnded() || gameRoom.SinglePlayer)
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

        public async ValueTask<QuestionAnsweredResponse> AnswerQuestion(string cardId, string answer, int lane)
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
            BroadcastExceptSelf(room).OnEnemyMove(new EnemyCardMove { Energy = question.Energy, LaneNumber = lane, Succeed = response.Succeed });

            return response;
        }

        public async ValueTask LeaveAsync()
        {
        }


        // You can hook OnConnecting/OnDisconnected by override.

    }

}
