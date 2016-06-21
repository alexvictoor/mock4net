using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Owin;

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
}