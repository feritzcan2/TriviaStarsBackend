using _0_TriviaStars.Scripts.Shared.GameEngine.Entities.Data.Response;
using Api.Service.GameHub;
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
        private readonly GameHubService _service;

        public GamingHub(GameRoomManager roomManager, GameHubService service)
        {
            _service = service;
            _roomManager = roomManager;
        }

        protected override ValueTask OnDisconnected()
        {
            if (room == null) return ValueTask.CompletedTask;
            _roomManager.RemoveRoom(room.GroupName);
            Broadcast(room).OnGameDisconnected();
            return base.OnDisconnected();
        }

        public async ValueTask JoinAsync(JoinEventData joinEventData)
        {
            try
            {
                (room, storage) = await Group.AddAsync(joinEventData.RoomName, player);
                var broadCaster = room.CreateBroadcasterTo<IGamingHubReceiver>(ConnectionId);
                var exceptSelf = room.CreateBroadcasterExcept<IGamingHubReceiver>(ConnectionId);

                var networkPlayer = new NetworkPlayer
                {
                    ConnectionId = ConnectionId,
                    Broadcaster = broadCaster,
                    BroadcasterExceptSelf = exceptSelf
                };

                (gameRoom, player) = await _service.JoinAsync(joinEventData, networkPlayer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in join " + e.ToString());
            }
        }
      
        public async ValueTask<OpenQuestionResponse> OpenCard(string id)
        {
            return await _service.OpenCard(gameRoom, ConnectionId, id);
        }

        public async ValueTask EndTurn()
        {
            await _service.EndTurn(gameRoom,ConnectionId);
        }

        public async ValueTask<QuestionAnsweredResponse> AnswerQuestion(string cardId, string answer, int lane)
        {
            return await _service.AnswerQuestion(gameRoom, ConnectionId, cardId, answer, lane);
        }

        public async ValueTask LeaveAsync()
        {
        }


        // You can hook OnConnecting/OnDisconnected by override.

    }

}
