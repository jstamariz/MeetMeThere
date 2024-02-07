using System.Net.WebSockets;
using System.Text;

var url = new Uri("ws://localhost:5124/ws");
var token = CancellationToken.None;

try
{
    using var client = new ClientWebSocket();
    Console.WriteLine("Connecting...");
    await client.ConnectAsync(url, token);
    Console.WriteLine("Connected!");

    var coordinates = "18.5458;69.8673";
    var buffer = new byte[1024 * 4];
    while (client.State == WebSocketState.Open)
    {
        try
        {
            await client.SendAsync(Encoding.UTF8.GetBytes(coordinates), WebSocketMessageType.Text, true, token);
            var result = await client.ReceiveAsync(buffer, token);

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var data = message.Split(";");

            coordinates = String
                .Join(";", data
                    .Take(2)
                    .Select(coordinate => double.Parse(coordinate) + .1));

            Console.WriteLine($"Distance is {data.ElementAt(2)} meters");

            Thread.Sleep(2000);
        }
        catch (Exception error)
        {
            await client.CloseAsync(WebSocketCloseStatus.InternalServerError, error.Message, token);
        }
    }

}
catch (System.Exception)
{

    throw;
}