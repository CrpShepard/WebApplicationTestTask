using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebApplicationTestTask.Models;

namespace WebApplicationTestTask.Controllers
{
    public class WebSocketHandler
    {
        //private readonly ILogger<WebSocketHandler> _logger;
        private static List<WebSocket> _sockets = new List<WebSocket>();

        public WebSocketHandler()
        {
            //_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                _sockets.Add(socket);

                //_logger.LogInformation("WebSocket connection established. Total connections: {ConnectionCount}", _sockets.Count);

                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        _sockets.Remove(socket);
                        //_logger.LogInformation("WebSocket connection closed. Total connections: {ConnectionCount}", _sockets.Count);
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                    }
                }
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        public static async Task BroadcastMessageAsync(Message message)
        {
            var output = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(output);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
