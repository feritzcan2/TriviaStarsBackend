using Api.Service.GameHub.Data.Player;

namespace Api.Service.GameHub.Data
{
    public class GameRoom
    {
        public GameRoundInfo Round = new GameRoundInfo();

        public IList<GamePlayer> Players { get; set; } = new List<GamePlayer>();


        public void AddPlayer(GamePlayer gamePlayer)
        {
            Players.Add(gamePlayer);
        }
    }

    public class GameRoundInfo
    {
        public DateTime? RoundStartDate { get; set; }
        public int Round { get; set; }

        public HashSet<Guid> PlayedPlayerGuids { get; set; } = new HashSet<Guid>();

        public void SetNewRound()
        {
            PlayedPlayerGuids.Clear();
            Round++;
            RoundStartDate = DateTime.UtcNow;
        }
    }
}
