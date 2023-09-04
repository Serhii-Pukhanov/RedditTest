using RedditTestData.Interfaces;
using RedditTestData.Models;
using System.Collections.Concurrent;

namespace RedditTestData.Storages
{
    public class MemoryStorage: IMemoryStorage
    {
        private readonly ConcurrentDictionary<string, Post> Posts = new();

        void IMemoryStorage.AddOrUpdate(Post post)
        {
            Posts.AddOrUpdate(post.Id, post, (key, oldValue) => post);
        }

        IEnumerable<Post> IMemoryStorage.GetPosts()
        {
            return Posts.Values;
        }
    }
}
