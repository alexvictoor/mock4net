using System;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace Mock4Net.Core.Http
{
    public static class OwinStartup
    {
        private static void ConfigureControllerApi(IAppBuilder appBuilder)
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

        public static void Configure(IFluentMockServer mockServer, IAppBuilder builder, string apiKey)
        {
            builder.MapWhen(context => context.IsServerControllerApiRequest(), app =>
            {
                app.Use<OwinMockServerControllerAuthenticationMiddleware>(apiKey);
                app.Use<OwinMockServerControllerApiMiddleware>(new ApiControllerFluentMockServerWrapper(mockServer));

                ConfigureControllerApi(app);
            });

            Action<IOwinContext> act = new Action<IOwinContext>(context => mockServer.HandleRequest(context));
            builder.Use<OwinHttpMockServerMiddleware>(act);
        }
    }
}