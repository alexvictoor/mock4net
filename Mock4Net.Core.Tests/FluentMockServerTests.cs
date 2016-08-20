using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using NFluent;
using NUnit.Framework;

namespace Mock4Net.Core.Tests
{
    [TestFixture]
    [Timeout(5000)]
    public class FluentMockServerTests
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
        public async void Should_find_a_request_satisfying_a_request_spec()
        {
            // given
            _server = FluentMockServer.Start();

            // when
            await new HttpClient().GetAsync("http://localhost:" + _server.Port + "/foo");
            await new HttpClient().GetAsync("http://localhost:" + _server.Port + "/bar");

            // then
            var result = _server.SearchLogsFor(Requests.WithUrl("/b*")); 
            Check.That(result).HasSize(1);
            var requestLogged = result.First();
            Check.That(requestLogged.Url).IsEqualTo("/bar");
        }

        [Test]
        public async void Should_reset_requestlogs()
        {
            // given
            _server = FluentMockServer.Start();

            // when
            await new HttpClient().GetAsync("http://localhost:" + _server.Port + "/foo");
            _server.Reset();

            // then
            Check.That(_server.RequestLogs).IsEmpty();
        }

        [Test]
        public async void Should_reset_routes()
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
            _server.Reset();

            // then
            Check.ThatAsyncCode(() => new HttpClient().GetStringAsync("http://localhost:" + _server.Port + "/foo"))
                .ThrowsAny();
        }

        [Test]
        public async void Should_respond_a_redirect_without_body()
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
                        .WithStatusCode(307)
                        .WithHeader("Location", "/bar")
                    );
            _server
                .Given(
                    Requests
                        .WithUrl("/bar")
                        .UsingGet())
                .RespondWith(
                    Responses
                        .WithStatusCode(200)
                        .WithBody("REDIRECT SUCCESSFUL")
                    );

            // when
            var response
                = await new HttpClient().GetStringAsync("http://localhost:" + _server.Port + "/foo");

            // then
            Check.That(response).IsEqualTo("REDIRECT SUCCESSFUL");
        }

        [Test]
        public async void Should_delay_responses_for_a_given_route()
        {
            // given
            _server = FluentMockServer.Start();

            _server
                .Given(
                    Requests
                        .WithUrl("/*")
                    )
                .RespondWith(
                    Responses
                        .WithStatusCode(200)
                        .WithBody(@"{ msg: ""Hello world!""}")
                        .AfterDelay(TimeSpan.FromMilliseconds(2000))
                    );

            // when
            var watch = new Stopwatch();
            watch.Start();
            var response
                = await new HttpClient().GetStringAsync("http://localhost:" + _server.Port + "/foo");
            watch.Stop();

            // then
            Check.That(watch.ElapsedMilliseconds).IsGreaterThan(2000);
        }

        [Test]
        public async void Should_delay_responses()
        {
            // given
            _server = FluentMockServer.Start();
            _server.AddRequestProcessingDelay(TimeSpan.FromMilliseconds(2000));
            _server
                .Given(
                    Requests
                        .WithUrl("/*")
                    )
                .RespondWith(
                    Responses
                        .WithStatusCode(200)
                        .WithBody(@"{ msg: ""Hello world!""}")
                    );

            // when
            var watch = new Stopwatch();
            watch.Start();
            var response
                = await new HttpClient().GetStringAsync("http://localhost:" + _server.Port + "/foo");
            watch.Stop();

            // then
            Check.That(watch.ElapsedMilliseconds).IsGreaterThan(2000);
        }

        [TearDown]
        public void ShutdownServer()
        {
            _server.Stop();
        }
    }
}
