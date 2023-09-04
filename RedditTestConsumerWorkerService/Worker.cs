using Newtonsoft.Json;
using Reddit;
using RedditTestConsumerWorkerService.Interfaces;
using RedditTestConsumerWorkerService.Models;

namespace RedditTestConsumerWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly RedditCongif redditCongif;
        private readonly string[] tokens;
        private readonly IApiClient apiClient;
        private readonly ILogger<Worker> logger;
        private readonly int baseQPM = 100;

        public Worker(IConfiguration config, ILogger<Worker> logger, IApiClient apiClient)
        {
            redditCongif = config.GetSection("RedditCongif").Get<RedditCongif>();
            tokens = config.GetSection("RefreshTokens").Get<string[]>();
            this.apiClient = apiClient;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"Worker started.");

            if (redditCongif.TrackedSubreddits.Length == 0)
            {
                throw new ArgumentException("Tracked subreddits are not defined.");
            }

            if (tokens.Length == 0)
            {
                throw new ArgumentException("Tokens are not defined.");
            }

            var baseDelay = 60 * 1000 / baseQPM * tokens.Length / redditCongif.TrackedSubreddits.Length;
            baseDelay = baseDelay >= 1 ? baseDelay : 1;
            logger.LogInformation($"Base delay is {baseDelay} ms.");

            var tokenIndex = 0;
            foreach (var subreddit in redditCongif.TrackedSubreddits)
            {
                RunMonitoring(subreddit, redditCongif.AppId, tokens[tokenIndex], redditCongif.AppSecret, baseDelay);
                tokenIndex = tokenIndex == tokens.Length - 1 ? 0 : ++tokenIndex;
                await Task.Delay(baseDelay, stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            logger.LogInformation($"Worker finished.");
        }

        private void RunMonitoring(string subredditName, string appId, string refreshToken, string appSecret, int baseDelay)
        {
            var loggedTokenPart = refreshToken.Length > 6 ?
                $"{refreshToken.Substring(0, 3)}...{refreshToken.Substring(refreshToken.Length - 3, 3)}"
                : $"{refreshToken.Substring(0, 3)}...";

            logger.LogInformation($"Run monitoring for '{subredditName}' with token '{loggedTokenPart}'");

            try
            {
                var redditClient = new RedditClient(appId, refreshToken, appSecret);
                var subreddit = redditClient.Subreddit(subredditName).About();

                var posts = subreddit.Posts.GetBest();
                logger.LogInformation($"Received best for '{subredditName}' with {posts.Count} posts");
                posts.ForEach(p => SavePost(p));

                logger.LogInformation($"Run monitoring for '{subredditName}'");

                subreddit.Posts.BestUpdated += (sender, e) =>
                {
                    logger.LogInformation($"Received BestUpdated for '{subredditName}' with {e.Added.Count} posts");
                    e.Added.ForEach(p => SavePost(p));
                };
                subreddit.Posts.MonitorBest(monitoringBaseDelayMs: baseDelay, useCache: false);

                subreddit.Posts.RisingUpdated += (sender, e) =>
                {
                    logger.LogInformation($"Received RisingUpdated for '{subredditName}' with {e.Added.Count} posts");
                    e.Added.ForEach(p => SavePost(p));
                };
                subreddit.Posts.MonitorRising(monitoringBaseDelayMs: baseDelay, useCache: false);
            }
            catch (Exception ex)
            {
                logger.LogError($"Can't start monitoring for '{subredditName}'. Exception: {ex.Message}");
            }
        }

        private void SavePost(Reddit.Controllers.Post post)
        {
            var postDto = new PostDto()
            {
                Id = post.Id,
                Subreddit = post.Subreddit,
                Author = post.Author,
                Title = post.Title,
                UpVotes = post.UpVotes
            };

            var serializedPostDto = JsonConvert.SerializeObject(postDto);
            apiClient.PostPostAsync(serializedPostDto);
        }
    }
}