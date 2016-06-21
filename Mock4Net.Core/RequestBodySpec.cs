using System;

namespace Mock4Net.Core
{
    internal class RequestBodySpec : ISpecifyRequests
    {
        private string _body;

        internal RequestBodySpec(string body)
        {
            _body = body.Trim();
        }

        public bool IsSatisfiedBy(Request request)
        {
            return WildcardPatternMatcher.MatchWildcardString(_body, request.Body.Trim());
        }
    }
}
