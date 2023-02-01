using MessagePack;

namespace Shared
{
    // Server -> Client definition


    // for example, request object by MessagePack.
    [MessagePackObject]
    public class Player
    {
        [Key(0)]
        public string Name { get; set; }
    }
}
