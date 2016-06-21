using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock4Net.MockServerController.Models;

namespace Mock4Net.Core
{
    internal class ApiControllerFluentMockServerWrapper: IMockServerManager
    {
        private readonly IFluentMockServer _mockServer;

        internal ApiControllerFluentMockServerWrapper(IFluentMockServer mockServer)
        {
            _mockServer = mockServer;
        }


        public void AddMock(RequestCondition requestCondition, MockServerController.Models.Response response)
        {
            var condition = MapMock4NetCondition(requestCondition);
            var mockResponse = MapMock4NetResponse(response);

            _mockServer
              .Given(condition)
              .RespondWith(mockResponse);
            
        }

        public void Reset()
        {
            _mockServer.Reset();
        }
        public List<MockServerController.Models.Request> SearchLogsFor(RequestCondition condition)
        {
            var m4NetCondition = MapMock4NetCondition(condition);
            var result = _mockServer.SearchLogsFor(m4NetCondition)
                .Select(MapMock4NetRequest).ToList();

            return result;
        }

        private RequestVerb GetVerb(string verb)
        {
            if (string.Compare(verb, "get", StringComparison.InvariantCultureIgnoreCase) == 0)
                return RequestVerb.Get;


            if (string.Compare(verb, "post", StringComparison.InvariantCultureIgnoreCase) == 0)
                return RequestVerb.Post;


            if (string.Compare(verb, "put", StringComparison.InvariantCultureIgnoreCase) == 0)
                return RequestVerb.Put;

            if (string.Compare(verb, "head", StringComparison.InvariantCultureIgnoreCase) == 0)
                return RequestVerb.Head;

            return RequestVerb.Any;

        }

        private MockServerController.Models.Request MapMock4NetRequest(Request arg)
        {
            var request = new MockServerController.Models.Request()
            {

                Body = arg.Body,
                URl = arg.Url,
                Path = arg.Path,
                Verb = GetVerb(arg.Verb),
                Headers = new Dictionary<string, string>(),
                Params = new Dictionary<string, List<string>>()
            };

            if (arg.Headers != null)
            {
                foreach (var header in arg.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (arg.Params != null)
            {
                foreach (var param in arg.Params)
                {
                    request.Params.Add(param.Key, param.Value.ToList());
                }
            }
            return request;

        }
        private Responses MapMock4NetResponse(MockServerController.Models.Response response)
        {
            var mockResponse = (Responses)Responses.WithStatusCode(response.StatusCode);

            if (response.Headers != null && response.Headers.Count > 0)
            {
                foreach (var header in response.Headers)
                {
                    mockResponse = (Responses)mockResponse.WithHeader(header.Key, header.Value);
                }
            }

            if (!String.IsNullOrWhiteSpace(response.Body))
            {
                mockResponse = (Responses)mockResponse.WithBody(response.Body);
            }

            if (response.Delay.TotalSeconds > 0)
            {
                mockResponse = (Responses)mockResponse.AfterDelay(response.Delay);
            }
            return mockResponse;
        }

        private Requests MapMock4NetCondition(RequestCondition requestCondition)
        {
            var condition = (Requests)(String.IsNullOrWhiteSpace(requestCondition.URl) ? Requests.WithPath(requestCondition.Path) : Requests.WithUrl(requestCondition.URl));

            condition = (Requests)SetVerb(requestCondition, condition);

            if (requestCondition.Headers != null && requestCondition.Headers.Count > 0)
            {
                foreach (var header in requestCondition.Headers)
                {
                    condition = (Requests)condition.WithHeader(header.Key, header.Value);
                }
            }
            if (!String.IsNullOrWhiteSpace(requestCondition.Body))
            {
                condition = (Requests)condition.WithBody(requestCondition.Body);
            }

            if (requestCondition.Params != null && requestCondition.Params.Count > 0)
            {
                foreach (var param in requestCondition.Params)
                {
                    condition = (Requests)condition.WithParam(param.Key, param.Value.ToArray());
                }
            }

            return condition;
        }

        private IHeadersRequestBuilder SetVerb(RequestCondition requestCondition, IVerbRequestBuilder verbRequestBuilder)
        {
            switch (requestCondition.Verb)
            {
                case RequestVerb.Any:
                    return verbRequestBuilder.UsingAnyVerb();
                case RequestVerb.Post:
                    return verbRequestBuilder.UsingPost();
                case RequestVerb.Put:
                    return verbRequestBuilder.UsingPut();
                case RequestVerb.Head:
                    return verbRequestBuilder.UsingHead();
                default:
                    return verbRequestBuilder.UsingGet();

            }
        }
    }
}
