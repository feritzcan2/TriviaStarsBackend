using Api.Service.GameHub.Data.Player;
using Api.Service.GameHub.Data.Room;

namespace Api.Service.GameHub.Data.Game
{
    public class GameRoom
    {
        public GameRoundInfo Round = new GameRoundInfo();

        public GameRoom(bool singlePlayer)
        {
            SinglePlayer = singlePlayer;
        }

        public bool SinglePlayer { get; set; }

        public IList<GamePlayer> Players { get; set; } = new List<GamePlayer>();


        public void AddPlayer(GamePlayer gamePlayer)
        {
            Round.RoundPlayData.Add(gamePlayer.UserId, new PlayerRoundPlayData());
            Players.Add(gamePlayer);
        }
    }


}
