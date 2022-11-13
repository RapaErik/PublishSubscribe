namespace PublishSubscribe.DTOs
{
    public class MessageDTO
    {

        public long PublishedBy { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Message { get; set; }
    }
}
