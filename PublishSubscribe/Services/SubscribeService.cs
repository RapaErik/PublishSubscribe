﻿using PublishSubscribe.Models;
using System.Collections.Concurrent;

namespace PublishSubscribe.Services
{
    public class SubscribeService : ISubscribeService
    {
        //private readonly SubscriberEqualityComparer _subscriberEqualityComparer = new SubscriberEqualityComparer();
        private readonly ConcurrentDictionary<Subscriber, Queue<QueueItem>> _subscriberQueues;
        private static object _lockSubscriber = new object();
        public SubscribeService()
        {
            _subscriberQueues = new ConcurrentDictionary<Subscriber, Queue<QueueItem>>(new SubscriberEqualityComparer());
        }

        public void AddToQueue(string topic, QueueItem queueItem)
        {
            lock (_lockSubscriber)
            {
                var toUpdate = _subscriberQueues.Where(s => s.Key.Topic == topic).Select(s => s.Key);
                if (toUpdate?.Count() > 0)
                {
                    foreach (var item in toUpdate)
                    {
                        _subscriberQueues[item].Enqueue(queueItem);
                    }
                }
            }
        }
        public IEnumerable<string> GetTopics()
        {
            foreach (var item in _subscriberQueues.Keys.Select(s=>s.Topic))
            {
                yield return item;
            }
        }

        public IEnumerable<(Subscriber Subscriber, QueueItem Message)> GetSubscribers(string topic)
        {
            var subscribers = _subscriberQueues.Where(s => s.Key.Topic == topic && s.Value.Count>0).Select(s => s.Key);
            if (subscribers?.Count() > 0)
            {
                foreach (var item in subscribers)
                {
                    if (_subscriberQueues.TryGetValue(item, out var queues))
                    { 
                        yield return (item, queues.Peek());
                    }
                }
            }
        }

        public bool Subscribe(Subscriber subscriber)
        {
            return _subscriberQueues.TryAdd(subscriber, new Queue<QueueItem>());
        }

        public bool Unsubscribe(Subscriber subscriber)
        {
            return _subscriberQueues.TryRemove(subscriber, out var queues);
        }

        public bool RemoveFromQueue(Subscriber subscriber)
        {
            bool isSuccessed = false;
            lock (_lockSubscriber)
            {
                if (_subscriberQueues.TryGetValue(subscriber, out var queues))
                {
                    isSuccessed = queues.TryDequeue(out var queueItem);
                }
            }
            return isSuccessed;
        }
    }
}
