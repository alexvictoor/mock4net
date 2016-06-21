using System;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace Mock4Net.Core.Http
{
    internal static class OwinStartup
    {
        internal static void ConfigureControllerApi(IAppBuilder appBuilder)
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

        internal static void Configure(IFluentMockServer mockServer, IAppBuilder builder, string apiKey)
        {
            builder.MapWhen(context => context.IsServerControllerApiRequest(), app =>
            {
                app.Use<OwinMockServerControllerAuthenticationMiddleware>(apiKey);
                app.Use<OwinMockServerControllerApiMiddleware>(new ApiControllerFluentMockServerWrapper(mockServer));

                ConfigureControllerApi(app);
            });

            var act = new Action<IOwinContext>(mockServer.HandleRequest);
            builder.Use<OwinHttpMockServerMiddleware>(act);
        }
    }
}