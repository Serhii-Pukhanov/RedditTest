namespace RedditTestAPI.Models
{
    public class UpsertPostRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Subreddit { get; set; }
        public string Author { get; set; }
        public int UpVotes { get; set; }
    }
}
