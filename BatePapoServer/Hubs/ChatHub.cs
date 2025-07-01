using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace BatePapoServer.Hubs
{
    
    public class ChatHub : Hub
    {
       private readonly IMongoCollection<ChatMessage> _messages;

            public ChatHub(IMongoDatabase datebase)
        {
            _messages = datebase.GetCollection<ChatMessage>("messages");

        }
        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new ChatMessage
            {
                User = user,
                Message = message,
                Timestamp = DateTime.UtcNow

            };
            //Persistir no MongoDB
            await _messages.InsertOneAsync(chatMessage);
            // Enviar para todos os Clientes conectados
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }          
                 

    }
    public class ChatMessage
    {
        public string User{  get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
