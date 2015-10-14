using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class Requests : CompositeRequestSpec, IVerbRequestBuilder, IHeadersRequestBuilder, IParamsRequestBuilder
    {
        private readonly IList<ISpecifyRequests> _requestSpecs;

        private Requests(IList<ISpecifyRequests> requestSpecs) : base(requestSpecs)
        {
            _requestSpecs = requestSpecs;
        }

        public static IVerbRequestBuilder WithUrl(string url)
        {
            var specs = new List<ISpecifyRequests>();
            var requests = new Requests(specs);
            specs.Add(new RequestUrlSpec(url));
            return requests;
        }

        public static IVerbRequestBuilder WithPath(string path)
        {
            var specs = new List<ISpecifyRequests>();
            var requests = new Requests(specs);
            specs.Add(new RequestPathSpec(path));
            return requests;
        }

        public IHeadersRequestBuilder UsingGet()
        {
            _requestSpecs.Add(new RequestVerbSpec("get"));
            return this;
        }

        public IHeadersRequestBuilder UsingPost()
        {
            _requestSpecs.Add(new RequestVerbSpec("post"));
            return this;
        }

        public IHeadersRequestBuilder UsingPut()
        {
            _requestSpecs.Add(new RequestVerbSpec("put"));
            return this;
        }

        public IHeadersRequestBuilder UsingHead()
        {
            _requestSpecs.Add(new RequestVerbSpec("head"));
            return this;
        }

        public IHeadersRequestBuilder UsingAnyVerb()
        {
            return this;
        }

        public IHeadersRequestBuilder UsingVerb(string verb)
        {
            _requestSpecs.Add(new RequestVerbSpec(verb));
            return this;
        }

        public ISpecifyRequests WithBody(string body)
        {
            _requestSpecs.Add(new RequestBodySpec(body));
            return this;
        }

        public ISpecifyRequests WithParam(string key, params string[] values)
        {
            _requestSpecs.Add(new RequestParamSpec(key, values.ToList()));
            return this;
        }

        public IHeadersRequestBuilder WithHeader(string name, string value)
        {
            _requestSpecs.Add(new RequestHeaderSpec(name, value));
            return this;
        }

        

    }
}
