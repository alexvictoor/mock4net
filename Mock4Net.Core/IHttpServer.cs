using System;
using System.Net;

namespace Mock4Net.Core.Http
{
    public interface IHttpServer
    {
        void Stop();
        //void Start(string v, Action<object> handleRequest, IFluentMockServer fluentMockServer);
        void Start(string v, IFluentMockServer fluentMockServer);
    }
}