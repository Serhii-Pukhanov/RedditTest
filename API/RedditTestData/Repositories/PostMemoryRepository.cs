using RedditTestData.Interfaces;
using RedditTestData.Models;
using RedditTestData.Storages;

namespace RedditTestData.Repositories
{
    public class PostMemoryRepository : IPostRepository
    {
        private readonly IMemoryStorage memoryStorage;
        public PostMemoryRepository(MemoryStorage memoryStorage)
        {
            this.memoryStorage = memoryStorage ?? throw new ArgumentNullException(nameof(memoryStorage));
        }

        public Task Upsert(Post post)
        {
            memoryStorage.AddOrUpdate(post);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<SubredditStatistics>> GetStats()
        {
            return Task.FromResult(memoryStorage.GetPosts()
                                                .GroupBy(p => p.Subreddit)
                                                .Select(g => new SubredditStatistics { Subreddit = g.Key, PostsCount = g.Count() }));
        }

        public Task<IEnumerable<Post>> GetMostVoted(string subredditName, int count)
        {
            return Task.FromResult(memoryStorage.GetPosts()
                                                .Where(p => p.Subreddit.ToLower() == subredditName.ToLower())
                                                .OrderByDescending(p => p.UpVotes)
                                                .Take(count));
        }
    }
}