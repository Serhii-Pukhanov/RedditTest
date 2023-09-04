using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RedditTestAPI.Controllers;
using RedditTestAPI.Models;
using RedditTestAppCore.Commands;

namespace RedditTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class Posts : RedditTestBaseController
    {
        public Posts(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

        [HttpPost]
        public async Task<IActionResult> UpsertPost([FromBody] UpsertPostRequest upsertPostRequest)
        {
            var command = mapper.Map<UpsertPostCommand>(upsertPostRequest);

            await mediator.Send(command);

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}