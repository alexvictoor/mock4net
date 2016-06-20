using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mock4Net.Core.Http
{
    public class TinyHttpServer : IHttpServer
    {
        private  HttpListener _listener;
        private CancellationTokenSource _cts;
        private IFluentMockServer _mockServer;

        public TinyHttpServer()
        {
       
        }

        public void Start(string urlPrefix,  IFluentMockServer mockServer)
        {
           // _httpHandler = httpHandler;
            _mockServer = mockServer;
            /*  .Net Framework is not supportted on XP or Server 2003, so no need for the check
                        if (!HttpListener.IsSupported)
                        {
                            Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                            return;
                        }
             */
            // Create a listener.
            _listener = new HttpListener();
            _listener.Prefixes.Add(urlPrefix);

            _listener.Start();
            _cts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                using (_listener)
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        HttpListenerContext context = await _listener.GetContextAsync();
                        _mockServer.HandleRequest(context);
                        context.Response.Close();
                    }
                }
            }
            , _cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();

        }

    }
}
