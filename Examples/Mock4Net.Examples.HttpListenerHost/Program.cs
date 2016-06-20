﻿using System;
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


            var apiKey = "06E9D3E64FB7E7C8B2C554A634792624";
            var httpServer = OwinSelfHostedHttpServer.New(apiKey);
            
            var mockServer = FluentMockServer.Start(httpServer, 8666);

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
