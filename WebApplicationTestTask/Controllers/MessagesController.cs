using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplicationTestTask.Models;

namespace WebApplicationTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        [NonAction]
        public IActionResult Index()
        {
            return View();
        }

        private readonly IMessageService _messageService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessageService messageService, ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        /// <summary>
        /// Отправляет сообщение.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>Результат выполнения запроса</returns>
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] MessageDto messageDto)
        {
            if (string.IsNullOrEmpty(messageDto.Text) || messageDto.Text.Length > 128)
            {
                _logger.LogError("POST: Invalid model state for message: {Text}, {Order}", messageDto.Text, messageDto.Order);
                return BadRequest("Message text must be between 1 and 128 characters.");
            }

            try
            {
                _logger.LogInformation("POST: Sending message: {Text}, {Order}", messageDto.Text, messageDto.Order);
                await _messageService.AddMessageAsync(messageDto);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST: Error occurred while sending message: {Text}, {Order}", messageDto.Text, messageDto.Order);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Возвращает сообщения за последние 10 минут.
        /// </summary>
        /// <returns>Список сообщений вместе с результатом запроса</returns>
        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var messages = await _messageService.GetMessagesAsync(from, to);

            if (messages == null)
            {
                _logger.LogError("GET: Error occurred while sending messages: {messages}", messages);
                return StatusCode(500, "Internal server error");
            }
            try
            {
                _logger.LogInformation("GET: Sending messages: {messages}", messages);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET: Error occurred while sending messages: {messages}", messages);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
