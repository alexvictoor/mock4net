using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class FluentMockServer
    {

        private FluentMockServer(int port)
        {
            
        }

        public FluentMockServer StartHttpServer(int port = 0)
        {
            return null;
        }
        
        
        public IVerbRequestBuilder ForRequest(string url = "*")
        {
            return null;
        }

        public void Test()
        {
            ForRequest(url: "/toto")
                .Get()
                .WithHeader("", "")
                .WithHeader("","")
             .Respond()
                .WithBody("");
        }

        public interface IVerbRequestBuilder
        {

            IHeadersRequestBuilder Get();
            IHeadersRequestBuilder Post();
            IHeadersRequestBuilder Put();
            IHeadersRequestBuilder Head();
            IHeadersRequestBuilder AnyVerb();
            IHeadersRequestBuilder Verb(string verb);
        }

        public interface IHeadersRequestBuilder : IBodyRequestBuilder
        {
            IHeadersRequestBuilder WithHeader(string name, string value);
        }

        public interface IBodyRequestBuilder : IRequestResponseBuilder
        {
            IRequestResponseBuilder WithBody(string body);
        }

        public interface IRequestResponseBuilder
        {
            IStatusCodeResponseBuilder Respond();

        }

        public interface IStatusCodeResponseBuilder : IHeadersResponseBuilder
        {
            IHeadersResponseBuilder WithStatusCode(int code);
        }

        public interface IHeadersResponseBuilder : IBodyResponseBuilder
        {
            IHeadersResponseBuilder WithHeader(string name, string value);

        }

        public interface IBodyResponseBuilder
        {
            void WithBody(string body);

        }
    }
}
