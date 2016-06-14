using System;
using System.Net;
using Microsoft.Owin.Hosting;
using Mock4Net.Core.Http;
using Owin;

namespace Mock4Net.Core.Http
{
    public class OwinHttpServer : IHttpServer
    {
        private IDisposable _server;

        public void Start(string urlPrefix, Action<HttpListenerContext> httpHandler)
        {
            Stop();
            _server = WebApp.Start(new StartOptions(urlPrefix), builder =>
            {
                builder.Use<OwinHttpServerMiddleware>(httpHandler);

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


    public class OwinServiceFabricHttpServer : IHttpServer
    {
        //private IDisposable _server;
        private IAppBuilder _appBuilder;

        private OwinServiceFabricHttpServer(IAppBuilder appBuilder)
        {
            _appBuilder = appBuilder;
        }
        public static OwinServiceFabricHttpServer New(IAppBuilder appBuilder)
        {
            return new OwinServiceFabricHttpServer(appBuilder);
        }

        public void Start(string urlPrefix, Action<HttpListenerContext> httpHandler)
        {
            //Stop();
            var bla = _appBuilder.Use<OwinHttpServerMiddleware>(httpHandler);
        }

        public void Stop()
        {

            //managed by SF
        }

    }
}