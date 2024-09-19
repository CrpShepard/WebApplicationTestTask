using Microsoft.AspNetCore.Mvc;
using WebApplicationTestTask.Models;

namespace WebApplicationTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] MessageDto messageDto)
        {
            if (string.IsNullOrEmpty(messageDto.Text) || messageDto.Text.Length > 128)
            {
                return BadRequest("Message text must be between 1 and 128 characters.");
            }

            await _messageService.AddMessageAsync(messageDto);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var messages = await _messageService.GetMessagesAsync(from, to);
            return Ok(messages);
        }
    }
}
