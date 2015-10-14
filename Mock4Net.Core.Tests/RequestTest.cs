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
    public class RequestTest
    {

        [Test]
        public void Should_handle_empty_query()
        {
            // given
            var request = new Request("/foo", "", "blabla", "whatever", new Dictionary<string, string>());
            // then
            Check.That(request.GetParameter("foo")).IsEmpty();
        }

        [Test]
        public void Should_parse_query_params()
        {
            // given
            var request = new Request("/foo", "foo=bar&multi=1&multi=2", "blabla", "whatever", new Dictionary<string, string>());
            // then
            Check.That(request.GetParameter("foo")).Contains("bar");
            Check.That(request.GetParameter("multi")).Contains("1");
            Check.That(request.GetParameter("multi")).Contains("2");
        }
    }
}
