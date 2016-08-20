using System.Linq;
using System.Net;
using System.Text;

namespace Mock4Net.Core
{
    public class HttpListenerResponseMapper
    {
        public void Map(Response response, HttpListenerResponse result)
        {
            result.StatusCode = response.StatusCode;
            response.Headers.ToList().ForEach(pair => result.AddHeader(pair.Key, pair.Value));
            if (response.Body != null)
            {
                var content = Encoding.UTF8.GetBytes(response.Body);
                result.OutputStream.Write(content, 0, content.Length);    
            }
        }
    }
}
