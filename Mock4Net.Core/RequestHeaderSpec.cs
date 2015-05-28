using System.Collections.Generic;

namespace Mock4Net.Core
{
    public class RequestHeaderSpec : ISpecifyRequests
    {
        private readonly string _name;
        private readonly string _value;

        public RequestHeaderSpec(string name, string value)
        {
            _name = name.ToLower();
            _value = value.ToLower();
        }

        public bool IsSatisfiedBy(Request request)
        {
            return request.Headers.Contains(new KeyValuePair<string, string>(_name, _value));
        }
    }
}