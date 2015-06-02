using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public interface IVerbRequestBuilder : ISpecifyRequests
    {

        IHeadersRequestBuilder UsingGet();
        IHeadersRequestBuilder UsingPost();
        IHeadersRequestBuilder UsingPut();
        IHeadersRequestBuilder UsingHead();
        IHeadersRequestBuilder UsingAnyVerb();
        IHeadersRequestBuilder UsingVerb(string verb);
    }

    public interface IHeadersRequestBuilder : IBodyRequestBuilder, ISpecifyRequests
    {
        IHeadersRequestBuilder WithHeader(string name, string value);
    }

    public interface IBodyRequestBuilder
    {
        ISpecifyRequests WithBody(string body);
    }

}
