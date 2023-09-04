using MediatR;
using RedditTestData.Interfaces;
using RedditTestData.Models;

namespace RedditTestAppCore.Queries.Handlers
{
    internal class GetStatsHandler : IRequestHandler<GetStatsQuery, IEnumerable<SubredditStatistics>>
    {
        readonly IPostRepository repository;

        public GetStatsHandler(IPostRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<SubredditStatistics>> Handle(GetStatsQuery request, CancellationToken cancellationToken)
        {
            return repository.GetStats();
        }
    }
}
