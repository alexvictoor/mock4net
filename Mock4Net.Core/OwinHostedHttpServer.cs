using System;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace Mock4Net.Core.Http
{
    public class OwinHostedHttpServer : IHttpServer
    {
        private IAppBuilder _appBuilder;
        private readonly string _apiKey;
        private IFluentMockServer _mockServer;

        private OwinHostedHttpServer(IAppBuilder appBuilder, string apiKey)
        {
            _appBuilder = appBuilder;
            _apiKey = apiKey;
        }

        /// <summary>
        /// Setup a new mock server and Api managager
        /// </summary>
        /// <param name="appBuilder">The IAppBuilder to configure</param>
        /// <param name="apiKey">They key to use to authenticate all requests to the management api</param>
        /// <returns></returns>
        public static OwinHostedHttpServer New(IAppBuilder appBuilder, string apiKey)
        {
            return new OwinHostedHttpServer(appBuilder, apiKey);
        }

        public void Start(string urlPrefix, IFluentMockServer mockServer)
        {
            _mockServer = mockServer;
            OwinStartup.Configure(mockServer, _appBuilder, _apiKey);
        }
        
        public void Stop()
        {
            
            //managed by host
        }

    }
}