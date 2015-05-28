using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mock4Net.Core.Http
{
    public class TinyHttpServer
    {
        private readonly Action<HttpListenerContext> _httpHandler;
        private HttpListener _listener;
        private CancellationTokenSource _cts;

        public TinyHttpServer(string urlPrefix, Action<HttpListenerContext> httpHandler)
        {
            _httpHandler = httpHandler;
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            // Create a listener.
            _listener = new HttpListener();
            _listener.Prefixes.Add(urlPrefix);
        }

        public void Start()
        {
            _listener.Start();
            _cts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                using (_listener)
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        HttpListenerContext context = await _listener.GetContextAsync();
                        _httpHandler.Invoke(context);
                    }
                }
            }, _cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();

        }
    }
}
