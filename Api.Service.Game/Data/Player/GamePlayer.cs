namespace Api.Service.GameHub.Data.Player
{
    public class GamePlayer
    {
        public string Name { get; set; }
        public Deck Deck { get; set; }
        public Guid ConnectionId { get; set; }
        public int Energy { get; set; }
    }
}
