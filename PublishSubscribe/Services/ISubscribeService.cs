using PublishSubscribe.Models;

namespace PublishSubscribe.Services
{
    public interface ISubscribeService
    {
        bool Subscribe(Subscriber subscriber);
        bool Unsubscribe(Subscriber subscriber);

        void AddToQueue(string topic, QueueItem item);
        bool RemoveFromQueue(Subscriber subscriber);
        IEnumerable<(Subscriber Subscriber, QueueItem Message)> GetSubscribers(string topic);
        IEnumerable<string> GetTopics();
    }
}
