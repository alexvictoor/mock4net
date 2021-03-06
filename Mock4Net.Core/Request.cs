﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class Request
    {
        private readonly string _path;

        private readonly Dictionary<string, List<string>>_params = new Dictionary<string, List<string>>();

        private readonly string _verb;

        private readonly IDictionary<string, string> _headers;

        private readonly string _body;

        public Request(string path, string query, string verb, string body, IDictionary<string, string> headers)
        {
            if (!string.IsNullOrEmpty(query))
            {
                if (query.StartsWith("?"))
                    query = query.Substring(1);
                _params = query.Split('&')
                    .Aggregate(new Dictionary<string, List<string>>(), (dict, term) =>
                    {
                        var key = term.Split('=')[0];
                        if (!dict.ContainsKey(key))
                            dict.Add(key, new List<string>());
                        dict[key].Add(term.Split('=')[1]);
                        return dict;
                    });
            }
            _path = path;
            _headers = headers.ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value.ToLower());
            _verb = verb.ToLower();
            _body = body==null ? "" : body.Trim();
        }

        public string Url
        {
            get
            {
                if (!_params.Any())
                    return _path;
                return _path + "?" + string.Join("&", _params.SelectMany(kv => kv.Value.Select(value => kv.Key + "=" + value)));
            }
        }

        public string Path
        {
            get { return _path; }
        }

        public List<string> GetParameter(string key)
        {
            if (_params.ContainsKey(key))
                return _params[key];
            return new List<string>();
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
