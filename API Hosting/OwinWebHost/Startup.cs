using System;
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
           var httpServer = OwinHostedHttpServer.New(app);
            Mock4Net.Core.FluentMockServer.Start(httpServer, 80, false);
        }
    }
}
