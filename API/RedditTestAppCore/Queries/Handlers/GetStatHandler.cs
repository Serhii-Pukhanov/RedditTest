using MediatR;
using RedditTestData.Interfaces;
using RedditTestData.Models;

namespace RedditTestAppCore.Queries.Handlers
{
    internal class GetMostVotedQueryHandler : IRequestHandler<GetMostVotedQuery, IEnumerable<Post>>
    {
        readonly IPostRepository repository;

        public GetMostVotedQueryHandler(IPostRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<Post>> Handle(GetMostVotedQuery request, CancellationToken cancellationToken)
        {
            return repository.GetMostVoted(request.SubredditName, request.Count);
        }
    }
}
