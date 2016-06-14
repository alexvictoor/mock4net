using System;
using System.Net;

namespace Mock4Net.Core.Http
{
    public interface IHttpServer
    {
        void Start(string urlPrefix, Action<HttpListenerContext> httpHandler);
        void Stop();
    }
}