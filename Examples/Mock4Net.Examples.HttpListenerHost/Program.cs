using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Mock4Net.Core;
using Mock4Net.Core.Http;

namespace Mock4Net.Examples.HttpListenerHost
{
    class Program
    {
        static void Main(string[] args)
        {

            //var httpServer = new TinyHttpServer();
            var httpServer = new OwinHttpServer();

            var mockServer = FluentMockServer.Start(httpServer, 888);

            var requestSpec = Requests.WithUrl("/HelloWorld")
               .UsingGet();

            var responseSpec = Responses
                .WithStatusCode((int)HttpStatusCode.OK)
                .WithBody(@"{ msg: ""Hello world!""}");


            mockServer.Given(requestSpec)
                .RespondWith(responseSpec);

            Console.ReadKey();
        }
    }
}
