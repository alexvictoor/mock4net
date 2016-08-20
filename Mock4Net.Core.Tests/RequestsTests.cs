using System.Collections.Generic;
using NFluent;
using NUnit.Framework;

[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here, as it's a tests class.")]
[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1633:FileMustHaveHeader",
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]
// ReSharper disable InconsistentNaming
namespace Mock4Net.Core.Tests
{
    [TestFixture]
    public class RequestsTests
    {
        [Test]
        public void Should_specify_requests_matching_given_url()
        {
            // given
            var spec = Requests.WithUrl("/foo");

            // when
            var request = new Request("/foo", "", "blabla", "whatever", new Dictionary<string, string>());

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsTrue();
        }

        [Test]
        public void Should_specify_requests_matching_given_url_prefix()
        {
            // given
            var spec = Requests.WithUrl("/foo*");

            // when
            var request = new Request("/foo/bar", "", "blabla", "whatever", new Dictionary<string, string>());

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsTrue();
        }

        [Test]
        public void Should_exclude_requests_not_matching_given_url()
        {
            // given
            var spec = Requests.WithUrl("/foo");

            // when
            var request = new Request("/bar", "", "blabla", "whatever", new Dictionary<string, string>());

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsFalse();
        }

        [Test]
        public void Should_specify_requests_matching_given_path()
        {
            // given
            var spec = Requests.WithPath("/foo");

            // when
            var request = new Request("/foo", "?param=1", "blabla", "whatever", new Dictionary<string, string>());

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsTrue();
        }

        [Test]
        public void Should_specify_requests_matching_given_url_and_method()
        {
            // given
            var spec = Requests.WithUrl("/foo").UsingPut();

            // when
            var request = new Request("/foo", "", "PUT", "whatever", new Dictionary<string, string>());

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsTrue();
        }

        [Test]
        public void Should_exclude_requests_matching_given_url_but_not_http_method()
        {
            // given
            var spec = Requests.WithUrl("/foo").UsingPut();

            // when
            var request = new Request("/foo", "", "POST", "whatever", new Dictionary<string, string>());

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsFalse();
        }

        [Test]
        public void Should_exclude_requests_matching_given_http_method_but_not_url()
        {
            // given
            var spec = Requests.WithUrl("/bar").UsingPut();

            // when
            var request = new Request("/foo", "", "PUT", "whatever", new Dictionary<string, string>());

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsFalse();
        }

        [Test]
        public void Should_specify_requests_matching_given_url_and_headers()
        {
            // given
            var spec = Requests.WithUrl("/foo").UsingAnyVerb().WithHeader("X-toto", "tata");

            // when
            var request = new Request("/foo", "", "PUT", "whatever", new Dictionary<string, string>() { { "X-toto", "tata" } });
            
            // then
            Check.That(spec.IsSatisfiedBy(request)).IsTrue();
        }

        [Test]
        public void Should_exclude_requests_not_matching_given_headers()
        {
            // given
            var spec = Requests.WithUrl("/foo").UsingAnyVerb().WithHeader("X-toto", "tatata");

            // when
            var request = new Request("/foo", "", "PUT", "whatever", new Dictionary<string, string>() { { "X-toto", "tata" } });

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsFalse();
        }

        [Test]
        public void Should_specify_requests_matching_given_header_prefix()
        {
            // given
            var spec = Requests.WithUrl("/foo").UsingAnyVerb().WithHeader("X-toto", "tata*");

            // when
            var request = new Request("/foo", "", "PUT", "whatever", new Dictionary<string, string>() { { "X-toto", "tatata" } });

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsTrue();
        }

        [Test]
        public void Should_specify_requests_matching_given_body()
        {
            // given
            var spec = Requests.WithUrl("/foo").UsingAnyVerb().WithBody("      Hello world!   ");

            // when
            var request = new Request("/foo", "", "PUT", "Hello world!", new Dictionary<string, string>() { { "X-toto", "tatata" } });

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsTrue();
        }

        [Test]
        public void Should_specify_requests_matching_given_body_as_wildcard()
        {
            // given
            var spec = Requests.WithUrl("/foo").UsingAnyVerb().WithBody("H*o wor?d!");

            // when
            var request = new Request("/foo", "", "PUT", "Hello world!", new Dictionary<string, string>() { { "X-toto", "tatata" } });

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsTrue();
        }

        [Test]
        public void Should_exclude_requests_not_matching_given_body()
        {
            // given
            var spec = Requests.WithUrl("/foo").UsingAnyVerb().WithBody("      Hello world!   ");

            // when
            var request = new Request("/foo", "", "PUT", "XXXXXXXXXXX", new Dictionary<string, string>() { { "X-toto", "tatata" } });

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsFalse();
        }

        [Test]
        public void Should_specify_requests_matching_given_params()
        {
            // given
            var spec = Requests.WithPath("/foo").WithParam("bar", "1", "2");

            // when
            var request = new Request("/foo", "bar=1&bar=2", "Get", "Hello world!", new Dictionary<string, string>());

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsTrue();
        }

        [Test]
        public void Should_exclude_requests_not_matching_given_params()
        {
            // given
            var spec = Requests.WithPath("/foo").WithParam("bar", "1");

            // when
            var request = new Request("/foo", "", "PUT", "XXXXXXXXXXX", new Dictionary<string, string>());

            // then
            Check.That(spec.IsSatisfiedBy(request)).IsFalse();
        }
    }
}
