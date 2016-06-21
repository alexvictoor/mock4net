using System.Threading.Tasks;
using Microsoft.Owin;

namespace Mock4Net.Core.Http
{
    public class OwinMockServerControllerApiMiddleware : OwinMiddleware
    {
        private readonly IMockServerManager _mockServerManager;
        //private Action<HttpListenerContext> _handler;

        public OwinMockServerControllerApiMiddleware(OwinMiddleware next, IMockServerManager mockServerManager)
            : base(next)
        {
            _mockServerManager = mockServerManager;
            //_handler = httpHandler;
        }

        public override async Task Invoke(IOwinContext context)
        {
            context.Set("FluentMockServer", _mockServerManager);
            
            await Next.Invoke(context);
        }
    }
}