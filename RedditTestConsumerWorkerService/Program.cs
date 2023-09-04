using RedditTestConsumerWorkerService;
using RedditTestConsumerWorkerService.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<IApiClient, ApiClient>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
