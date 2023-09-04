using RedditTestConsumerWorkerService.Interfaces;
using System.Text;

namespace RedditTestConsumerWorkerService
{
    internal class ApiClient : IApiClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<ApiClient> logger;

        public ApiClient(IConfiguration config, ILogger<ApiClient> logger)
        {
            var apiBaseUrl = config.GetSection("ApiBaseUrl").Get<string>();
            httpClient = new HttpClient() { BaseAddress = new Uri(apiBaseUrl) };
            this.logger = logger;
        }

        public async void PostPostAsync(string postData)
        {
            try
            {
                using StringContent content = new(postData, Encoding.UTF8, "application/json");
                using HttpResponseMessage response = await httpClient.PostAsync("api/posts", content);
                // logger.LogInformation(response.IsSuccessStatusCode ? $"Post saved" : $"Failed to save post. Status code: {response.StatusCode}.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Can not send request to API.", ex.Message);
            }
        }
    }
}
