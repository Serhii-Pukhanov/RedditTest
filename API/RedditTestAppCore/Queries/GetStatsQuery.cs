using MediatR;
using RedditTestData.Models;

namespace RedditTestAppCore.Queries
{
    public class GetStatsQuery : IRequest<IEnumerable<SubredditStatistics>>
    {
    }
}
