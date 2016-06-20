using System;
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

    public class Mock4NetAPIClient
    {
        private readonly string _baseUrl;

        public Mock4NetAPIClient(string baseUrl)
        {
            _baseUrl = baseUrl.Last() == '/' ? baseUrl : baseUrl + "/";
        }


        public void AddMock(RequestCondition requestCondition, Response response)
        {
            var mock = new Mock() {RequestCondition = requestCondition,Response = response};
            var entity = JsonConvert.SerializeObject(mock);

            var uri = GetUriForPath("AddMock");
          
            var result = HttpPost(uri, entity);
        }
        public string GetMockServerAddress()
        {
            var uri = GetUriForPath("GetMockServerAddress");

            var result = HttpPost(uri, "");

            var typedResult = JsonConvert.DeserializeObject<string>(result);
            var apiUri = new Uri(_baseUrl);
            var mockeServerUri = new UriBuilder(typedResult);
            mockeServerUri.Host = apiUri.Host;
            
            return mockeServerUri.Uri.ToString();
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

        private static string HttpPost(string URI, string entity)
        {

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(URI);


            byte[] byteArray = Encoding.UTF8.GetBytes(entity);
            myReq.Method = "POST";
            myReq.ContentType = "application/json";
            myReq.ContentLength = byteArray.Length;
            myReq.Headers.Add("X-MockServerControllerApiKey", "ANUSNDLJNS9343y94ndfNDJFNJDK383nSNS");
            Stream requestStream = myReq.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();

            using (WebResponse wr = myReq.GetResponse())
            {
                HttpWebResponse httpResponse = (HttpWebResponse)wr;

                using (Stream receiveStream = wr.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                   
                }
            }

            
        }
    }
}
