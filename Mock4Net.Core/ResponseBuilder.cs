using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    /*public interface IStatusCodeResponseBuilder : IHeadersResponseBuilder
    {
        IHeadersResponseBuilder WithStatusCode(int code);
    }*/

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
