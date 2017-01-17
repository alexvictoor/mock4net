using System.Diagnostics.CodeAnalysis;

[module:
    SuppressMessage("StyleCop.CSharp.DocumentationRules", 
        "SA1600:ElementsMustBeDocumented", 
        Justification = "Reviewed. Suppression is OK here, as it's a tests class.")]
[module:
    SuppressMessage("StyleCop.CSharp.DocumentationRules", 
        "SA1633:FileMustHaveHeader", 
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]

namespace Mock4Net.Core.Tests.Http
{
    using System.Net.Http;

    using Mock4Net.Core.Http;

    using NFluent;

    using NUnit.Framework;

    [TestFixture]
    public class TinyHttpServerTests
    {
        [Test]
        public void Should_Call_Handler_on_Request()
        {
            // given
            var port = Ports.FindFreeTcpPort();
            bool called = false;
            var urlPrefix = "http://localhost:" + port + "/";
            var server = new TinyHttpServer(urlPrefix, ctx => called = true);
            server.Start();

            // when
            var httpClient = new HttpClient();
            httpClient.GetAsync(urlPrefix).Wait(3000);

            // then
            Check.That(called).IsTrue();
        }
    }
}
