using System;
using System.Net;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Mock4Net.Core.Http;

namespace Mock4Net.Core.Http
{
    public class OwinSelfHostedHttpServer : IHttpServer
    {
        private IDisposable _server;
        private IFluentMockServer _mockServer;
        private string _apiKey;

        private OwinSelfHostedHttpServer(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// Setup a new mock server and Api managager
        /// </summary>
        /// <param name="apiKey">They key to use to authenticate all requests to the management api</param>
        /// <returns></returns>
        public static OwinSelfHostedHttpServer New(string apiKey)
        {
            return new OwinSelfHostedHttpServer( apiKey);
        }


        public void Start(string urlPrefix, IFluentMockServer mockServer)
        {
            _mockServer = mockServer;
            Stop();
            _server = WebApp.Start(new StartOptions(urlPrefix), builder =>
            {
                OwinStartup.Configure(mockServer, builder,_apiKey);
            });

        }

        

        public void Stop()
        {
            if (_server != null)
            {
                try
                {
                    _server.Dispose();
                }
                catch
                {
                }
            }
            _server = null;
        }

    }


    public static class Helpers
    {

        public static bool IsServerControllerApiRequest(this IOwinContext context)
        {
            //TODO: Add proper (OAuth2) owin auth!
            return context.Request.Headers.ContainsKey("X-MockServerControllerApiKey");
        }
    }
}