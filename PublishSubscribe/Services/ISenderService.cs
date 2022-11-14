namespace PublishSubscribe.Services
{
    public interface ISenderService
    {
        Task<bool> SendPostAsync(string url, string message);
    }
}
