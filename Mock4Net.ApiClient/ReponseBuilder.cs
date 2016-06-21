using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock4Net.ApiClient.Models;

namespace Mock4Net.ApiClient
{
    public class ResponseBuilder : Response
    {
        private ResponseBuilder()
        {
        }

        public static ResponseBuilder WithStatusCode(int statusCode)
        {
            return new ResponseBuilder()
            {
                Delay = TimeSpan.Zero,
                Headers = new Dictionary<string, string>(),
                StatusCode = statusCode,
                Body = ""
            };
        }

        public ResponseBuilder WithBody(string body)
        {
            Body = body;
            return this;
        }

        public ResponseBuilder WithDelay(TimeSpan delay)
        {
            Delay = delay;
            return this;
        }



        public ResponseBuilder WithHeader(string name, string value)
        {
            if (Headers.ContainsKey(name))
            {
                Headers[name] = value;
            }
            else
            {
                Headers.Add(name, value);
            }

            return this;
        }
    }
}
