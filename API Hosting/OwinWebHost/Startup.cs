using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin;
using Mock4Net.Core.Http;
using Owin;

[assembly: OwinStartup(typeof(OwinWebHost.Startup))]

namespace OwinWebHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var apiKey = ConfigurationManager.AppSettings["MockServerControllerApiKey"];

            var httpServer = OwinHostedHttpServer.New(app, apiKey);
            Mock4Net.Core.FluentMockServer.Start(httpServer, 80, false); //Port is ignored as this is set by this host

        }
    }
}
