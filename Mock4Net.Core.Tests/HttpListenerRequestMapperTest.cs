using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mock4Net.Core.Http;
using NFluent;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Mock4Net.Core.Tests
{
    [TestFixture]
    public class HttpListenerRequestMapperTest
    {

        private MapperServer _server;

        [SetUp]
        public void StartListenerServer()
        {
            _server = MapperServer.Start();
        }
        
        [Test]
        public async void Should_map_uri_from_listener_request()
        {
            // given
            var client  = new HttpClient();
            // when 
            await client.GetAsync(MapperServer.UrlPrefix + "toto");
            // then
            Check.That(MapperServer.LastRequest).IsNotNull();
            Check.That(MapperServer.LastRequest.Url).IsEqualTo("/toto");

        }

        [Test]
        public async void Should_map_verb_from_listener_request()
        {
            // given
            var client = new HttpClient();
            // when 
            await client.PutAsync(MapperServer.UrlPrefix, new StringContent("Hello!"));
            // then
            Check.That(MapperServer.LastRequest).IsNotNull();
            Check.That(MapperServer.LastRequest.Verb).IsEqualTo("put");

        }

        [Test]
        public async void Should_map_body_from_listener_request()
        {
            // given
            var client = new HttpClient();
            // when 
            await client.PutAsync(MapperServer.UrlPrefix, new StringContent("Hello!"));
            // then
            Check.That(MapperServer.LastRequest).IsNotNull();
            Check.That(MapperServer.LastRequest.Body).IsEqualTo("Hello!");

        }

        [Test]
        public async void Should_map_headers_from_listener_request()
        {
            // given
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Alex", "1706");
            // when 
            await client.GetAsync(MapperServer.UrlPrefix);
            // then
            Check.That(MapperServer.LastRequest).IsNotNull();
            Check.That(MapperServer.LastRequest.Headers).Not.IsNullOrEmpty();
            Check.That(MapperServer.LastRequest.Headers.Contains(new KeyValuePair<string, string>("x-alex", "1706"))).IsTrue();

        }

        [Test]
        public async void Should_map_params_from_listener_request()
        {
            // given
            var client = new HttpClient();
            // when 
            await client.GetAsync(MapperServer.UrlPrefix + "index.html?id=toto");
            // then
            Check.That(MapperServer.LastRequest).IsNotNull();
            Check.That(MapperServer.LastRequest.Path).EndsWith("/index.html");
            Check.That(MapperServer.LastRequest.GetParameter("id")).HasSize(1);

        }

        [TearDown]
        public void StopListenerServer()
        {
            _server.Stop();
        }

        class MapperServer : TinyHttpServer
        {
            public static volatile Request LastRequest;
            public static string UrlPrefix;

            private MapperServer(string urlPrefix, Action<HttpListenerContext> httpHandler) : base(urlPrefix, httpHandler)
            {
            }

            public new static MapperServer Start()
            {
                var port = Ports.FindFreeTcpPort();
                UrlPrefix = "http://localhost:" + port + "/";
                var server = new MapperServer(UrlPrefix, context =>
                {
                    LastRequest = new HttpListenerRequestMapper().Map(context.Request);
                    context.Response.Close();
                });
                ((TinyHttpServer) server).Start();
                return server;
            }

            public new void Stop()
            {
                base.Stop();
                LastRequest = null;
            }
        }
    }
}
