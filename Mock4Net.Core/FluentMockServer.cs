﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin;
using Mock4Net.Core.Http;
using Newtonsoft.Json;

namespace Mock4Net.Core
{
    public interface IFluentMockServer
    {
        int Port { get; }
        IEnumerable<Request> RequestLogs { get; }
        void Reset();
        IEnumerable<Request> SearchLogsFor(ISpecifyRequests spec);
        void AddRequestProcessingDelay(TimeSpan delay);
        void Stop();
        FluentMockServer.IRespondWithAProvider Given(ISpecifyRequests requestSpec);
        void HandleRequest(HttpListenerContext ctx);
        void HandleRequest(IOwinContext ctx);
    }

    public class FluentMockServer : IFluentMockServer
    {

        private readonly IHttpServer _httpServer;
        private readonly IList<Route> _routes = new List<Route>(); 
        private readonly IList<Request> _requestLogs = new List<Request>(); 
        private readonly HttpListenerRequestMapper _requestMapper = new HttpListenerRequestMapper();
        private readonly HttpListenerResponseMapper _responseMapper = new HttpListenerResponseMapper();
        private readonly int _port;
        private TimeSpan _requestProcessingDelay = TimeSpan.Zero;
        private object _syncRoot = new object();

        private FluentMockServer(int port, bool ssl) : this(port,ssl, new TinyHttpServer())
        {
        }

        private FluentMockServer(int port, bool ssl, IHttpServer httpServer)
        {
            string protocol = ssl ? "https" : "http";
            _httpServer = httpServer;
            _port = port;
            _httpServer.Start(protocol + "://+:" + port + "/", this);
        }

        public int Port
        {
            get { return _port; }
        }

        public IEnumerable<Request> RequestLogs
        {
            get
            {
                lock (((ICollection) _requestLogs).SyncRoot)
                {
                    return new ReadOnlyCollection<Request>(_requestLogs);
                }
            }
        }

        public void Reset()
        {
            lock (((ICollection) _requestLogs).SyncRoot)
            {
                _requestLogs.Clear();
            }
            lock (((ICollection)_routes).SyncRoot)
            {
                _routes.Clear();
            }
        }

        public IEnumerable<Request> SearchLogsFor(ISpecifyRequests spec)
        {
            lock (((ICollection)_requestLogs).SyncRoot)
            {
                return _requestLogs.Where(spec.IsSatisfiedBy);
            }
        }

        public void AddRequestProcessingDelay(TimeSpan delay)
        {
            lock (_syncRoot)
            {
                _requestProcessingDelay = delay;
            }
        }

        private void RegisterRoute(Route route)
        {
            lock (((ICollection)_routes).SyncRoot)
            {
                _routes.Add(route);
            }
        }

        private void LogRequest(Request request)
        {
            lock (((ICollection)_requestLogs).SyncRoot)
            {
                _requestLogs.Add(request);
            }
        }

        public async void HandleRequest(HttpListenerContext ctx)
        {

            var request = _requestMapper.Map(ctx.Request);

            if (HandleLogRequest(ctx, request)) return;


            lock (_syncRoot)
            {
                Task.Delay(_requestProcessingDelay).Wait();
            }

            LogRequest(request);
            var targetRoute = _routes.FirstOrDefault(route => route.IsRequestHandled(request));
            if (targetRoute == null)
            {
                ctx.Response.StatusCode = 404;
                var content = Encoding.UTF8.GetBytes("<html><body>Mock Server: page not found</body></html>");
                ctx.Response.OutputStream.Write(content, 0, content.Length);
            }
            else
            {
                var response = await targetRoute.ResponseTo(request);

                _responseMapper.Map(response, ctx.Response);
            }

            //ctx.Response.Close();
        }

        public async void HandleRequest(IOwinContext ctx)
        {

            var request = _requestMapper.Map(ctx.Request);

          //  if (HandleLogRequest(ctx, request)) return;


            lock (_syncRoot)
            {
                Task.Delay(_requestProcessingDelay).Wait();
            }

            LogRequest(request);
            var targetRoute = _routes.FirstOrDefault(route => route.IsRequestHandled(request));
            if (targetRoute == null)
            {
                ctx.Response.StatusCode = 404;
                var content = Encoding.UTF8.GetBytes("<html><body>Mock Server: page not found</body></html>");
                ctx.Response.Body.Write(content, 0, content.Length);
            }
            else
            {
                var response = await targetRoute.ResponseTo(request);

                _responseMapper.Map(response, ctx.Response);
            }

            //ctx.Response.Close();
        }

        private bool HandleLogRequest(HttpListenerContext ctx, Request request)
        {
            if (request.Path == "/RequestLogs" && request.Verb == "get")
            {
                var jsonResponse = JsonConvert.SerializeObject(RequestLogs, Formatting.Indented);
                ctx.Response.StatusCode = 200;
                ctx.Response.ContentType = "application/json";
                var content = Encoding.UTF8.GetBytes(jsonResponse);
                ctx.Response.OutputStream.Write(content, 0, content.Length);
                //ctx.Response.Close();
                return true;
            }
            return false;
        }

        public static IFluentMockServer Start(int port = 0, bool ssl = false)
        {
           return Start(new TinyHttpServer(),port,ssl);
        }

        public static FluentMockServer Start(IHttpServer server, int port = 0, bool ssl = false)
        {
            if (port == 0)
            {
                port = Ports.FindFreeTcpPort();
            }
            return new FluentMockServer(port, ssl,server);
        }

        public void Stop()
        {
            _httpServer.Stop();
        }

        public IRespondWithAProvider Given(ISpecifyRequests requestSpec)
        {
            return new RespondWithAProvider(RegisterRoute, requestSpec);
        }

        public interface IRespondWithAProvider
        {
            void RespondWith(IProvideResponses provider);
        }

        class RespondWithAProvider : IRespondWithAProvider
        {
            private readonly RegistrationCallback _registrationCallback;
            private readonly ISpecifyRequests _requestSpec;

            public RespondWithAProvider(RegistrationCallback registrationCallback, ISpecifyRequests requestSpec)
            {
                _registrationCallback = registrationCallback;
                _requestSpec = requestSpec;
            }

            public void RespondWith(IProvideResponses provider)
            {
                _registrationCallback(new Route(_requestSpec, provider));
            }
        }
    }
}
