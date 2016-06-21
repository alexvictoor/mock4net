using System.Collections.Generic;

namespace Mock4Net.Core
{
    internal class RequestHeaderSpec : ISpecifyRequests
    {
        private readonly string _name;
        private readonly string _pattern;

        internal RequestHeaderSpec(string name, string pattern)
        {
            _name = name.ToLower();
            _pattern = pattern.ToLower();
        }

        public bool IsSatisfiedBy(Request request)
        {
            var headerValue = request.Headers[_name];
            return WildcardPatternMatcher.MatchWildcardString(_pattern, headerValue);
        }
    }
}