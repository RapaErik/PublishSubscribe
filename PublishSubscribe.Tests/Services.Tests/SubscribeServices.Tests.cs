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
            Assert.Equal(2, service.SubscriberQueue.Count);
        }
        #endregion

        #region Unsubscribe
        [Fact]
        public void Unsubscribe_SubsciberExistsInTheList_ItShouldReturnTrue()
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
        public void Unsubscribe_SubsciberNotExistsInTheList_ItShouldReturnFalse()
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
        public void Unsubscribe_MultipleUnsubscibe_SubscibersExistsInTheList_ItShouldReturnTrue()
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
            Assert.Equal(queueItem.Message, queue.First().Message);
            Assert.Equal(queueItem.PublishedDate, queue.First().PublishedDate);
            Assert.Equal(queueItem.PublisherId, queue.First().PublisherId);

        }
        [Fact]
        public void AddToQueue_EmptyQueue_ItShouldNotDoAnything()
        {
            // Assert
            QueueItem queueItem = new QueueItem { Message = "msg", PublisherId = 3 };

            var service = new SubscribeService();
            // Act
            service.AddToQueue("topic", queueItem);

            // Assert
            Assert.Empty(service.SubscriberQueue);
        }

        [Fact]
        public void AddToQueue_AddToNotExistingSubscribers_ItShouldUpdateSubscriberQueue()
        {
            // Assert
            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            QueueItem queueItem = new QueueItem { Message = "msg", PublisherId = 3 };


            var service = new SubscribeService();

            var sub = service.Subscribe(subscriber1);


            // Act
            service.AddToQueue("topic", queueItem);

            // Assert
            Assert.True(sub);
            Assert.Single(service.SubscriberQueue);

            Queue<QueueItem> queue = service.SubscriberQueue.FirstOrDefault().Value;
            Assert.NotNull(queue);
            Assert.Empty(queue);

        }
        [Fact]
        public void AddToQueue_AddToManyExistingSubscribers_ItShouldUpdateSubscriberQueue()
        {
            // Assert
            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            Subscriber subscriber2 = new Subscriber { Id = 2, ResponseUrl = "https://google.com", Topic = "topic1" };
            Subscriber subscriber3 = new Subscriber { Id = 3, ResponseUrl = "https://google.com", Topic = "topic2" };

            QueueItem queueItem = new QueueItem { Message = "msg", PublisherId = 4 };


            var service = new SubscribeService();

            var sub1 = service.Subscribe(subscriber1);
            var sub2 = service.Subscribe(subscriber2);
            var sub3 = service.Subscribe(subscriber3);


            // Act
            service.AddToQueue(subscriber1.Topic, queueItem);

            // Assert
            Assert.True(sub1);
            Assert.True(sub2);
            Assert.True(sub3);
            Assert.Equal(3, service.SubscriberQueue.Count);

            List<Queue<QueueItem>> queues = service.SubscriberQueue.Where(x => x.Key.Topic == subscriber1.Topic).Select(s => s.Value).ToList();
            Assert.NotNull(queues);
            Assert.Equal(2, queues.Count);
            foreach (var item in queues)
            {
                Assert.Equal(queueItem.Message, item.First().Message);
                Assert.Equal(queueItem.PublishedDate, item.First().PublishedDate);
                Assert.Equal(queueItem.PublisherId, item.First().PublisherId);
            }
            

        }
        #endregion

        #region GetTopics
        [Fact]
        public void GetTopics_WithManySubscribers_ItShouldReturnAll()
        {
            // Assert

            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            Subscriber subscriber2 = new Subscriber { Id = 2, ResponseUrl = "https://google.com", Topic = "topic2" };
            Subscriber subscriber3 = new Subscriber { Id = 3, ResponseUrl = "https://google.com", Topic = "topic4" };
            Subscriber subscriber4 = new Subscriber { Id = 4, ResponseUrl = "https://google.com", Topic = "topic1" };

            var service = new SubscribeService();


            var sub1 = service.Subscribe(subscriber1);
            var sub2 = service.Subscribe(subscriber2);
            var sub3 = service.Subscribe(subscriber3);
            var sub4 = service.Subscribe(subscriber4);
            // Act

            var topics = service.GetTopics();

            // Assert
            Assert.True(sub1);
            Assert.True(sub2);
            Assert.True(sub3);
            Assert.True(sub4);

            Assert.Equal(4, service.SubscriberQueue.Count);

            Assert.Equal(3, topics.Count());

            Assert.NotNull(topics.Where(x => x == subscriber1.Topic).FirstOrDefault());
            Assert.NotNull(topics.Where(x => x == subscriber2.Topic).FirstOrDefault());
            Assert.NotNull(topics.Where(x => x == subscriber3.Topic).FirstOrDefault());
            Assert.NotNull(topics.Where(x => x == subscriber4.Topic).FirstOrDefault());

        }
        [Fact]
        public void GetTopics_WithEmptyDictionary_ItShouldReturnNone()
        {
            // Assert
            var service = new SubscribeService();

            // Act

            var topics = service.GetTopics();

            // Assert

            Assert.NotNull(service.SubscriberQueue);
            Assert.Empty(service.SubscriberQueue);

            Assert.NotNull(topics);
            Assert.Empty(topics);

        }

        #endregion

        #region GetSubscribers

        [Fact]
        public void GetSubscribers_MultipleSubscribers_WithQueueItemInEachQueue_ItShouldReturnAll()
        {
            // Assert
            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            Subscriber subscriber2 = new Subscriber { Id = 2, ResponseUrl = "https://google.ro", Topic = "topic1" };
            Subscriber subscriber3 = new Subscriber { Id = 3, ResponseUrl = "https://google.hu", Topic = "topic3" };

            QueueItem queueItem1 = new QueueItem { Message = "msg1", PublisherId = 4 };
            QueueItem queueItem2 = new QueueItem { Message = "msg2", PublisherId = 5 };
            QueueItem queueItem3 = new QueueItem { Message = "msg3", PublisherId = 6 };

            var service = new SubscribeService();


            var sub1 = service.Subscribe(subscriber1);
            var sub2 = service.Subscribe(subscriber2);
            var sub3 = service.Subscribe(subscriber3);


            service.AddToQueue(subscriber1.Topic, queueItem1);
            service.AddToQueue(subscriber2.Topic, queueItem2);
            service.AddToQueue(subscriber3.Topic, queueItem3);

            // Act

            var queueLastItems =  service.GetSubscribers(subscriber1.Topic);

            // Assert

            Assert.True(sub1);
            Assert.True(sub2);
            Assert.True(sub3);

            Assert.NotNull(queueLastItems);
            Assert.NotEmpty(queueLastItems);
            Assert.Equal(2, queueLastItems.Count());

            foreach (var item in queueLastItems)
            {
                Assert.Contains(item.Subscriber, new List<Subscriber> { subscriber1, subscriber2 });
                Assert.Equal(queueItem1.Message, item.Message.Message);
            }
        }

        [Fact]
        public void GetSubscribers_MultipleSubscribers_WithEmptyQueue_ItShouldEmpty()
        {
            // Assert
            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };
            Subscriber subscriber2 = new Subscriber { Id = 2, ResponseUrl = "https://google.ro", Topic = "topic1" };
            Subscriber subscriber3 = new Subscriber { Id = 3, ResponseUrl = "https://google.hu", Topic = "topic3" };

            QueueItem queueItem1 = new QueueItem { Message = "msg1", PublisherId = 4 };
            QueueItem queueItem2 = new QueueItem { Message = "msg2", PublisherId = 5 };
            QueueItem queueItem3 = new QueueItem { Message = "msg3", PublisherId = 6 };

            var service = new SubscribeService();


            var sub1 = service.Subscribe(subscriber1);
            var sub2 = service.Subscribe(subscriber2);
            var sub3 = service.Subscribe(subscriber3);

            // Act

            var queueLastItems = service.GetSubscribers(subscriber1.Topic);

            // Assert

            Assert.True(sub1);
            Assert.True(sub2);
            Assert.True(sub3);

            Assert.NotNull(queueLastItems);
            Assert.Empty(queueLastItems);
        }

        #endregion
        #region RemoveFromQueue

        [Fact]
        public void RemoveFromQueue_WithLastItemInQueue_ItShouldRemoveTheLastOne_ItShouldReturnTrue()
        {
            // Assert
            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };

            QueueItem queueItem1 = new QueueItem { Message = "msg1", PublisherId = 4 };

            var service = new SubscribeService();

            var sub1 = service.Subscribe(subscriber1);

            service.AddToQueue(subscriber1.Topic, queueItem1);

            // Act

            var res = service.RemoveFromQueue(subscriber1);

            // Assert

            Assert.True(sub1);
            Assert.True(res);

            Assert.NotNull(service.SubscriberQueue);
            Assert.NotNull(service.SubscriberQueue[subscriber1]);
            Assert.Empty(service.SubscriberQueue[subscriber1]);
          
        }

        [Fact]
        public void RemoveFromQueue_WithEmptyDictionary_ItShouldReturnFalse()
        {
            // Assert
            Subscriber subscriber1 = new Subscriber { Id = 1, ResponseUrl = "https://google.com", Topic = "topic1" };

            QueueItem queueItem1 = new QueueItem { Message = "msg1", PublisherId = 4 };

            var service = new SubscribeService();

           
            service.AddToQueue(subscriber1.Topic, queueItem1);

            // Act

            var res = service.RemoveFromQueue(subscriber1);

            // Assert

            Assert.False(res);

            Assert.NotNull(service.SubscriberQueue);
            Assert.Empty(service.SubscriberQueue);

        }
        #endregion
    }
}
