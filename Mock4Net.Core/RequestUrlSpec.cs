﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class RequestUrlSpec : ISpecifyRequests
    {
        private readonly string _url;

        public RequestUrlSpec(string url)
        {
            _url = url;
        }

        public bool IsSatisfiedBy(Request request)
        {
            return WildcardPatternMatcher.MatchWildcardString(_url, request.Url,true);
        }
    }
}
