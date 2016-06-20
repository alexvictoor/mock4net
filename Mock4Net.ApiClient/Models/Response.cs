using System;
using System.Collections.Generic;

namespace Mock4Net.ApiClient.Models
{
    public class Response
    {
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
        public TimeSpan Delay { get; set; }
        public int StatusCode { get; set; }
    }
}