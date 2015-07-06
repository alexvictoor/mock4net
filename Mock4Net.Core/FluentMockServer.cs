using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mock4Net.Core.Http;

namespace Mock4Net.Core
{
    public class FluentMockServer
    {

        private readonly TinyHttpServer _httpServer;
        private readonly IList<Route> _routes = new List<Route>(); 
        private readonly IList<Request> _requestLogs = new List<Request>(); 
        private readonly HttpListenerRequestMapper _requestMapper = new HttpListenerRequestMapper();
        private readonly HttpListenerResponseMapper _responseMapper = new HttpListenerResponseMapper();
        private readonly int _port;
        private TimeSpan _requestProcessingDelay = TimeSpan.Zero;

        private FluentMockServer(int port, bool ssl)
        {
            string protocol = ssl ? "https" : "http";
            _httpServer = new TinyHttpServer(protocol + "://localhost:" + port + "/", HandleRequest);
            _port = port;
            _httpServer.Start();
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
            lock (this)
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

        private async void HandleRequest(HttpListenerContext ctx)
        {
            lock (this)
            {
                Task.Delay(_requestProcessingDelay).Wait();
            }
            var request = _requestMapper.Map(ctx.Request);
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
            ctx.Response.Close();
        }

        public static FluentMockServer Start(int port = 0, bool ssl = false)
        {
            if (port == 0)
            {
                port = Ports.FindFreeTcpPort();
            }
            return new FluentMockServer(port, ssl);
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
