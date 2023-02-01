using MagicOnion.Server.Hubs;
using Shared.Data;
using Shared.Hubs.Receiver;
using System.Numerics;

namespace Api.Hubs
{
    public class GamingHub : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        // this class is instantiated per connected so fields are cache area of connection.
        IGroup room;
        Player self;
        IInMemoryStorage<Player> storage;

        public async Task<Player[]> JoinAsync(string roomName, string userName)
        {
            Console.WriteLine("JOİNED");
            self = new Player() { };

            // Group can bundle many connections and it has inmemory-storage so add any type per group.
            (room, storage) = await Group.AddAsync(roomName, self);

            // Typed Server->Client broadcast.
            Broadcast(room).OnJoin(self);

            return storage.AllValues.ToArray();
        }

        public async Task LeaveAsync()
        {
            await room.RemoveAsync(this.Context);
            Broadcast(room).OnLeave(self);
        }

        public async Task MoveAsync(Vector3 position, Quaternion rotation)
        {

            Broadcast(room).OnMove(self);
        }

        // You can hook OnConnecting/OnDisconnected by override.

    }

}
