using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NFluent;
using NUnit.Framework;

namespace Mock4Net.Core.Tests
{
    [TestFixture]
    public class RouteBuilderTest
    {
        private Route _route;


        [Test]
        public void Should_build_a_route_that_handles_a_request_if_url_matches()
        {
            // given
            var builder = new RouteBuilder(route => _route = route, "/foo");
            // when
            builder.AnyVerb().Respond(); // set _route
            // then
            var request = new Request("/foo", "blabla", "whatever", new Dictionary<string, string>());
            Check.That(_route.IsRequestHandled(request)).IsTrue();
        }

        [Test]
        public void Should_build_a_route_that_doesnt_handle_a_request_if_url_doesnt_match()
        {
            // given
            var builder = new RouteBuilder(route => _route = route, "/foo");
            // when
            builder.AnyVerb().Respond(); // set _route
            // then
            var request = new Request("/bar", "blabla", "whatever", new Dictionary<string, string>());
            Check.That(_route.IsRequestHandled(request)).IsFalse();
        }


        [Test]
        public void Should_build_a_route_that_handles_a_request_if_url_and_http_method_match()
        {
            // given
            var builder = new RouteBuilder(route => _route = route, "/foo");
            // when
            builder.Put().Respond(); // set _route
            // then
            var request = new Request("/foo", "PUT", "whatever", new Dictionary<string, string>());
            Check.That(_route.IsRequestHandled(request)).IsTrue();
        }

        [Test]
        public void Should_build_a_route_that_doesnt_handle_a_request_if_url_match_but_not_http_method()
        {
            // given
            var builder = new RouteBuilder(route => _route = route, "/foo");
            // when
            builder.Put().Respond(); // set _route
            // then
            var request = new Request("/foo", "post", "whatever", new Dictionary<string, string>());
            Check.That(_route.IsRequestHandled(request)).IsFalse();
        }

        [Test]
        public void Should_build_a_route_that_doesnt_handle_a_request_if_url_doesnt_match_while_http_method_does()
        {
            // given
            var builder = new RouteBuilder(route => _route = route, "/bar");
            // when
            builder.Put().Respond(); // set _route
            // then
            var request = new Request("/foo", "PUT", "whatever", new Dictionary<string, string>());
            Check.That(_route.IsRequestHandled(request)).IsFalse();
        }

        [Test]
        public void Should_build_a_route_that_handles_a_request_if_url_and_headers_match()
        {
            // given
            FluentMockServer.IVerbRequestBuilder builder = new RouteBuilder(route => _route = route, "/foo");
            // when
            builder.AnyVerb().WithHeader("X-toto", "tata").Respond(); // set _route
            // then
            var request = new Request("/foo", "put", "whatever", new Dictionary<string, string>() { { "X-toto", "tata" } });
            Check.That(_route.IsRequestHandled(request)).IsTrue();
        }

        [Test]
        public void Should_build_a_route_that_doesnt_handle_a_request_if_headers_dont_match()
        {
            // given
            FluentMockServer.IVerbRequestBuilder builder = new RouteBuilder(route => _route = route, "/foo");
            // when
            builder.AnyVerb().WithHeader("X-toto", "tatata").Respond(); // set _route
            // then
            var request = new Request("/foo", "put", "whatever", new Dictionary<string, string>() { { "X-toto", "tata" } });
            Check.That(_route.IsRequestHandled(request)).IsFalse();
        }

        [Test]
        public void Should_build_a_route_that_handles_a_request_if_body_matches()
        {
            // given
            FluentMockServer.IVerbRequestBuilder builder = new RouteBuilder(route => _route = route, "/foo");
            // when
            builder.AnyVerb().WithBody("  Hello World!  ").Respond(); // set _route
            // then
            var request = new Request("/foo", "put", " Hello World!", new Dictionary<string, string>() { { "X-toto", "tata" } });
            Check.That(_route.IsRequestHandled(request)).IsTrue();
        }

        [Test]
        public void Should_build_a_route_that_doesnt_handle_a_request_if_body_matches()
        {
            // given
            FluentMockServer.IVerbRequestBuilder builder = new RouteBuilder(route => _route = route, "/foo");
            // when
            builder.AnyVerb().WithBody("  Hello World!  ").Respond(); // set _route
            // then
            var request = new Request("/foo", "put", " XXXXXXXX", new Dictionary<string, string>() { { "X-toto", "tata" } });
            Check.That(_route.IsRequestHandled(request)).IsFalse();
        }
    }
}
