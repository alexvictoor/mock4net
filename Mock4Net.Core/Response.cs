﻿using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mock4Net.Core
{
    public class Response
    {
        private readonly IDictionary<string, string> _headers = new ConcurrentDictionary<string, string>();

        public volatile int StatusCode = 200;

        public volatile string Body;

        public IDictionary<string, string> Headers
        {
            get { return _headers; }
        }

        public void AddHeader(string name, string value)
        {
            _headers.Add(name, value);
        }
    }
}