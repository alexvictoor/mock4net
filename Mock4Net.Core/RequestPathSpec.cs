using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class RequestPathSpec : ISpecifyRequests
    {
        private readonly string _path;

        public RequestPathSpec(string path)
        {
            _path = path;
        }

        public bool IsSatisfiedBy(Request request)
        {
            return WildcardPatternMatcher.MatchWildcardString(_path, request.Path);
        }
    }
}
