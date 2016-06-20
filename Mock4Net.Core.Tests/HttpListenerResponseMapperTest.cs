using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mock4Net.Core.Http;
using Moq;
using NFluent;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Mock4Net.Core.Tests
{
    [TestFixture]
    public class HttpListenerResponseMapperTest
    {
        private TinyHttpServer _server;
        private Task<HttpResponseMessage> _responseMsgTask;

        [Test]
        public void Should_map_status_code_from_original_response()
        {
            // given
            var response = new Response() {StatusCode = 404};
            var httpListenerResponse = CreateHttpListenerResponse();
            // when
            new HttpListenerResponseMapper().Map(response, httpListenerResponse);
            // then
            Check.That(httpListenerResponse.StatusCode).IsEqualTo(404);
        }

        [Test]
        public void Should_map_headers_from_original_response()
        {
            // given
            var response = new Response();
            response.AddHeader("cache-control", "no-cache");
            var httpListenerResponse = CreateHttpListenerResponse();
            // when
            new HttpListenerResponseMapper().Map(response, httpListenerResponse);
            // then
            Check.That(httpListenerResponse.Headers.Keys).Contains("cache-control");
            Check.That(httpListenerResponse.Headers.Get("cache-control")).Contains("no-cache");
        }

        [Test]
        public void Should_map_body_from_original_response()
        {
            // given
            var response = new Response();
            response.Body = "Hello !!!";
            var httpListenerResponse = CreateHttpListenerResponse();
            // when
            new HttpListenerResponseMapper().Map(response, httpListenerResponse);
            // then
            var responseMessage = ToResponseMessage(httpListenerResponse);
            Check.That(responseMessage).IsNotNull();
            var contentTask = responseMessage.Content.ReadAsStringAsync();
            Check.That(contentTask.Result).IsEqualTo("Hello !!!");
        }

        [TearDown]
        public void StopServer()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }



        /// <summary>
        /// Dirty HACK to get HttpListenerResponse instances
        /// </summary>
        public HttpListenerResponse CreateHttpListenerResponse()
        {
            var port = Ports.FindFreeTcpPort();
            var urlPrefix = "http://localhost:" + port + "/";
            var responseReady = new AutoResetEvent(false);
            HttpListenerResponse response = null;
            _server = new TinyHttpServer();


            var mockMockServer = new Mock<IFluentMockServer>();
            mockMockServer.Setup(fluentServer => fluentServer.HandleRequest(It.IsAny<HttpListenerContext>()))
                   .Callback((HttpListenerContext context) =>
                   {
                       response = context.Response;
                       responseReady.Set();
                   });

            
            _server.Start(urlPrefix, mockMockServer.Object);
            _responseMsgTask = new HttpClient().GetAsync(urlPrefix);
            responseReady.WaitOne();
            return response;
        }

        public HttpResponseMessage ToResponseMessage(HttpListenerResponse listenerResponse)
        {
            listenerResponse.Close();
            _responseMsgTask.Wait();
            return _responseMsgTask.Result;
        }

    }
}
