using System.Collections.Generic;

namespace Mock4Net.MockServerController.Models
{
    public class RequestCondition 
    {

        private RequestVerb _verb = RequestVerb.Any;

        public RequestVerb Verb
        {
            get { return _verb; }
            set { _verb = value; }
        }

        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, List<string>> Params { get; set; }
        public string Body { get; set; }
        public string URl { get; set; }
        public string Path { get; set; }
    }
}