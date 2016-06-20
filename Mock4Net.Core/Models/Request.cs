using System.Collections.Generic;

namespace Mock4Net.Core.Models
{
    public class Request
    {
        public RequestVerb Verb { get; set; }
        public string URl { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, List<string>> Params { get; set; }
        public string Body { get; set; }

    }
}