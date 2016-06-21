using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace Mock4Net.Core
{
    public class Response
    {

        private readonly IDictionary<string, string> _headers = new ConcurrentDictionary<string, string>();

        internal volatile int StatusCode = 200;

        internal volatile string Body;


        internal IDictionary<string, string> Headers
        {
            get { return _headers; }
        }

        internal void AddHeader(string name, string value)
        {
            _headers.Add(name, value);
        }

    }
}