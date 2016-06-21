using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    internal class RequestParamSpec : ISpecifyRequests
    {
        private readonly string _key;
        private readonly List<string> _values;

        internal RequestParamSpec(string key, List<string> values)
        {
            _key = key;
            _values = values;
        }

        public bool IsSatisfiedBy(Request request)
        {
            return request.GetParameter(_key).Intersect(_values).Count() == _values.Count;
        }
    }
}
