using AuthTokenRetriever.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Net.Sockets;
using System.Text;
using uhttpsharp;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;

namespace AuthTokenRetriever
{
    internal class InternalHttpServer
    {
        public event EventHandler<OAuthToken> AuthSuccess;

        public void AwaitCallback(string redirectUri)
        {
            Uri uri = new(redirectUri);

            using var httpServer = new HttpServer(new HttpRequestProvider());
            var listener = new TcpListener(IPAddress.Parse(uri.Host.Equals("localhost") ? IPAddress.Loopback.ToString() : uri.Host), uri.Port);
            httpServer.Use(new TcpListenerAdapter(listener));

            httpServer.Use((context, next) =>
            {
                string code = null;
                string state = null;
                try
                {
                    code = context.Request.QueryString.GetByName("code");
                    state = context.Request.QueryString.GetByName("state");
                }
                catch (KeyNotFoundException)
                {
                    context.Response = new uhttpsharp.HttpResponse(HttpResponseCode.Ok, Encoding.UTF8.GetBytes("<b>ERROR:  No code and/or state received!</b>"), false);
                    throw new Exception("ERROR:  Request received without code and/or state!");
                }

                if (!string.IsNullOrWhiteSpace(code)
                    && !string.IsNullOrWhiteSpace(state))
                {
                    RestRequest restRequest = new("/api/v1/access_token", Method.POST);

                    restRequest.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(state)));
                    restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    restRequest.AddParameter("grant_type", "authorization_code");
                    restRequest.AddParameter("code", code);
                    restRequest.AddParameter("redirect_uri", redirectUri);

                    IRestResponse res = new RestClient("https://www.reddit.com").Execute(restRequest);
                    if (res != null && res.IsSuccessful)
                    {
                        OAuthToken oAuthToken = JsonConvert.DeserializeObject<OAuthToken>(res.Content);
                        AuthSuccess?.Invoke(this, oAuthToken);
                    }
                    else
                    {
                        Exception ex = new("API returned non-success response.");
                        ex.Data.Add("res", res);
                        throw ex;
                    }

                    context.Response = new uhttpsharp.HttpResponse(HttpResponseCode.Ok, Encoding.UTF8.GetBytes(res.Content), false);
                }

                return Task.Factory.GetCompleted();
            });

            httpServer.Use(new ErrorHandler());

            httpServer.Start();
        }
    }

    class ErrorHandler : IHttpRequestHandler
    {
        public Task Handle(IHttpContext context, Func<Task> next)
        {
            context.Response = new uhttpsharp.HttpResponse(HttpResponseCode.NotFound, "These are not the droids you are looking for.", true);
            return Task.Factory.GetCompleted();
        }
    }

}
