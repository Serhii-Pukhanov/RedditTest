using RedditTestData.Models;

namespace RedditTestData.Interfaces
{
    internal interface IMemoryStorage
    {
        IEnumerable<Post> GetPosts();
        void AddOrUpdate(Post postDto);
    }
}
