using Api.Service.GameHub.Data;
using Api.Service.GameHub.Data.Player;
using Api.Service.GameHub.PlayerManagement;

namespace Api.Service.GameHub.Utils
{
    public class GameRoomManager
    {
        private readonly PlayerCreator _playerCreator;
        public Dictionary<string, GameRoom> GameRooms { get; set; }
        public GameRoomManager(PlayerCreator playerCreator)
        {
            GameRooms = new Dictionary<string, GameRoom>();
            _playerCreator = playerCreator;
        }

        public async Task<(GameRoom room, GamePlayer player)> EnterRoom(string roomId, string username,
            Guid connectionId)
        {
            GameRooms.TryGetValue(roomId, out var room);
            if (room == null)
            {
                room = new GameRoom();
                GameRooms.Add(roomId, room);
            }

            var player = await _playerCreator.CreateStartingPlayer(username, connectionId);
            room.AddPlayer(player);
            return (room, player);
        }

        public void RemoveRoom(string roomId)
        {
            GameRooms.Remove(roomId);
        }

    }
}
