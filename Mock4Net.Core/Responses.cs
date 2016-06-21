using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class Responses : IHeadersResponseBuilder
    {
        private readonly Response _response;
        private TimeSpan _delay = TimeSpan.Zero;


        public Responses(Response response)
        {
            _response = response;
        }

        public static IHeadersResponseBuilder WithStatusCode(int code)
        {
            var response = new Response(){StatusCode = code}; 
            return new Responses(response);
        }

        public async Task<Response> ProvideResponse(Request request)
        {
            await Task.Delay(_delay);
            return _response;
        }

        public IHeadersResponseBuilder WithHeader(string name, string value)
        {
            _response.AddHeader(name, value);
            return this;
        }

        public IDelayResponseBuilder WithBody(string body)
        {
            _response.Body = body;
            return this;
        }

        public IProvideResponses AfterDelay(TimeSpan delay)
        {
            _delay = delay;
            return this;
        }
    }
}
