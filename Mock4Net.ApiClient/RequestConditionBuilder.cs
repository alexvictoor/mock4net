using System.Collections.Generic;
using Mock4Net.ApiClient.Models;

namespace Mock4Net.ApiClient
{
    public class RequestConditionBuilder : RequestCondition
    {
        private RequestConditionBuilder()
        {
        }

        public RequestConditionBuilder WithVerb(RequestVerb verb)
        {
            Verb = verb;
            return this;
        }

        public RequestConditionBuilder WithHeader(string name, string value)
        {
            if (Headers.ContainsKey(name))
            {
                Headers[name] = value;
            }
            else
            {
                Headers.Add(name, value);
            }

            return this;
        }

        public RequestConditionBuilder WithParam(string name, string value)
        {
            if (!Params.ContainsKey(name))
            {
                Params.Add(name, new List<string>() { value });
            }

            if (Params.ContainsKey(name) && !Params[name].Contains(value))
            {
                Params[name].Add(value);
            }


            return this;
        }

        public RequestConditionBuilder WithBody(string body)
        {
            Body = body;
            return this;
        }

        public static RequestConditionBuilder WithUrl(string url)
        {
            return new RequestConditionBuilder()
            {
                Verb = RequestVerb.Any,
                Headers = new Dictionary<string, string>(),
                Params = new Dictionary<string, List<string>>(),
                Path = "",
                Body = "",
                URl = url

            };
        }

        public static RequestConditionBuilder WithPath(string path)
        {
            return new RequestConditionBuilder()
            {
                Verb = RequestVerb.Any,
                Headers = new Dictionary<string, string>(),
                Params = new Dictionary<string, List<string>>(),
                Path = path,
                Body = "",
                URl = ""

            };
        }
    }
}