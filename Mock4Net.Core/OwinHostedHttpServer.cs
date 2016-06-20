using System;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace Mock4Net.Core.Http
{
    public class OwinHostedHttpServer : IHttpServer
    {
        //private IDisposable _server;
        private IAppBuilder _appBuilder;
        private IFluentMockServer _mockServer;

        private OwinHostedHttpServer(IAppBuilder appBuilder)
        {
            _appBuilder = appBuilder;
        }
        public static OwinHostedHttpServer New(IAppBuilder appBuilder)
        {
            return new OwinHostedHttpServer(appBuilder);
        }

        public void Start(string urlPrefix, IFluentMockServer mockServer)
        {
            _mockServer = mockServer;
            //Mapp API Mock Server controls
            _appBuilder.MapWhen(context => context.IsServerControllerApiRequest(), app =>
            {
                app.Use<OwinMockServerControllerApiMiddleware>(new ApiControllerFluentMockServerWrapper(mockServer));

                ApiServerControllerStartup.ConfigureApp(app);
            });

            Action<IOwinContext> act = context => _mockServer.HandleRequest(context);
            var bla = _appBuilder.Use<OwinHttpMockServerMiddleware>(act);
        }

       

        public void Stop()
        {
            
            //managed by SF
        }

    }
}