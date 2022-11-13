namespace PublishSubscribe.DTOs
{
    public class SubscribeDTO
    {
        public long SubscriberId { get; set; }
        public string Token { get; set; }
        public string Topic { get; set; }
        public string ClientUrl { get; set; }
    }
}
