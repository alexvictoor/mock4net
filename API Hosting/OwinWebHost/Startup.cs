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
            //TODO. How to capture current port without request to get it from
            //Mock4Net.Core.FluentMockServer.Start(httpServer, 80, false);
            //Port does not actualy matter here (is ignored.. as the hoisted owin server uses the port from its host..
            Mock4Net.Core.FluentMockServer.Start(httpServer, 1234, false); //Debug settings

        }
    }
}
