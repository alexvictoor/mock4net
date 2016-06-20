using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Mock4Net.Core
{
    public class HttpListenerResponseMapper
    {

        public void Map(Response response, HttpListenerResponse result)
        {
            result.StatusCode = response.StatusCode;
            response.Headers.ToList().ForEach(pair => result.AddHeader(pair.Key, pair.Value));
            if (response.Body != null)
            {
                result.ContentType = "application/json";
                var content = Encoding.UTF8.GetBytes(response.Body);
                result.OutputStream.Write(content, 0, content.Length);
            }
        }

        internal void Map(Response response, IOwinResponse result)
        {
            result.StatusCode = response.StatusCode;
            response.Headers.ToList().ForEach(pair => result.Headers.Append(pair.Key, pair.Value));
            if (response.Body != null)
            {
                result.ContentType = "application/json";
                var content = Encoding.UTF8.GetBytes(response.Body);
                result.Body.Write(content, 0, content.Length);
            }
        }
    }
}
