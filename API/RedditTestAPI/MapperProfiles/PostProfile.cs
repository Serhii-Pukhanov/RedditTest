using AutoMapper;
using RedditTestAPI.Models;
using RedditTestAppCore.Commands;
using RedditTestData.Models;

namespace RedditTestAPI.MapperProfiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<UpsertPostRequest, UpsertPostCommand>();
            CreateMap<UpsertPostCommand, Post>();
        }
    }
}
