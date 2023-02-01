// See https://aka.ms/new-console-template for more information

using Api.Hubs;
using Grpc.Net.Client;
using MagicOnion;
using MagicOnion.Client;
using Shared;

Console.WriteLine("Hello, World!");


try
{

    // Connect to the server using gRPC channel.
    var channel = GrpcChannel.ForAddress("http://192.168.1.23:12345");

    // NOTE: If your project targets non-.NET Standard 2.1, use `Grpc.Core.Channel` class instead.
    // var channel = new Channel("localhost", 5001, new SslCredentials());

    // Create a proxy to call the server transparently.

    var client = await StreamingHubClient.ConnectAsync<IGamingHub, IGamingHubReceiver>(channel, new Receiver());
    // Call the server-side method using the proxy.
    var res = await client.JoinAsync("s", "s");
    Console.WriteLine($"Result: ");

}
catch (Exception e)
{
    Console.Write(e);
}
public class Receiver : IGamingHubReceiver
{
    public void OnJoin(Player player)
    {
    }

    public void OnLeave(Player player)
    {
        throw new NotImplementedException();
    }

    public void OnMove(Player player)
    {
        throw new NotImplementedException();
    }
}
public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
{
    // The method must return `Task`, `Task<T>`, `Task` or `Task<T>` and can have up to 15 parameters of any type.
    ValueTask<Player[]> JoinAsync(string roomName, string userName);
    ValueTask LeaveAsync();

}