using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;

namespace Mock4Net.Core.Tests
{
    [TestFixture]
    [Timeout(5000)]
    public class FluentMockServerTest
    {
        private FluentMockServer _server;

        [Test]
        public async void Should_respond_to_request()
        {
            // given
            _server = FluentMockServer.Start();

            _server
                .Given(
                    Requests
                        .WithUrl("/foo")
                        .UsingGet())
                .RespondWith(
                    Responses
                        .WithStatusCode(200)
                        .WithBody(@"{ msg: ""Hello world!""}")
                    );

            // when
            var response 
                = await new HttpClient().GetStringAsync("http://localhost:" + _server.Port + "/foo");
            // then
            Check.That(response).IsEqualTo(@"{ msg: ""Hello world!""}");
        }

        [Test]
        public async void Should_respond_404_for_unexpected_request()
        {
            // given
            _server = FluentMockServer.Start();
            // when
            var response
                = await new HttpClient().GetAsync("http://localhost:" + _server.Port + "/foo");
            // then
            Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
            Check.That((int)response.StatusCode).IsEqualTo(404);
        }

        [Test]
        public async void Should_record_requests_in_the_requestlogs()
        {
            // given
            _server = FluentMockServer.Start();
            // when
            await new HttpClient().GetAsync("http://localhost:" + _server.Port + "/foo");
            // then
            Check.That(_server.RequestLogs).HasSize(1);
            var requestLogged = _server.RequestLogs.First();
            Check.That(requestLogged.Verb).IsEqualTo("get");
            Check.That(requestLogged.Body).IsEmpty();

        }

        [Test]
        public async void Should_find_a_requests_satisfying_a_request_spec()
        {
            // given
            _server = FluentMockServer.Start();
            // when
            await new HttpClient().GetAsync("http://localhost:" + _server.Port + "/foo");
            await new HttpClient().GetAsync("http://localhost:" + _server.Port + "/bar");
            // then
            var result = _server.SearchRequestLogsFor(Requests.WithUrl("/b*")); 
            Check.That(result).HasSize(1);
            var requestLogged = result.First();
            Check.That(requestLogged.Url).IsEqualTo("/bar");

        }

        [Test]
        public async void Should_reset_the_requestlogs()
        {
            // given
            _server = FluentMockServer.Start();
            // when
            await new HttpClient().GetAsync("http://localhost:" + _server.Port + "/foo");
            _server.ResetRequestLogs();
            // then
            Check.That(_server.RequestLogs).IsEmpty();

        }

        [TearDown]
        public void ShutdownServer()
        {
            _server.Stop();
        }
    }
}
