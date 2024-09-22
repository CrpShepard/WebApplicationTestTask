using Microsoft.AspNetCore.Mvc;
using WebApplicationTestTask.Models;

namespace WebApplicationTestTask.Controllers
{
    public interface IMessageService
    {
        Task<Message> AddMessageAsync(MessageDto messageDto);
        Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to);
    }

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IMessageRepository messageRepository, ILogger<MessageService> logger)
        {
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task<Message> AddMessageAsync(MessageDto messageDto)
        {
            var message = new Message
            {
                Text = messageDto.Text,
                Timestamp = DateTime.UtcNow,
                Order = messageDto.Order
            };

            _logger.LogInformation("Sending WebSocket broadcast message:{Order}, {Text}, {Timestamp}", message.Order, message.Text, message.Timestamp);
            WebSocketHandler.BroadcastMessageAsync(message); // сразу отправка сообщения во все сокеты

            await _messageRepository.AddMessageAsync(message);
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to)
        {
            return await _messageRepository.GetMessagesAsync(from, to);
        }
    }
}
