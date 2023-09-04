using RedditTestData.Models;

namespace RedditTestData.Interfaces
{
    public interface IPostRepository
    {
        public Task Upsert(Post post);
        public Task<IEnumerable<Post>> GetMostVoted(string subredditName, int count);
        Task<IEnumerable<SubredditStatistics>> GetStats();
    }
}
