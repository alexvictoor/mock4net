using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class Route
    {
        private readonly IEnumerable<ISpecifyRequests> _requestSpecs;
        private readonly Response _response;

        public Route(IEnumerable<ISpecifyRequests> requestSpecs, Response response)
        {
            _requestSpecs = requestSpecs;
            _response = response;
        }

        public Response Response
        {
            get { return _response; }
        }

        public bool IsRequestHandled(Request request)
        {
            return _requestSpecs.All(spec => spec.IsSatisfiedBy(request));
        }


    }
}
