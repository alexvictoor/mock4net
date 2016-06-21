using System;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace Mock4Net.Core.Http
{

    /// <summary>
    /// Hosted owin IHttpServer used for web hosting the mock server. This server will start also start up a management api that can be used via the ApiClient library.
    /// Note that the port on the startup method will be ignored as this is dictated by the host.
    /// </summary>
    /// <example> 
    /// This sample shows how to use in via a startup class using owin web host.
    /// <code>
    //[assembly: OwinStartup(typeof(OwinWebHost.Startup))]
    //namespace OwinWebHost
    // {
    //     public class Startup
    //     {
    //         public void Configuration(IAppBuilder app)
    //         {
    //             var apiKey = ConfigurationManager.AppSettings["MockServerControllerApiKey"];
    //   
    //             var httpServer = OwinHostedHttpServer.New(app, apiKey);
    //             Mock4Net.Core.FluentMockServer.Start(httpServer, 1234, false); //Port is ignored
    //         }
    //     }
    // }
    /// </code>
    /// </example>
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
            OwinStartup.Configure(_mockServer, _appBuilder, _apiKey);
        }
        
        public void Stop()
        {
            
            //managed by host
        }

    }
}