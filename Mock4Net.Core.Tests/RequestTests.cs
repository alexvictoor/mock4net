﻿using System.Collections.Generic;

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
    public class RequestTests
    {
        [Test]
        public void Should_handle_empty_query()
        {
            // given
            var request = new Request("/foo", string.Empty, "blabla", "whatever", new Dictionary<string, string>());

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
