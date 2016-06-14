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
            var server = FluentMockServer.Start(new TinyHttpServer(), 888);

            var requestSpec = Requests.WithUrl("/HelloWorld")
               .UsingGet();

            var responseSpec = Responses
                .WithStatusCode((int)HttpStatusCode.OK)
                .WithBody(@"{ msg: ""Hello world!""}");


            server.Given(requestSpec)
                .RespondWith(responseSpec);

            Console.ReadKey();
        }
    }
}
