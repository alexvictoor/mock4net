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
        private readonly ISpecifyRequests _requestSpec;
        private readonly Response _response;

        public Route(ISpecifyRequests requestSpec, Response response)
        {
            _requestSpec = requestSpec;
            _response = response;
        }

        public Response Response
        {
            get { return _response; }
        }

        public bool IsRequestHandled(Request request)
        {
            return _requestSpec.IsSatisfiedBy(request);
        }


    }
}
