using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mock4Net.ApiClient.Models;

namespace Mock4Net.ApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //var baseAddress = new Uri("http://vmob-dev-2-workflow-mock4net.azurewebsites.net/");
            var baseAddress = new Uri("http://localhost:43420/");
            var clientAddress = new UriBuilder(baseAddress);
            clientAddress.Path = "api/Mockserver";
            var client = new Mock4NetAPIClient(clientAddress.ToString());

            //var client = new Mock4NetAPIClient("http://localhost:8080/api/Mockserver");



            Console.WriteLine("Resetting mocks on mock server...");
            client.Reset();


            //Create the request and response conditions using
            var requestCondition = RequestConditionBuilder
                .WithUrl("/HelloWorld")
                .WithVerb(RequestVerb.Get);


            var response = ResponseBuilder
                .WithStatusCode(200)
                .WithBody(@"{ msg: ""Hello world!""}")
                .WithHeader("x-MyHeader", "Hi!");



            Console.WriteLine("Adding Hello world Mock...");
            client.AddMock(requestCondition, response);

            var requestCondition2 = RequestConditionBuilder
             .WithUrl("/HelloWorld2")
             .WithVerb(RequestVerb.Get);


            var response2 = ResponseBuilder
                .WithStatusCode(200)
                .WithBody(@"{ msg: ""Hello world2!""}")
                .WithHeader("x-MyHeader", "Hi2!");



            Console.WriteLine("Adding Hello world Mock...");
            client.AddMock(requestCondition2, response2);


            // Console.WriteLine("Getting Mock Server Address...");
            // var mockServerAddress = "http://localhost:8666";//client.GetMockServerAddress();



            // Console.WriteLine($"Address is: {mockServerAddress}");


            var url = new UriBuilder(baseAddress);
            url.Path = requestCondition2.URl;

            Console.WriteLine($"Calling hello world mock on the mock server: {url.Uri}");
            var responseBody = new HttpClient().GetStringAsync(url.Uri);

            Console.WriteLine($"Response was {responseBody.Result}");


            Console.WriteLine("Getting the request logs from the server");
            var logs = client.SearchLogsFor(requestCondition);


            Console.WriteLine("Done!");

            Console.ReadKey();
        }
    }
}
