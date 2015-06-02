using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class CompositeRequestSpec : ISpecifyRequests
    {
        private readonly IEnumerable<ISpecifyRequests> _requestSpecs;

        public CompositeRequestSpec(IEnumerable<ISpecifyRequests> requestSpecs)
        {
            _requestSpecs = requestSpecs;
        }

        public bool IsSatisfiedBy(Request request)
        {
            return _requestSpecs.All(spec => spec.IsSatisfiedBy(request));
        }
    }
}
