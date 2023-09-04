using AutoMapper;
using MediatR;
using RedditTestData.Interfaces;
using RedditTestData.Models;

namespace RedditTestAppCore.Commands.Handlers
{
    internal class UpsertPostCommandHandler : IRequestHandler<UpsertPostCommand>
    {
        readonly IPostRepository repository;
        readonly IMapper mapper;

        public UpsertPostCommandHandler(IPostRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task Handle(UpsertPostCommand request, CancellationToken cancellationToken)
        {
            var post = mapper.Map<Post>(request);
            if (post.Id == null)
            {
                throw new ArgumentNullException(nameof(post.Id));
            }

            return repository.Upsert(post);
        }
    }
}
