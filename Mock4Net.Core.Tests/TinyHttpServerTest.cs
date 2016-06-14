﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mock4Net.Core.Http;
using NFluent;
using NUnit.Framework;

namespace Mock4Net.Core.Tests.Http
{
    [TestFixture]
    public class TinyHttpServerTest
    {

        [Test]
        public void Should_Call_Handler_on_Request()
        {
            // given
            var port = Ports.FindFreeTcpPort();
            bool called = false;
            var urlPrefix = "http://localhost:" + port + "/";
            var server = new TinyHttpServer();
            server.Start(urlPrefix, ctx => called = true);
            // when
            var httpClient = new HttpClient();
            httpClient.GetAsync(urlPrefix).Wait(3000);
            // then
            Check.That(called).IsTrue();
        }
    }
}
