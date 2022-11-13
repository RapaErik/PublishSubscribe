namespace PublishSubscribe.Services
{
    public interface ISenderService
    {
        Task<bool> SendPost(string url, string message);
        Task<bool> SendPing();
    }
}
