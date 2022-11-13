using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublishSubscribe.DTOs;
using PublishSubscribe.Models;
using PublishSubscribe.Services;

namespace PublishSubscribe.Controllers
{
    [ApiController]
    [Route("")]
    public class PublishSubscribeController : Controller
    {
        private readonly ISubscribeService _subscribeService;

        public PublishSubscribeController(ISubscribeService subscribeService)
        {
            _subscribeService = subscribeService;
        }
        [HttpPost]
        [Route("subscribe")]
        public IActionResult Subscribe(SubscribeDTO dto)
        {
            var res = _subscribeService.Subscribe(new Subscriber { Id = dto.SubscriberId,  Topic = dto.Topic, ResponseUrl = dto.ClientUrl });
            if (res)
            {
                return Ok("Subscribe");
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("unsubscribe")]
        public IActionResult Unsubscribe(SubscribeDTO dto)
        {
            var res = _subscribeService.Unsubscribe(new Subscriber { Id = dto.SubscriberId, Topic = dto.Topic, ResponseUrl = dto.ClientUrl });
            if (res)
            {
                return Ok("Unsubscribe");
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("publish")]
        public IActionResult Publish(PublishDTO dto)
        {
            QueueItem queueItem = new QueueItem { Message = dto.Message,PublisherId=dto.PublisherId};
            if (queueItem != null)
            {
                _subscribeService.AddToQueue(dto.Topic, queueItem);
            }

            return Ok("Publish");
        }
    }
}
