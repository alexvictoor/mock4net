using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    internal class Route
    {
        private readonly ISpecifyRequests _requestSpec;
        private readonly IProvideResponses _provider;

        internal Route(ISpecifyRequests requestSpec, IProvideResponses provider)
        {
            _requestSpec = requestSpec;
            _provider = provider;
        }

        internal Task<Response> ResponseTo(Request request)
        {
            return _provider.ProvideResponse(request);
        }

        internal bool IsRequestHandled(Request request)
        {
            return _requestSpec.IsSatisfiedBy(request);
        }


    }
}
