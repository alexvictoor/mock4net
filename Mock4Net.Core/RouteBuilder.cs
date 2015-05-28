using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class RouteBuilder : FluentMockServer.IVerbRequestBuilder, FluentMockServer.IHeadersRequestBuilder, FluentMockServer.IStatusCodeResponseBuilder
    {
      
        private readonly RegistrationCallback _registrationCallback;
        private readonly Response _response = new Response();

        private readonly  IList<ISpecifyRequests> _specs = new List<ISpecifyRequests>();

        public RouteBuilder(RegistrationCallback registrationCallback, string url)
        {
            _registrationCallback = registrationCallback;
            _specs.Add(new RequestUrlSpec(url));
        }

        public FluentMockServer.IHeadersRequestBuilder Get()
        {
            _specs.Add(new RequestVerbSpec("get"));
            return this;
        }

        public FluentMockServer.IHeadersRequestBuilder Post()
        {
            _specs.Add(new RequestVerbSpec("post"));
            return this;
        }

        public FluentMockServer.IHeadersRequestBuilder Put()
        {
            _specs.Add(new RequestVerbSpec("put"));
            return this;
        }

        public FluentMockServer.IHeadersRequestBuilder Head()
        {
            _specs.Add(new RequestVerbSpec("head"));
            return this;
        }

        public FluentMockServer.IHeadersRequestBuilder AnyVerb()
        {
            return this;
        }

        public FluentMockServer.IHeadersRequestBuilder Verb(string verb)
        {
            _specs.Add(new RequestVerbSpec(verb.ToLower()));
            return this;
        }

        public FluentMockServer.IStatusCodeResponseBuilder Respond()
        {
            _registrationCallback(new Route(_specs, _response));
            return this;
        }

        public FluentMockServer.IRequestResponseBuilder WithBody(string body)
        {
            _specs.Add(new RequestBodySpec(body));
            return this;
        }

        FluentMockServer.IHeadersResponseBuilder FluentMockServer.IHeadersResponseBuilder.WithHeader(string name, string value)
        {
            _response.AddHeader(name, value);
            return this;
        }

        public FluentMockServer.IHeadersResponseBuilder WithStatusCode(int code)
        {
            _response.StatusCode = code;
            return this;
        }

        public FluentMockServer.IHeadersRequestBuilder WithHeader(string name, string value)
        {
            _specs.Add(new RequestHeaderSpec(name, value));
            return this;
        }

        void FluentMockServer.IBodyResponseBuilder.WithBody(string body)
        {
            _response.Body = body;
        }
    }
}
