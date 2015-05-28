using System;

namespace Mock4Net.Core
{
    public class RequestBodySpec : ISpecifyRequests
    {
        private string _body;

        public RequestBodySpec(string body)
        {
            _body = body.Trim();
        }

        public bool IsSatisfiedBy(Request request)
        {
            return _body == request.Body.Trim();
        }
    }
}