using Moq;
using Newtonsoft.Json;
using PublishSubscribe.DTOs;
using PublishSubscribe.Models;
using PublishSubscribe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.Tests.Services.Tests
{
    public class BrokerServiceTests
    {
        private readonly Mock<ISubscribeService> _subscribeServiceMock;
        private readonly Mock<ISenderService> _senderServiceMock;
        public BrokerServiceTests()
        {
            _subscribeServiceMock = new Mock<ISubscribeService>();
            _senderServiceMock = new Mock<ISenderService>();
        }
        [Fact]
        public async Task SendMessageAsync_TopicNotExistsInList_NeverMakeHttpRequestAndAcknowledge()
        {
            //Arrange

            string topic = "topic0";

            IEnumerable<(Subscriber, QueueItem)> enumerable = new List<(Subscriber, QueueItem)>();
            _subscribeServiceMock.Setup(x => x.GetSubscribers(It.IsAny<string>())).Returns(enumerable);

            var service = new BrokerService(_subscribeServiceMock.Object, _senderServiceMock.Object);

            //Act

            await service.SendMessageAsync(topic);

            //Assert

            _subscribeServiceMock.Verify(x => x.GetSubscribers(It.IsAny<string>()), Times.Once);
            _senderServiceMock.Verify(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _subscribeServiceMock.Verify(x => x.RemoveFromQueue(It.IsAny<Subscriber>()), Times.Never);
        }


        [Fact]
        public async Task SendMessageAsync_TopicExistsInListOnce_MakeHttpRequestAndGetAcknowledge()
        {
            //Arrange

            string topic = "topic1";

            IEnumerable<(Subscriber, QueueItem)> enumerable = new List<(Subscriber, QueueItem)> { new(new Subscriber(), new QueueItem()) };

            _subscribeServiceMock.Setup(x => x.GetSubscribers(It.IsAny<string>())).Returns(enumerable);
            _subscribeServiceMock.Setup(x => x.RemoveFromQueue(It.IsAny<Subscriber>()));
            _senderServiceMock.Setup(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            var service = new BrokerService(_subscribeServiceMock.Object, _senderServiceMock.Object);

            //Act

            await service.SendMessageAsync(topic);

            //Assert

            _subscribeServiceMock.Verify(x => x.GetSubscribers(It.IsAny<string>()), Times.Once);
            _senderServiceMock.Verify(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _subscribeServiceMock.Verify(x => x.RemoveFromQueue(It.IsAny<Subscriber>()), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_TopicExistsInListOnce_MakeHttpRequestAndGetNoAcknowledge()
        {
            //Arrange

            string topic = "topic1";

            IEnumerable<(Subscriber, QueueItem)> enumerable = new List<(Subscriber, QueueItem)> { new(new Subscriber(), new QueueItem()) };

            _subscribeServiceMock.Setup(x => x.GetSubscribers(It.IsAny<string>())).Returns(enumerable);
            _subscribeServiceMock.Setup(x => x.RemoveFromQueue(It.IsAny<Subscriber>()));
            _senderServiceMock.Setup(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var service = new BrokerService(_subscribeServiceMock.Object, _senderServiceMock.Object);

            //Act

            await service.SendMessageAsync(topic);

            //Assert

            _subscribeServiceMock.Verify(x => x.GetSubscribers(It.IsAny<string>()), Times.Once);
            _senderServiceMock.Verify(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _subscribeServiceMock.Verify(x => x.RemoveFromQueue(It.IsAny<Subscriber>()), Times.Never);
        }

        [Fact]
        public async Task SendMessageAsync_TopicExistsInListExactly3Times_MakeHttpRequestAndGetNoAcknowledge()
        {
            //Arrange

            string topic = "topic1";

            IEnumerable<(Subscriber, QueueItem)> enumerable = new List<(Subscriber, QueueItem)> {
                new(new Subscriber(), new QueueItem()) ,
                new(new Subscriber(), new QueueItem()) ,
                new(new Subscriber(), new QueueItem()) ,
            };

            _subscribeServiceMock.Setup(x => x.GetSubscribers(It.IsAny<string>())).Returns(enumerable);
            _subscribeServiceMock.Setup(x => x.RemoveFromQueue(It.IsAny<Subscriber>()));
            _senderServiceMock.Setup(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var service = new BrokerService(_subscribeServiceMock.Object, _senderServiceMock.Object);

            //Act

            await service.SendMessageAsync(topic);

            //Assert

            _subscribeServiceMock.Verify(x => x.GetSubscribers(It.IsAny<string>()), Times.Once);
            _senderServiceMock.Verify(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
            _subscribeServiceMock.Verify(x => x.RemoveFromQueue(It.IsAny<Subscriber>()), Times.Never);
        }
        [Fact]
        public async Task SendMessageAsync_TopicExistsInListExactly3Times_MakeHttpRequestAndGetAcknowledge()
        {
            //Arrange

            string topic = "topic1";

            IEnumerable<(Subscriber, QueueItem)> enumerable = new List<(Subscriber, QueueItem)> {
                new(new Subscriber(), new QueueItem()) ,
                new(new Subscriber(), new QueueItem()) ,
                new(new Subscriber(), new QueueItem()) ,
            };

            _subscribeServiceMock.Setup(x => x.GetSubscribers(It.IsAny<string>())).Returns(enumerable);
            _subscribeServiceMock.Setup(x => x.RemoveFromQueue(It.IsAny<Subscriber>()));
            _senderServiceMock.Setup(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            var service = new BrokerService(_subscribeServiceMock.Object, _senderServiceMock.Object);

            //Act

            await service.SendMessageAsync(topic);

            //Assert

            _subscribeServiceMock.Verify(x => x.GetSubscribers(It.IsAny<string>()), Times.Once);
            _senderServiceMock.Verify(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
            _subscribeServiceMock.Verify(x => x.RemoveFromQueue(It.IsAny<Subscriber>()), Times.Exactly(3));
        }


        [Fact]
        public async Task SendMessageAsync_TestingNewtonSerializePartWithExactObject_TopicExistsInListOnce_MakeHttpRequestAndGetAcknowledge()
        {
            //Arrange

            string topic = "topic1";
            DateTime testNow = DateTime.Now;

            Subscriber subscriber = new Subscriber
            {
                Id = 2,
                ResponseUrl = "TestUrl",
                Topic = topic
            };
            QueueItem queueItem = new QueueItem
            {
                Message = "message",
                PublishedDate = testNow,
                PublisherId = 1
            };

            string msg = JsonConvert.SerializeObject(
                       new MessageDTO
                       {
                           Message = queueItem.Message,
                           PublishedDate = queueItem.PublishedDate,
                           PublishedBy = queueItem.PublisherId
                       });


            IEnumerable<(Subscriber, QueueItem)> enumerable = new List<(Subscriber, QueueItem)> {new(subscriber,queueItem)};

            _subscribeServiceMock.Setup(x => x.GetSubscribers(It.IsAny<string>())).Returns(enumerable);
            _subscribeServiceMock.Setup(x => x.RemoveFromQueue(It.IsAny<Subscriber>()));
            _senderServiceMock.Setup(x => x.SendPostAsync(subscriber.ResponseUrl, msg)).Returns(Task.FromResult(true));

            var service = new BrokerService(_subscribeServiceMock.Object, _senderServiceMock.Object);

            //Act

            await service.SendMessageAsync(topic);

            //Assert

            _subscribeServiceMock.Verify(x => x.GetSubscribers(It.IsAny<string>()), Times.Once);
            _senderServiceMock.Verify(x => x.SendPostAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _subscribeServiceMock.Verify(x => x.RemoveFromQueue(It.IsAny<Subscriber>()), Times.Once);

            _senderServiceMock.Verify(x => x.SendPostAsync(subscriber.ResponseUrl, msg), Times.Once);
            _subscribeServiceMock.Verify(x => x.RemoveFromQueue(subscriber), Times.Once);
        }
    }
}
