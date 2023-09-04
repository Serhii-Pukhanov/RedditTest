using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RedditTestAPI.Controllers
{
    public class RedditTestBaseController : ControllerBase
    {
        protected IMediator mediator;
        protected IMapper mapper;

        public RedditTestBaseController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }
    }
}
