using FlashSale.OrderManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlashSale.OrderManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutBoxController:ControllerBase
    {
        private readonly IOutboxRepository _outboxRepository;

        public OutBoxController(IOutboxRepository outboxRepository)
        {
            _outboxRepository = outboxRepository;
        }

        [HttpGet("{messageId:guid}")]
        public async Task<IActionResult> GetById(Guid messageId)
        {
            var message =
                await _outboxRepository.GetOutBoxMessageByIdAsync(messageId);

            if(message is null)
                return NotFound();

            return Ok(message);
        }

        [HttpGet("unpublished")]
        public async Task<IActionResult> GetUnpublished()
        {
            var messages =
                await _outboxRepository.GetUnpublishedAsync();

            return Ok(messages);
        }
    }
}