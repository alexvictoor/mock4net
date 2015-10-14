using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public interface IVerbRequestBuilder : ISpecifyRequests, IHeadersRequestBuilder
    {

        IHeadersRequestBuilder UsingGet();
        IHeadersRequestBuilder UsingPost();
        IHeadersRequestBuilder UsingPut();
        IHeadersRequestBuilder UsingHead();
        IHeadersRequestBuilder UsingAnyVerb();
        IHeadersRequestBuilder UsingVerb(string verb);
    }

    public interface IHeadersRequestBuilder : IBodyRequestBuilder, ISpecifyRequests, IParamsRequestBuilder
    {
        IHeadersRequestBuilder WithHeader(string name, string value);
    }

    public interface IBodyRequestBuilder
    {
        ISpecifyRequests WithBody(string body);
    }

    public interface IParamsRequestBuilder
    {
        ISpecifyRequests WithParam(string key, params string[] values);
    }

}
