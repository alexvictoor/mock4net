using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class HttpListenerRequestMapper 
    {

        public Request Map(HttpListenerRequest listenerRequest)
        {
            var url = listenerRequest.Url.PathAndQuery;
            var verb = listenerRequest.HttpMethod;
            var body = GetRequestBody(listenerRequest);
            var listenerHeaders = listenerRequest.Headers;
            var headers = listenerHeaders.AllKeys.ToDictionary(k => k, k => listenerHeaders[k]);

            return new Request(url, verb, body, headers);
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
    }
}
