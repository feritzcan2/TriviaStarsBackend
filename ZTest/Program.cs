// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");


try
{

    //// Connect to the server using gRPC channel.
    //var channel = GrpcChannel.ForAddress("http://localhost:12345");

    //// NOTE: If your project targets non-.NET Standard 2.1, use `Grpc.Core.Channel` class instead.
    //// var channel = new Channel("localhost", 5001, new SslCredentials());

    //// Create a proxy to call the server transparently.

    //var client = await StreamingHubClient.ConnectAsync<IGamingHub, IGamingHubReceiver>(channel, new Receiver());
    //// Call the server-side method using the proxy.
    //var res = await client.JoinAsync("game", "test2");

    //await Task.Delay(500000);
    //Console.WriteLine($"Result: ");

}
catch (Exception e)
{
    Console.Write(e);
}

//namespace ZTest
//{
//    public class Receiver : IGamingHubReceiver
//    {
//        public void OnJoin(GamePlayer gamePlayer)
//        {
//        }

//        public void OnLeave(GamePlayer gamePlayer)
//        {
//            throw new NotImplementedException();
//        }

//        public void OnMove(GamePlayer gamePlayer)
//        {
//            throw new NotImplementedException();
//        }

//        public void OnGameStart(DeckDto deckDto)
//        {
//            throw new NotImplementedException();
//        }

//        public ValueTask OnGameStart(StartGameResponse response)
//        {
//            throw new NotImplementedException();
//        }

//        public UnaryResult OnEnemyMove(EnemyCardMove deckDto)
//        {
//            throw new NotImplementedException();
//        }

//        public UnaryResult OnGameDisconnected()
//        {
//            throw new NotImplementedException();
//        }

//        public UnaryResult OnNextRound(NextTurnResponse response)
//        {
//            throw new NotImplementedException();
//        }
//    }
//    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
//    {
//        // The method must return `Task`, `Task<T>`, `Task` or `Task<T>` and can have up to 15 parameters of any type.
//        ValueTask<GamePlayer[]> JoinAsync(string roomName, string userName);
//        ValueTask LeaveAsync();

//    }
//}