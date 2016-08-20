using System;

namespace Mock4Net.Core
{
    public interface IHeadersResponseBuilder : IBodyResponseBuilder
    {
        IHeadersResponseBuilder WithHeader(string name, string value);
    }

    public interface IBodyResponseBuilder : IDelayResponseBuilder
    {
        IDelayResponseBuilder WithBody(string body);
    }

    public interface IDelayResponseBuilder : IProvideResponses
    {
        IProvideResponses AfterDelay(TimeSpan delay);
    }
}
