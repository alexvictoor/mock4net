﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1633:FileMustHaveHeader",
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]

namespace Mock4Net.Core.Http
{
    /// <summary>
    /// The tiny http server.
    /// </summary>
    public class TinyHttpServer
    {
        /// <summary>
        /// The _http handler.
        /// </summary>
        private readonly Action<HttpListenerContext> _httpHandler;

        /// <summary>
        /// The _listener.
        /// </summary>
        private readonly HttpListener _listener;

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpServer"/> class.
        /// </summary>
        /// <param name="urlPrefix">
        /// The url prefix.
        /// </param>
        /// <param name="httpHandler">
        /// The http handler.
        /// </param>
        public TinyHttpServer(string urlPrefix, Action<HttpListenerContext> httpHandler)
        {
            _httpHandler = httpHandler;
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
        }

        /// <summary>
        /// The start.
        /// </summary>
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
                        _httpHandler(context);
                    }
                }
            }

            , _cts.Token);
        }

        /// <summary>
        /// The stop.
        /// </summary>
        public void Stop()
        {
            _cts.Cancel();
        }
    }
}
