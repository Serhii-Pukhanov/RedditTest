using Microsoft.Extensions.DependencyInjection;
using RedditTestData.Interfaces;
using RedditTestData.Repositories;
using RedditTestData.Storages;

namespace RedditTestData
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services)
        {
            services.AddSingleton<MemoryStorage>();

            services.AddScoped<IPostRepository, PostMemoryRepository>();

            return services;
        }
    }
}
