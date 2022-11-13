namespace PublishSubscribe.DTOs
{
    public class PublishDTO
    {
        public long PublisherId { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
