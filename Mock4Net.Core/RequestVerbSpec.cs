using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    class RequestVerbSpec : ISpecifyRequests
    {
        private readonly string _verb;

        public RequestVerbSpec(string verb)
        {
            _verb = verb.ToLower();
        }

        public bool IsSatisfiedBy(Request request)
        {
            return request.Verb == _verb;
        }
    }
}
