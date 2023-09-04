using AuthTokenRetriever.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace AuthTokenRetriever
{
    class Program
    {
        private const string Scope = "creddits%20modcontributors%20modmail%20modconfig%20subscribe%20structuredstyles%20vote%20wikiedit%20mysubreddits%20submit%20modlog%20modposts%20modflair%20save%20modothers%20read%20privatemessages%20report%20identity%20livemanage%20account%20modtraffic%20wikiread%20edit%20modwiki%20modself%20history%20flair";

        static void Main()
        {
            var builder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            RedditAppCongif raCongif = config.GetSection("RedditAppCongif").Get<RedditAppCongif>();
            string browser_path = config.GetSection("BrowserPath").Get<string>();

            var server = new InternalHttpServer();
            server.AuthSuccess += AuthSuccess;
            server.AwaitCallback(raCongif.RedirectUrl);

            var authUrl = "https://www.reddit.com/api/v1/authorize?client_id=" + raCongif.AppId
                + "&response_type=code&state=" + raCongif.AppId + ":" + raCongif.AppSecret
                + "&redirect_uri=" + raCongif.RedirectUrl
                + "&duration=permanent&scope=" + Scope;

            OpenBrowser(browser_path, authUrl);

            Console.WriteLine("Awaiting callback or press any key to abort...");

            Console.ReadKey();
        }

        private static void AuthSuccess(object sender, OAuthToken e)
        {
            Console.Clear();
            Console.WriteLine("Access Token: " + e.AccessToken);
            Console.WriteLine();
            Console.WriteLine("Refresh Token: " + e.RefreshToken);
        }

        private static void OpenBrowser(string browser_path, string authUrl)
        {
            try
            {
                Process.Start(authUrl);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Process.Start(new ProcessStartInfo(browser_path, authUrl));
            }
        }
    }
}