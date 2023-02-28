using Api.Service.GameHub.Data.Game;
using Api.Service.GameHub.Data.Player;
using Api.Service.GameHub.PlayerManagement;
using Api.Service.GameHub.ScheduledServices;
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

        public async Task<(GameRoom room, GamePlayer player)> EnterRoom(string roomId, string username,
            Guid connectionId, bool singlePlayer, IGamingHubReceiver gamingHubReceiver)
        {
            GameRooms.TryGetValue(roomId, out var room);
            if (room == null)
            {
                room = new GameRoom(singlePlayer);
                _finalizerService.AddRoom(room);
                GameRooms.Add(roomId, room);
            }

            var player = await _playerCreator.CreateStartingPlayer(username, connectionId, gamingHubReceiver);
            room.AddPlayer(player);
            return (room, player);
        }

        public void RemoveRoom(string roomId)
        {
            GameRooms.Remove(roomId);
        }

    }
}
