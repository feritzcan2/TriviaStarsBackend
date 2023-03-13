using GameEngine.Entities.Hubs.Receiver;

namespace Api.Service.GameHub.Data.Player
{
    public struct NetworkPlayer
    {
        public Guid ConnectionId { get; set; }
        public IGamingHubReceiver Broadcaster { get; set; }
        public IGamingHubReceiver BroadcasterExceptSelf { get; set; }
    }
}
