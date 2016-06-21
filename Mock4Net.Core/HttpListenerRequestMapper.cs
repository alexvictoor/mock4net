using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Mock4Net.Core
{
    internal class HttpListenerRequestMapper 
    {

        internal Request Map(HttpListenerRequest listenerRequest)
        {
            var path = listenerRequest.Url.AbsolutePath;
            var query = listenerRequest.Url.Query;
            var verb = listenerRequest.HttpMethod;
            var body = GetRequestBody(listenerRequest);
            var listenerHeaders = listenerRequest.Headers;
            var headers = listenerHeaders.AllKeys.ToDictionary(k => k, k => listenerHeaders[k]);

            return new Request(path, query, verb, body, headers);
        }

        private string GetRequestBody(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                return null;
            }
            using (var body = request.InputStream)
            {
                using (var reader = new StreamReader(body, request.ContentEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }


        private string GetRequestBody(IOwinRequest request)
        {
            
            using (var body = request.Body)
            {
                using (var reader = new StreamReader(body))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        internal Request Map(IOwinRequest request)
        {
            var path = request.Uri.AbsolutePath;
            var query = request.Uri.Query;
            var verb = request.Method;
            var body = GetRequestBody(request);
            var listenerHeaders = request.Headers;
            var headers = listenerHeaders.Keys.ToDictionary(k => k, k => listenerHeaders[k]);

            return new Request(path, query, verb, body, headers);
        }
    }
}
