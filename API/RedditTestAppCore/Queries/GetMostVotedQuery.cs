using MediatR;
using RedditTestData.Models;

namespace RedditTestAppCore.Queries
{
    public class GetMostVotedQuery : IRequest<IEnumerable<Post>>
    {
        public string SubredditName { get; set; }
        public int Count { get; set; }

        public GetMostVotedQuery(string subredditName, int count = 10)
        {
            SubredditName = subredditName;
            Count = count;
        }
    }
}
