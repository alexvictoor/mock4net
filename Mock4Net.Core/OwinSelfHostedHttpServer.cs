using System;
using System.Net;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Mock4Net.Core.Http;
using Owin;

namespace Mock4Net.Core.Http
{
    public class OwinSelfHostedHttpServer : IHttpServer
    {
        private IDisposable _server;
        private IFluentMockServer _mockServer;

        public void Start(string urlPrefix, IFluentMockServer mockServer)
        {
            _mockServer = mockServer;
            Stop();
            _server = WebApp.Start(new StartOptions(urlPrefix), builder =>
            {
                builder.MapWhen(context => context.IsServerControllerApiRequest(), app =>
                {
                    app.Use<OwinMockServerControllerApiMiddleware>(new ApiControllerFluentMockServerWrapper(mockServer));

                    HttpConfiguration config = new HttpConfiguration();
                    
                    config.Routes.MapHttpRoute(
                        name: "DefaultApi",
                        routeTemplate: "api/{controller}/{action}/{id}",
                        defaults: new { id = RouteParameter.Optional }
                    );

                    app.UseWebApi(config);
                });

                Action<IOwinContext> act = new Action<IOwinContext>(context => _mockServer.HandleRequest(context));
                builder.Use<OwinHttpMockServerMiddleware>(act);

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

                HttpConfiguration config = new HttpConfiguration();

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{action}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                app.UseWebApi(config);
            });

            Action<IOwinContext> act = context => _mockServer.HandleRequest(context);
            var bla = _appBuilder.Use<OwinHttpMockServerMiddleware>(act);
        }

       

        public void Stop()
        {
            
            //managed by SF
        }

    }

    public static class Helpers
    {

        public static bool IsServerControllerApiRequest(this IOwinContext context)
        {
            //TODO: Add some form of Key validation for basic security...
            return context.Request.Headers.ContainsKey("X-MockServerControllerApiKey");
        }
    }

    public static class ApiServerControllerStartup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public static void ConfigureApp(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            appBuilder.UseWebApi(config);
        }
    }
}