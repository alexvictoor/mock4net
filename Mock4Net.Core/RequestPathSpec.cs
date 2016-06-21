using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    internal class RequestPathSpec : ISpecifyRequests
    {
        private readonly string _path;

        internal RequestPathSpec(string path)
        {
            _path = path;
        }

        public bool IsSatisfiedBy(Request request)
        {
            return WildcardPatternMatcher.MatchWildcardString(_path, request.Path);
        }
    }
}
