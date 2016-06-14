using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Mock4Net.Core.Http
{
    public class OwinHttpServerMiddleware : OwinMiddleware
    {
        private Action<HttpListenerContext> _handler;

        public OwinHttpServerMiddleware(OwinMiddleware next, Action<HttpListenerContext> httpHandler)
            : base(next)
        {
            _handler = httpHandler;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var httpContext = context.Environment["System.Net.HttpListenerContext"] as System.Net.HttpListenerContext;
            if (httpContext != null)
            {
                _handler(httpContext);
            }
        }
    }
}