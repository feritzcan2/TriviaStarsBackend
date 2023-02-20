namespace Api.Service.GameHub.Data.Player
{
    public class GamePlayer
    {
        public string UserId { get; set; }
        public Deck Deck { get; set; }
        public Guid ConnectionId { get; set; }
    }
}
