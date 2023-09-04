namespace RedditTestConsumerWorkerService.Models
{
    public class RedditCongif
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string[] TrackedSubreddits { get; set; }
    }
}
