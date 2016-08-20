namespace Mock4Net.Core
{
    public class RequestHeaderSpec : ISpecifyRequests
    {
        private readonly string _name;
        private readonly string _pattern;

        public RequestHeaderSpec(string name, string pattern)
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