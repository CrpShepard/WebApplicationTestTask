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

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> AddMessageAsync(MessageDto messageDto)
        {
            var message = new Message
            {
                Text = messageDto.Text,
                Timestamp = DateTime.UtcNow,
                Order = messageDto.Order
            };

            await _messageRepository.AddMessageAsync(message);
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to)
        {
            return await _messageRepository.GetMessagesAsync(from, to);
        }
    }
}
