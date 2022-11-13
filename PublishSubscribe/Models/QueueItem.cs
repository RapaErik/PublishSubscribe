namespace PublishSubscribe.Models
{
    public class QueueItem
    {
        public long PublisherId { get; set; }
        public string Message { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

    }
}
