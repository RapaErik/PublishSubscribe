using Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SubClient1.Models;
using SubClient1.Services;
using System;
using System.Diagnostics;
using System.Text;

namespace SubClient1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataWrapper _dataWrapper;
        private readonly ISubscriberClient _subscriberClient;

        public HomeController(ILogger<HomeController> logger, DataWrapper dataWrapper, ISubscriberClient subscriberClient)
        {
            
            _logger = logger;
            _dataWrapper = dataWrapper;
            _subscriberClient = subscriberClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                Topic1 = _dataWrapper.Topic1,
                Topic2 = _dataWrapper.Topic2,
                Topic3 = _dataWrapper.Topic3,
                Status1 = _dataWrapper.StatusTopic1,
                Status2 = _dataWrapper.StatusTopic2,
                Status3 = _dataWrapper.StatusTopic3
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string buttonSubscirbe, string buttonUnsubscribe)
        {

            var sub1 = new SubscribeContract
            {
                ClientUrl = "https://127.0.0.1:7069/receiver1",
                SubscriberId = 10,
                Token = "4c91020e40fdaf78a251a3892bf94146",
                Topic = TopicConstants.Topic1
            };


            var sub2 = new SubscribeContract
            {
                ClientUrl = "https://127.0.0.1:7069/receiver2",
                SubscriberId = 10,
                Token = "73b71181297b2c64d19af1efd9d8f282",
                Topic = TopicConstants.Topic2
            };

            var sub3 = new SubscribeContract
            {
                ClientUrl = "https://127.0.0.1:7069/receiver3",
                SubscriberId = 10,
                Token = "ee40e0dd816b7d1fbab0fa62b897c3e5",
                Topic = TopicConstants.Topic3
            };




            if (!string.IsNullOrWhiteSpace(buttonSubscirbe))
            {
                switch (buttonSubscirbe)
                {
                    case TopicConstants.Topic1:
                        if (!_dataWrapper.StatusTopic1)
                        {
                            bool res = await _subscriberClient.Subscribe(sub1);
                            _dataWrapper.StatusTopic1 = res;
                        }
                        break;

                    case TopicConstants.Topic2:
                        if (!_dataWrapper.StatusTopic2)
                        {
                            bool res = await _subscriberClient.Subscribe(sub2);
                            _dataWrapper.StatusTopic2 = res;
                        }
                        break;

                    case TopicConstants.Topic3:
                        if (!_dataWrapper.StatusTopic3)
                        {
                            bool res = await _subscriberClient.Subscribe(sub3);
                            _dataWrapper.StatusTopic3 = res;
                        }
                        break;
                }
            }
            else if (!string.IsNullOrWhiteSpace(buttonUnsubscribe))
            {
                switch (buttonUnsubscribe)
                {
                    case TopicConstants.Topic1:
                        if (_dataWrapper.StatusTopic1)
                        {
                            bool res = await _subscriberClient.Unsubscribe(sub1);
                            _dataWrapper.Topic1.Clear();
                            _dataWrapper.StatusTopic1 = !res;
                        }
                        break;

                    case TopicConstants.Topic2:
                        if (_dataWrapper.StatusTopic2)
                        {
                            bool res = await _subscriberClient.Unsubscribe(sub2);
                            _dataWrapper.Topic2.Clear();
                            _dataWrapper.StatusTopic2 = !res;
                        }
                        break;

                    case TopicConstants.Topic3:
                        if (_dataWrapper.StatusTopic3)
                        {
                            bool res = await _subscriberClient.Unsubscribe(sub3);
                            _dataWrapper.Topic3.Clear();
                            _dataWrapper.StatusTopic3 = !res;
                        }
                        break;
                }
            }


            var viewModel = new HomeViewModel
            {
                Topic1 = _dataWrapper.Topic1,
                Topic2 = _dataWrapper.Topic2,
                Topic3 = _dataWrapper.Topic3,
                Status1 = _dataWrapper.StatusTopic1,
                Status2 = _dataWrapper.StatusTopic2,
                Status3 = _dataWrapper.StatusTopic3
            };


            return View(viewModel);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}