using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RedditTestAPI.Controllers;
using RedditTestAppCore.Queries;

namespace RedditTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Subreddits : RedditTestBaseController
    {
        public Subreddits(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

        [HttpGet("GetStats")]
        public async Task<IActionResult> GetStats()
        {
            var query = new GetStatsQuery();

            var response = await mediator.Send(query);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("{subredditName}/GetMostVoted")]
        public async Task<IActionResult> GetMostVoted([FromRoute] string subredditName, [FromQuery] int count = 5)
        {
            var query = new GetMostVotedQuery(subredditName, count);

            var response = await mediator.Send(query);

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}