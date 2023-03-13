using Api.Service.GameHub.Data.Game;
using Api.Service.GameHub.Data.Player;
using Api.Service.GameHub.PlayerManagement;
using Api.Service.GameHub.ScheduledServices;
using GameEngine.Entities.Data.GameEvent;
using GameEngine.Entities.Hubs.Receiver;

namespace Api.Service.GameHub.Utils
{
    public class GameRoomManager
    {
        private readonly PlayerCreator _playerCreator;
        private readonly GameRoundFinalizerService _finalizerService;
        public Dictionary<string, GameRoom> GameRooms { get; set; }
        public GameRoomManager(PlayerCreator playerCreator, GameRoundFinalizerService finalizerService)
        {
            _finalizerService = finalizerService;
            GameRooms = new Dictionary<string, GameRoom>();
            _playerCreator = playerCreator;
        }

        public async Task<(GameRoom room, GamePlayer player)> EnterRoom(JoinEventData joinEventData,NetworkPlayer networkPlayer)
        {
            GameRooms.TryGetValue(joinEventData.RoomName, out var room);
            if (room == null)
            {
                room = new GameRoom(joinEventData.IsSinglePlayer);
                _finalizerService.AddRoom(room);
                GameRooms.Add(joinEventData.RoomName, room);
            }

            var player = await _playerCreator.CreateStartingPlayer(joinEventData.Username, networkPlayer);
            room.AddPlayer(player);
            return (room, player);
        }

        public void RemoveRoom(string roomId)
        {
            GameRooms.Remove(roomId);
        }

    }
}
