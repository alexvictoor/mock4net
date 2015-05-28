using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class Request
    {
        private readonly string _url;

        private readonly string _verb;

        private readonly IDictionary<string, string> _headers;

        private readonly string _body;

        public Request(string url, string verb, string body, IDictionary<string, string> headers)
        {
            _url = url;
            _headers = headers.ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value.ToLower());
            _verb = verb.ToLower();
            _body = body.Trim();
        }

        public string Url
        {
            get { return _url; }
        }

        public string Verb
        {
            get { return _verb; }
        }

        public IDictionary<string, string> Headers
        {
            get { return _headers; }
        }

        public string Body
        {
            get { return _body; }
        }
    }
}
