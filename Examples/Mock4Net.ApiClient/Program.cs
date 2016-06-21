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
            var apiKey = "06E9D3E64FB7E7C8B2C554A634792624";

            var baseAddress = new Uri("http://vmob-dev-2-workflow-mock4net.azurewebsites.net/");
            //var baseAddress = new Uri("http://localhost:11824/");
            var clientAddress = new UriBuilder(baseAddress);
            var client = new Mock4NetAPIClient(clientAddress.ToString(), apiKey);

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

            var url = new UriBuilder(baseAddress);
            url.Path = requestCondition.URl;

            Console.WriteLine($"Calling hello world mock on the mock server: {url.Uri}");
            var responseBody = new HttpClient().GetStringAsync(url.Uri);
            Console.WriteLine($"Response was {responseBody.Result}");


            Console.WriteLine("Getting the request logs from the server");
            var logs = client.SearchLogsFor(requestCondition);
            Console.WriteLine($"Found {logs.Count} logs for the request");

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
