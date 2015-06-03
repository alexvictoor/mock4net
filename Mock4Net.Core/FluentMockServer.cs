using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
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

        private FluentMockServer(int port)
        {
            _httpServer = new TinyHttpServer("http://localhost:" + port + "/", HandleRequest);
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

        public void ResetRequestLogs()
        {
            lock (((ICollection) _requestLogs).SyncRoot)
            {
                _requestLogs.Clear();
            }
        }

        public IEnumerable<Request> SearchRequestLogsFor(ISpecifyRequests spec)
        {
            lock (((ICollection)_requestLogs).SyncRoot)
            {
                return _requestLogs.Where(spec.IsSatisfiedBy);
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


        private void HandleRequest(HttpListenerContext ctx)
        {
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
                var response = targetRoute.ResponseTo(request);
                _responseMapper.Map(response, ctx.Response);
            }
            ctx.Response.Close();
        }

        public static FluentMockServer Start(int port = 0)
        {
            if (port == 0)
            {
                port = Ports.FindFreeTcpPort();
            }
            return new FluentMockServer(port);
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
