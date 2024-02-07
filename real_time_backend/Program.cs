using System.Net;
using System.Text;
using real_time_backend;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseWebSockets();


app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var downtownCoords = new Coordinates(18.4510761, -69.9554688);
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            while (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                var buffer = new byte[1024 * 4];
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine(message);

                if (String.IsNullOrEmpty(message)) return;

                var (lat, lon) = message.Split(";") switch
                {
                    var parts => (float.Parse(parts[0]),
                    double.Parse(parts[1]))
                };

                var distance = HarversineDistanceCalculator.Calculate(
                    lat,
                    lon,
                    downtownCoords.Latitude,
                    downtownCoords.Longitude
                );

                string response = $"{lat};{lon};{distance}";
                await webSocket.SendAsync(Encoding.UTF8.GetBytes(response),
                    System.Net.WebSockets.WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );

            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    else
    {
        await next(context);
    }
});

await app.RunAsync();

record Coordinates(double Latitude, double Longitude);