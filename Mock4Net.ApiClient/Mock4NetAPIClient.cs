﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Mock4Net.ApiClient.Models;
using Newtonsoft.Json;

namespace Mock4Net.ApiClient
{
    public class Mock
    {
        public RequestCondition RequestCondition { get; set; }
        public Response Response { get; set; }
    }

    /// <summary>
    /// APi Cient library for managing mocks on a VMob Mock4Net Server
    /// <example>
    /// <code>
    ///  var apiKey = "06E9D3E64FB7E7C8B2C554A634792624";
    ///  var baseAddress = new Uri("http://localhost:43420/");
    ///  var client = new Mock4NetAPIClient(baseAddress, apiKey)
    /// 
    /// //Reset the mocks on the server
    ///  client.Reset();
    /// 
    /// //Create the request and response conditions
    /// var requestCondition = RequestConditionBuilder
    ///    .WithUrl("/HelloWorld")
    ///    .WithVerb(RequestVerb.Get);
    ///
    ///  var response = ResponseBuilder
    ///    .WithStatusCode(200)
    ///    .WithBody(@"{ msg: ""Hello world!""}")
    ///    .WithHeader("x-MyHeader", "Hi!");
    /// 
    /// //Add the mock definition to the server
    /// client.AddMock(requestCondition, response);
    /// 
    /// // test call of the mock
    /// var url = new UriBuilder(baseAddress);
    /// url.Path = requestCondition.URl;
    /// var responseBody = new HttpClient().GetStringAsync(url.Uri);
    ///
    /// // Check the mock server logs for a matching request..
    /// var logs = client.SearchLogsFor(requestCondition);
    /// </code>
    /// </example>
    /// </summary>
    public class Mock4NetAPIClient
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public Mock4NetAPIClient(string baseUrl, string apiKey)
        {
            _apiKey = apiKey;
            var baseUri = new UriBuilder(baseUrl);

            if (!baseUri.Path.ToLower().EndsWith("api/mockserver"))
                baseUri.Path = "api/mockserver";

            _baseUrl = baseUri.ToString();
            _baseUrl = _baseUrl.Last() == '/' ? _baseUrl : _baseUrl + "/";
        }


        public void AddMock(RequestCondition requestCondition, Response response)
        {
            var mock = new Mock() {RequestCondition = requestCondition,Response = response};
            var entity = JsonConvert.SerializeObject(mock);

            var uri = GetUriForPath("AddMock");
          
            var result = HttpPost(uri, entity);
        }
       

       

        public void Reset()
        {

            var uri = GetUriForPath("ResetMocks");
            HttpPost(uri, "");
        }

        public List<Request> SearchLogsFor(RequestCondition condition)
        {
            var uri = GetUriForPath("SearchLogsFor");
            var entity = JsonConvert.SerializeObject(condition);
            var result = HttpPost(uri, entity);
            var typedResult = JsonConvert.DeserializeObject<List<Request>>(result);
            return typedResult;
        }
        private string GetUriForPath(string path)
        {
            var builder = new UriBuilder(_baseUrl);
            builder.Path += path;
            var uri = builder.Uri.ToString();
            return uri;
        }

        private string HttpPost(string URI, string entity)
        {

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(URI);
            //myReq.Credentials = new NetworkCredential("myUser", "myPassword");
            //request.CookieContainer = myContainer;
           // myReq.PreAuthenticate = true;

            byte[] byteArray = Encoding.UTF8.GetBytes(entity);
            myReq.Method = "POST";
            myReq.ContentType = "application/json";
            myReq.ContentLength = byteArray.Length;
            myReq.Headers.Add("X-MockServerControllerApiKey", _apiKey);
            Stream requestStream = myReq.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();

            using (WebResponse wr = myReq.GetResponse())
            {
                using (Stream receiveStream = wr.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                   
                }
            }

            
        }
    }
}
