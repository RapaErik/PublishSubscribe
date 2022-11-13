using Contracts;

namespace SubClient1.Services
{
    public interface ISubscriberClient
    {
        Task<bool> Subscribe(SubscribeContract subscribe);
        Task<bool> Unsubscribe(SubscribeContract unsubscribe);
    }
}
