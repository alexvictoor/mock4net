using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class Responses : IProvideResponses, IHeadersResponseBuilder
    {
        private readonly Response _response;


        public Responses(Response response)
        {
            this._response = response;
        }

        public static IHeadersResponseBuilder WithStatusCode(int code)
        {
            var response = new Response(){StatusCode = code}; 
            return new Responses(response);
        }

        public Response ProvideResponse(Request request)
        {
            return _response;
        }

        public IProvideResponses WithBody(string body)
        {
            _response.Body = body;
            return this;
        }

        public IHeadersResponseBuilder WithHeader(string name, string value)
        {
            _response.AddHeader(name, value);
            return this;
        }
    }
}
