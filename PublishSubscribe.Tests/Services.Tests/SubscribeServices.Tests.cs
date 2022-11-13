using Moq;
using PublishSubscribe.Models;
using PublishSubscribe.Services;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.Tests.Services.Tests
{
    public class SubscribeServices
    {
        public SubscribeServices()
        {
           
        }

        #region Subscribe
        [Fact]
        public void Subscribe_FirstEntryInList_ItShouldReturnTrue()
        {
            // Assert

            Subscriber subscriber = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            var service = new SubscribeService();

            // Act
            var res = service.Subscribe(subscriber);

            // Assert
            Assert.True(res);
            Assert.Single(service.SubscriberQueue);
        }

        [Fact]
        public void Subscribe_TwoIdenticalSubscribers_AvoidDuplications_ItShouldReturnFalse()
        {
            // Assert

            Subscriber subscriber = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            var service = new SubscribeService();

            var firsRes = service.Subscribe(subscriber);

            // Act
            var secoundRes = service.Subscribe(subscriber);

            // Assert
            Assert.True(firsRes);
            Assert.False(secoundRes);
            Assert.Single(service.SubscriberQueue);
        }

        [Fact]
        public void Subscribe_TwoDifferentSubscribers_AvoidDuplications_ItShouldReturnFalse()
        {
            // Assert

            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            Subscriber subscriber2 = new Subscriber { Id = 2, ResponseUrl = "https://google.ro", Topic = "topic3" };

            var service = new SubscribeService();

            var firsRes = service.Subscribe(subscriber1);

            // Act
            var secoundRes = service.Subscribe(subscriber2);

            // Assert
            Assert.True(firsRes);
            Assert.True(secoundRes);
            Assert.Equal(2,service.SubscriberQueue.Count);
        }
        #endregion

        #region Unubscribe
        [Fact]
        public void Unubscribe_SubsciberExistsInTheList_ItShouldReturnTrue()
        {
            // Assert
            Subscriber subscriber = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };

            var service = new SubscribeService();

            var sub = service.Subscribe(subscriber);

            // Act
            var res = service.Unsubscribe(subscriber);

            // Assert
            Assert.True(sub);
            Assert.True(res);
            Assert.Empty(service.SubscriberQueue);
        }

        [Fact]
        public void Unubscribe_SubsciberNotExistsInTheList_ItShouldReturnFalse()
        {
            // Assert
            Subscriber subscriber = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };

            var service = new SubscribeService();

            // Act
            var res = service.Unsubscribe(subscriber);

            // Assert
            Assert.False(res);
            Assert.Empty(service.SubscriberQueue);
        }

        [Fact]
        public void Unubscribe_MultipleUnsubscibe_SubscibersExistsInTheList_ItShouldReturnTrue()
        {
            // Assert
            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            Subscriber subscriber2 = new Subscriber { Id = 2, ResponseUrl = "https://google.ro", Topic = "topic2" };
            Subscriber subscriber3 = new Subscriber { Id = 3, ResponseUrl = "https://google.hu", Topic = "topic3" };

            var service = new SubscribeService();


            var sub1 = service.Subscribe(subscriber1);
            var sub2 = service.Subscribe(subscriber2);
            var sub3 = service.Subscribe(subscriber3);


            // Act
            var res1 = service.Unsubscribe(subscriber1);
            var res2 = service.Unsubscribe(subscriber2);
            var res3 = service.Unsubscribe(subscriber3);

            // Assert

            Assert.True(sub1);
            Assert.True(sub2);
            Assert.True(sub3);

            Assert.True(res1);
            Assert.True(res2);
            Assert.True(res3);

            Assert.Empty(service.SubscriberQueue);
        }



        #endregion

        #region AddToQueue
        [Fact]
        public void AddToQueue_AddToExistingSubscribers_ItShouldUpdateSubscriberQueue()
        {
            // Assert
            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            QueueItem queueItem = new QueueItem { Message = "msg", PublisherId = 3 };


            var service = new SubscribeService();

            var sub = service.Subscribe(subscriber1);


            // Act
            service.AddToQueue(subscriber1.Topic, queueItem);

            // Assert
            Assert.True(sub);
            Assert.Single(service.SubscriberQueue);

            Queue<QueueItem> queue = service.SubscriberQueue.FirstOrDefault().Value;
            Assert.NotNull(queue);
            Assert.Single(queue);
            Assert.Equal(queueItem.Message,queue.First().Message);
            Assert.Equal(queueItem.PublishedDate, queue.First().PublishedDate);
            Assert.Equal(queueItem.PublisherId, queue.First().PublisherId);

        }
        #endregion

    }
}
