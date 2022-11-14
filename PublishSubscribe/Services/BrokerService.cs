using Newtonsoft.Json;
using NLog;
using PublishSubscribe.DTOs;
using PublishSubscribe.Models;

namespace PublishSubscribe.Services
{
    public class BrokerService : BackgroundService
    {
        private readonly ISubscribeService _subscribeService;
        private readonly ISenderService _senderService;
        private readonly Logger _logger;

        public BrokerService(ISubscribeService subscribeService, ISenderService senderService)
        {
            _subscribeService = subscribeService;
            _senderService = senderService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (true)
                {
                    foreach (var item in _subscribeService.GetTopics())
                    {
                        await SendMessageAsync(item);
                    }
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

        }
      


        public async Task SendMessageAsync(string topic)
        {
            Dictionary<Subscriber, Task<bool>> msgSendings = new Dictionary<Subscriber, Task<bool>>();
            foreach (var item in _subscribeService.GetSubscribers(topic))
            {
                string msg = JsonConvert.SerializeObject(
                    new MessageDTO { 
                        Message = item.Message.Message, 
                        PublishedDate = item.Message.PublishedDate, 
                        PublishedBy = item.Message.PublisherId 
                    });
                msgSendings.Add(item.Subscriber, _senderService.SendPostAsync(item.Subscriber.ResponseUrl, msg));
            }
            await Task.WhenAll(msgSendings.Select(x => x.Value).ToList());

            //acknowledge part
            foreach (var item in msgSendings)
            {
                if (await item.Value)
                {
                    _subscribeService.RemoveFromQueue(item.Key);
                }
            }
        }

 
    }
}
