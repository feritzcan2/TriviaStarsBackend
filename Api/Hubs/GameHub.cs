using MagicOnion;
using Shared;

namespace Api.Hubs
{
    public interface IGamingHubReceiver
    {
        // The method must have a return type of `void` and can have up to 15 parameters of any type.
        void OnJoin(Player player);
        void OnLeave(Player player);
        void OnMove(Player player);
    }
    // implements `IStreamingHub<TSelf, TReceiver>`  and share this type between server and client.
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        // The method must return `Task`, `Task<T>`, `Task` or `Task<T>` and can have up to 15 parameters of any type.
        Task<Player[]> JoinAsync(string roomName, string userName);
        Task LeaveAsync();

    }

}
